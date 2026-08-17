using UnityEngine;

namespace EX360.Input
{
    public sealed class CrossPlatformInput : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public bool FireHeld { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool DashPressed { get; private set; }
        public bool GrenadePressed { get; private set; }
        public bool UsePressed { get; private set; }
        public bool IsTouchDevice => UnityEngine.Input.touchSupported;

        Vector2 touchMove;
        Vector2 touchAim;
        int leftFinger = -1;
        int rightFinger = -1;
        Vector2 leftStart;
        Vector2 rightStart;
        Vector3 lastAimDirection = Vector3.forward;

        void Update()
        {
            JumpPressed = false;
            DashPressed = false;
            GrenadePressed = false;
            UsePressed = false;

            ReadDesktopAndGamepad();
            if (UnityEngine.Input.touchSupported && UnityEngine.Input.touchCount > 0)
                ReadTouch();
        }

        void ReadDesktopAndGamepad()
        {
            float x = 0f;
            float y = 0f;
            if (UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow)) x -= 1f;
            if (UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow)) x += 1f;
            if (UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow)) y -= 1f;
            if (UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow)) y += 1f;

            // Unity's default Horizontal/Vertical axes also cover many gamepads.
            try
            {
                var axis = new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical"));
                if (axis.sqrMagnitude > new Vector2(x, y).sqrMagnitude) { x = axis.x; y = axis.y; }
            }
            catch { }

            Move = Vector2.ClampMagnitude(new Vector2(x, y), 1f);
            FireHeld = UnityEngine.Input.GetMouseButton(0) || UnityEngine.Input.GetKey(KeyCode.J) || SafeGetButton("Fire1");
            JumpPressed |= UnityEngine.Input.GetKeyDown(KeyCode.Space) || SafeGetButtonDown("Jump");
            DashPressed |= UnityEngine.Input.GetKeyDown(KeyCode.LeftShift) || UnityEngine.Input.GetKeyDown(KeyCode.K);
            GrenadePressed |= UnityEngine.Input.GetKeyDown(KeyCode.G) || UnityEngine.Input.GetMouseButtonDown(1);
            UsePressed |= UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.Return);
        }

        static bool SafeGetButton(string name)
        {
            try { return UnityEngine.Input.GetButton(name); } catch { return false; }
        }

        static bool SafeGetButtonDown(string name)
        {
            try { return UnityEngine.Input.GetButtonDown(name); } catch { return false; }
        }

        void ReadTouch()
        {
            FireHeld = false;
            float w = Screen.width;
            float h = Screen.height;
            float buttonR = Mathf.Min(w, h) * 0.075f;
            Vector2 fireCenter = new Vector2(w * 0.88f, h * 0.20f);
            Vector2 jumpCenter = new Vector2(w * 0.74f, h * 0.17f);
            Vector2 grenadeCenter = new Vector2(w * 0.88f, h * 0.38f);
            Vector2 useCenter = new Vector2(w * 0.74f, h * 0.36f);

            foreach (Touch t in UnityEngine.Input.touches)
            {
                Vector2 p = t.position;
                if (Vector2.Distance(p, fireCenter) <= buttonR) { FireHeld = true; continue; }
                if (Vector2.Distance(p, jumpCenter) <= buttonR && t.phase == TouchPhase.Began) { JumpPressed = true; continue; }
                if (Vector2.Distance(p, grenadeCenter) <= buttonR && t.phase == TouchPhase.Began) { GrenadePressed = true; continue; }
                if (Vector2.Distance(p, useCenter) <= buttonR && t.phase == TouchPhase.Began) { UsePressed = true; continue; }

                if (t.phase == TouchPhase.Began)
                {
                    if (p.x < w * 0.46f && leftFinger < 0) { leftFinger = t.fingerId; leftStart = p; }
                    else if (rightFinger < 0) { rightFinger = t.fingerId; rightStart = p; }
                }

                if (t.fingerId == leftFinger)
                {
                    touchMove = Vector2.ClampMagnitude((p - leftStart) / (Mathf.Min(w, h) * 0.16f), 1f);
                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) { leftFinger = -1; touchMove = Vector2.zero; }
                }
                else if (t.fingerId == rightFinger)
                {
                    touchAim = Vector2.ClampMagnitude((p - rightStart) / (Mathf.Min(w, h) * 0.12f), 1f);
                    if (touchAim.sqrMagnitude > 0.04f) FireHeld = true;
                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) { rightFinger = -1; touchAim = Vector2.zero; }
                }
            }

            if (leftFinger >= 0) Move = touchMove;
        }

        public Vector3 GetAimDirection(Camera cam, Vector3 origin)
        {
            if (cam == null) return lastAimDirection;

            if (rightFinger >= 0 && touchAim.sqrMagnitude > 0.04f)
            {
                Vector3 f = cam.transform.forward; f.y = 0f; f.Normalize();
                Vector3 r = cam.transform.right; r.y = 0f; r.Normalize();
                lastAimDirection = (r * touchAim.x + f * touchAim.y).normalized;
                return lastAimDirection;
            }

            if (!UnityEngine.Input.touchSupported || UnityEngine.Input.touchCount == 0)
            {
                Ray ray = cam.ScreenPointToRay(UnityEngine.Input.mousePosition);
                Plane plane = new Plane(Vector3.up, new Vector3(0f, origin.y, 0f));
                if (plane.Raycast(ray, out float enter))
                {
                    Vector3 hit = ray.GetPoint(enter);
                    Vector3 d = hit - origin; d.y = 0f;
                    if (d.sqrMagnitude > 0.01f) lastAimDirection = d.normalized;
                }
            }

            if (lastAimDirection.sqrMagnitude < 0.01f && Move.sqrMagnitude > 0.01f)
            {
                Vector3 f = cam.transform.forward; f.y = 0f; f.Normalize();
                Vector3 r = cam.transform.right; r.y = 0f; r.Normalize();
                lastAimDirection = (r * Move.x + f * Move.y).normalized;
            }
            return lastAimDirection;
        }
    }
}
