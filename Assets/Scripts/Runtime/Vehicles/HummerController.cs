using UnityEngine;
using EX360.Input;
using EX360.Player;
using EX360.CameraSystem;

namespace EX360.Vehicles
{
    public sealed class HummerController : MonoBehaviour
    {
        public CrossPlatformInput input;
        public PlayerMotor player;
        public PlayerCombat combat;
        public OrbitCamera orbit;
        bool occupied;
        CharacterController playerController;
        Renderer[] playerRenderers;

        void Start()
        {
            if (player != null)
            {
                playerController = player.GetComponent<CharacterController>();
                playerRenderers = player.GetComponentsInChildren<Renderer>();
            }
        }

        void Update()
        {
            if (input == null || player == null) return;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (!occupied && distance < 3.2f && input.UsePressed) Enter();
            else if (occupied && input.UsePressed) Exit();

            if (!occupied) return;
            Vector3 move = new Vector3(input.Move.x, 0f, input.Move.y);
            if (move.sqrMagnitude > 0.02f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 7f * Time.deltaTime);
                transform.position += transform.forward * (11f * input.Move.magnitude * Time.deltaTime);
            }
        }

        void Enter()
        {
            occupied = true;
            player.enabled = false;
            if (combat != null) combat.enabled = false;
            if (playerController != null) playerController.enabled = false;
            if (playerRenderers != null) foreach (var r in playerRenderers) r.enabled = false;
            if (orbit != null) orbit.SetTarget(transform);
        }

        void Exit()
        {
            occupied = false;
            if (playerController != null) playerController.enabled = false;
            player.transform.position = transform.position + transform.right * 2.2f + Vector3.up * 0.3f;
            if (playerController != null) playerController.enabled = true;
            player.enabled = true;
            if (combat != null) combat.enabled = true;
            if (playerRenderers != null) foreach (var r in playerRenderers) r.enabled = true;
            if (orbit != null) orbit.SetTarget(player.transform);
        }
    }
}
