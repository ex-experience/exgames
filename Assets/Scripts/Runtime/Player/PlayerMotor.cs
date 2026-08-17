using UnityEngine;
using EX360.Core;
using EX360.Input;

namespace EX360.Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        public CrossPlatformInput input;
        public Camera gameplayCamera;
        CharacterController controller;
        float verticalVelocity;
        float dashCooldown;
        Vector3 spawn;

        public Vector3 SpawnPoint { get => spawn; set => spawn = value; }

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            spawn = transform.position;
        }

        void Update()
        {
            if (input == null || gameplayCamera == null || !controller.enabled) return;
            dashCooldown -= Time.deltaTime;
            Vector3 f = gameplayCamera.transform.forward; f.y = 0f; f.Normalize();
            Vector3 r = gameplayCamera.transform.right; r.y = 0f; r.Normalize();
            Vector3 wish = Vector3.ClampMagnitude(r * input.Move.x + f * input.Move.y, 1f);

            float speed = GameConfig.PlayerMoveSpeed;
            if (input.DashPressed && dashCooldown <= 0f)
            {
                speed *= 2.4f;
                dashCooldown = 0.8f;
            }

            if (controller.isGrounded)
            {
                if (verticalVelocity < 0f) verticalVelocity = -2f;
                if (input.JumpPressed)
                    verticalVelocity = Mathf.Sqrt(GameConfig.PlayerJumpHeight * -2f * GameConfig.PlayerGravity);
            }
            verticalVelocity += GameConfig.PlayerGravity * Time.deltaTime;

            Vector3 motion = wish * speed;
            motion.y = verticalVelocity;
            controller.Move(motion * Time.deltaTime);

            if (wish.sqrMagnitude > 0.02f && !input.FireHeld)
            {
                Quaternion target = Quaternion.LookRotation(wish, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, 14f * Time.deltaTime);
            }

            if (transform.position.y < -5f) Respawn();
        }

        public void Respawn()
        {
            bool was = controller.enabled;
            controller.enabled = false;
            transform.position = spawn;
            verticalVelocity = 0f;
            controller.enabled = was;
        }
    }
}
