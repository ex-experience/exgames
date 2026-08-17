using UnityEngine;
using EX360.Input;
using EX360.Combat;

namespace EX360.Player
{
    public sealed class PlayerCombat : MonoBehaviour
    {
        public CrossPlatformInput input;
        public Camera gameplayCamera;
        public float fireRate = 9f;
        public float projectileSpeed = 34f;
        public float bulletDamage = 18f;
        float nextFire;
        float grenadeCooldown;

        void Update()
        {
            if (input == null || gameplayCamera == null) return;
            Vector3 aim = input.GetAimDirection(gameplayCamera, transform.position + Vector3.up * 0.9f);
            if (aim.sqrMagnitude > 0.01f)
            {
                Quaternion look = Quaternion.LookRotation(aim, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, look, 18f * Time.deltaTime);
            }

            if (input.FireHeld && Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                Fire(aim);
            }
            grenadeCooldown -= Time.deltaTime;
            if (input.GrenadePressed && grenadeCooldown <= 0f)
            {
                grenadeCooldown = 2.2f;
                ThrowGrenade(aim);
            }
        }

        void Fire(Vector3 direction)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "PlayerProjectile";
            go.transform.localScale = Vector3.one * 0.16f;
            go.transform.position = transform.position + Vector3.up * 1.0f + direction * 0.8f;
            Destroy(go.GetComponent<Collider>());
            var p = go.AddComponent<Projectile>();
            p.ownerFaction = Faction.Player;
            p.damage = bulletDamage;
            p.velocity = direction.normalized * projectileSpeed;
            go.GetComponent<Renderer>().material.color = new Color(1f, 0.8f, 0.2f);
        }

        void ThrowGrenade(Vector3 direction)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Grenade";
            go.transform.localScale = Vector3.one * 0.32f;
            go.transform.position = transform.position + Vector3.up * 1.1f + direction * 0.8f;
            Destroy(go.GetComponent<Collider>());
            var grenade = go.AddComponent<GrenadeProjectile>();
            grenade.velocity = direction.normalized * 14f + Vector3.up * 7f;
        }
    }

    public sealed class GrenadeProjectile : MonoBehaviour
    {
        public Vector3 velocity;
        float fuse = 1.15f;
        void Update()
        {
            velocity += Physics.gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
            fuse -= Time.deltaTime;
            if (fuse <= 0f)
            {
                Explosion.Create(transform.position, 5f, 80f, Faction.Player);
                Destroy(gameObject);
            }
        }
    }
}
