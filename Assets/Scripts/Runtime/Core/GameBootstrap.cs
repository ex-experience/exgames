using UnityEngine;
using EX360.Input;
using EX360.World;
using EX360.CameraSystem;
using EX360.Mission;
using EX360.Performance;
using EX360.UI;
using EX360.Player;

namespace EX360.Core
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        static bool booted;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void AutoBoot()
        {
            if (booted || Object.FindObjectOfType<GameBootstrap>() != null) return;
            var go = new GameObject("EX360_BOOTSTRAP");
            go.AddComponent<GameBootstrap>();
        }

        void Awake()
        {
            if (booted) { Destroy(gameObject); return; }
            booted = true;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = GameConfig.TargetFps;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            BuildRuntime();
        }

        void BuildRuntime()
        {
            var input = gameObject.AddComponent<CrossPlatformInput>();
            var mission = gameObject.AddComponent<MissionDirector>();
            var quality = gameObject.AddComponent<AdaptiveQuality>();

            var cameraGo = new GameObject("GameplayCamera");
            cameraGo.tag = "MainCamera";
            var camera = cameraGo.AddComponent<Camera>();
            camera.fieldOfView = 57f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 140f;
            var orbit = cameraGo.AddComponent<OrbitCamera>();

            var world = gameObject.AddComponent<WorldGenerator>();
            world.input = input;
            world.mission = mission;
            world.orbit = orbit;
            PlayerMotor player = world.Generate();
            orbit.SetTarget(player.transform);
            player.gameplayCamera = camera;
            var combat = player.GetComponent<PlayerCombat>();
            combat.gameplayCamera = camera;

            var hud = gameObject.AddComponent<HudOverlay>();
            hud.playerHealth = player.GetComponent<EX360.Combat.Health>();
            hud.mission = mission;
            hud.quality = quality;

            Debug.Log($"[EX360] Booted {GameConfig.ProductName} {GameConfig.Version}");
        }
    }
}
