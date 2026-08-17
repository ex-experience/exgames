using UnityEngine;
using EX360.Combat;

namespace EX360.AI
{
    [RequireComponent(typeof(Health))]
    public sealed class EnemyAgent : MonoBehaviour
    {
        public Transform target;
        public float speed = 3.4f;
        public float preferredRange = 12f;
        public float fireInterval = 1.1f;
        float nextFire;
        CharacterController controller;

        void Awake()
        {
            controller = gameObject.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = 0.42f;
        }

        void Update()
        {
            if (target == null) return;
            Vector3 delta = target.position - transform.position; delta.y = 0f;
            float dist = delta.magnitude;
            if (dist > 0.1f)
            {
                Vector3 dir = delta / dist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 8f * Time.deltaTime);
                if (dist > preferredRange) controller.SimpleMove(dir * speed);
                else if (dist < preferredRange * 0.55f) controller.SimpleMove(-dir * speed * 0.65f);
            }
            if (dist < preferredRange * 1.4f && Time.time >= nextFire)
            {
                nextFire = Time.time + fireInterval * Random.Range(0.8f, 1.2f);
                Shoot();
            }
        }

        void Shoot()
        {
            Vector3 origin = transform.position + Vector3.up * 1.0f;
            Vector3 dir = (target.position + Vector3.up * 0.8f - origin).normalized;
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "EnemyProjectile";
            go.transform.localScale = Vector3.one * 0.14f;
            go.transform.position = origin + dir * 0.8f;
            Destroy(go.GetComponent<Collider>());
            var p = go.AddComponent<Projectile>();
            p.ownerFaction = Faction.Hostile;
            p.damage = 9f;
            p.velocity = dir * 17f;
            go.GetComponent<Renderer>().material.color = new Color(1f, 0.15f, 0.08f);
        }
    }
}
