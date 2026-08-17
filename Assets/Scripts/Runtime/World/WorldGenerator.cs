using UnityEngine;
using EX360.Core;
using EX360.Combat;
using EX360.Player;
using EX360.Input;
using EX360.AI;
using EX360.CameraSystem;
using EX360.Mission;
using EX360.Vehicles;

namespace EX360.World
{
    public sealed class WorldGenerator : MonoBehaviour
    {
        public CrossPlatformInput input;
        public MissionDirector mission;
        public OrbitCamera orbit;
        Material floorMat, coverMat, enemyMat, playerMat, bossMat, vehicleMat;

        public PlayerMotor Generate()
        {
            Random.InitState(1996);
            CreateMaterials();
            CreateArena();
            var player = CreatePlayer();
            CreateEnemies(player.transform);
            CreateBoss(player.transform);
            CreateVehicle(player);
            return player;
        }

        void CreateMaterials()
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Legacy Shaders/Diffuse");
            floorMat = NewMat(shader, new Color(0.18f, 0.17f, 0.14f));
            coverMat = NewMat(shader, new Color(0.45f, 0.32f, 0.17f));
            enemyMat = NewMat(shader, new Color(0.56f, 0.08f, 0.06f));
            playerMat = NewMat(shader, new Color(0.05f, 0.45f, 0.9f));
            bossMat = NewMat(shader, new Color(0.34f, 0.06f, 0.05f));
            vehicleMat = NewMat(shader, new Color(0.17f, 0.24f, 0.10f));
        }

        Material NewMat(Shader s, Color c) { var m = new Material(s); m.color = c; return m; }

        void CreateArena()
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "ArenaFloor";
            floor.transform.position = new Vector3(0f, -0.55f, 0f);
            floor.transform.localScale = new Vector3(82f, 1f, 82f);
            floor.GetComponent<Renderer>().material = floorMat;

            for (int i = 0; i < GameConfig.CoverCount; i++)
            {
                float a = Random.Range(0f, Mathf.PI * 2f);
                float r = Random.Range(8f, GameConfig.ArenaRadius);
                var b = GameObject.CreatePrimitive(PrimitiveType.Cube);
                b.name = "Cover_" + i;
                b.transform.position = new Vector3(Mathf.Cos(a) * r, Random.Range(0.8f, 2.6f), Mathf.Sin(a) * r);
                b.transform.localScale = new Vector3(Random.Range(1.5f, 4.8f), b.transform.position.y * 2f, Random.Range(1.5f, 4.8f));
                b.GetComponent<Renderer>().material = coverMat;
            }

            var lightGo = new GameObject("Sun");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.15f;
            lightGo.transform.rotation = Quaternion.Euler(45f, -35f, 0f);
            RenderSettings.ambientLight = new Color(0.36f, 0.38f, 0.42f);
        }

        PlayerMotor CreatePlayer()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = "Player";
            go.transform.position = new Vector3(0f, 1f, -6f);
            Destroy(go.GetComponent<CapsuleCollider>());
            go.GetComponent<Renderer>().material = playerMat;
            var cc = go.AddComponent<CharacterController>();
            cc.height = 2f; cc.radius = 0.45f;
            var hp = go.AddComponent<Health>(); hp.Configure(Faction.Player, 150f);
            var motor = go.AddComponent<PlayerMotor>(); motor.input = input; motor.SpawnPoint = go.transform.position;
            var combat = go.AddComponent<PlayerCombat>(); combat.input = input;
            mission.player = motor; mission.playerHealth = hp;
            return motor;
        }

        void CreateEnemies(Transform target)
        {
            for (int i = 0; i < GameConfig.EnemyCount; i++)
            {
                float a = i / (float)GameConfig.EnemyCount * Mathf.PI * 2f + Random.Range(-0.2f, 0.2f);
                float r = Random.Range(17f, 32f);
                var go = GameObject.CreatePrimitive(i % 4 == 0 ? PrimitiveType.Cylinder : PrimitiveType.Capsule);
                go.name = "Hostile_" + i;
                go.transform.position = new Vector3(Mathf.Cos(a) * r, 1f, Mathf.Sin(a) * r);
                var collider = go.GetComponent<Collider>(); if (collider != null) Destroy(collider);
                go.GetComponent<Renderer>().material = enemyMat;
                var hp = go.AddComponent<Health>(); hp.Configure(Faction.Hostile, i % 4 == 0 ? 160f : 90f);
                var ai = go.AddComponent<EnemyAgent>(); ai.target = target; ai.speed += Random.Range(-0.4f, 0.5f);
                mission.RegisterHostile(hp);
            }
        }

        void CreateBoss(Transform target)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = "COMMAND_MECH_BOSS";
            go.transform.position = new Vector3(0f, 2.2f, 34f);
            go.transform.localScale = new Vector3(2.4f, 2.2f, 2.4f);
            var collider = go.GetComponent<Collider>(); if (collider != null) Destroy(collider);
            go.GetComponent<Renderer>().material = bossMat;
            var hp = go.AddComponent<Health>(); hp.Configure(Faction.Hostile, 900f);
            var boss = go.AddComponent<BossController>(); boss.SetTarget(target);
            mission.RegisterHostile(hp);
        }

        void CreateVehicle(PlayerMotor player)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "HUMMER_360";
            go.transform.position = new Vector3(-7f, 0.9f, -2f);
            go.transform.localScale = new Vector3(2.6f, 1.2f, 4.4f);
            go.GetComponent<Renderer>().material = vehicleMat;
            var vehicle = go.AddComponent<HummerController>();
            vehicle.input = input; vehicle.player = player; vehicle.combat = player.GetComponent<PlayerCombat>(); vehicle.orbit = orbit;
        }
    }
}
