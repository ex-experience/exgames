using UnityEngine;

namespace EX360.CameraSystem
{
    public sealed class OrbitCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 10.5f, -11.5f);
        public float smooth = 8f;

        public void SetTarget(Transform t) => target = t;

        void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, 1f - Mathf.Exp(-smooth * Time.deltaTime));
            Vector3 look = target.position + Vector3.up * 1.2f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look - transform.position), 1f - Mathf.Exp(-smooth * Time.deltaTime));
        }
    }
}
