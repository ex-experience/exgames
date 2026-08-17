using UnityEngine;

namespace EX360.Combat
{
    public sealed class Projectile : MonoBehaviour
    {
        public Vector3 velocity;
        public float damage = 22f;
        public float life = 3f;
        public Faction ownerFaction = Faction.Player;
        public float radius = 0.12f;

        void Update()
        {
            float dt = Time.deltaTime;
            Vector3 delta = velocity * dt;
            if (Physics.SphereCast(transform.position, radius, velocity.normalized, out RaycastHit hit, delta.magnitude, ~0, QueryTriggerInteraction.Ignore))
            {
                var hp = hit.collider.GetComponentInParent<Health>();
                if (hp != null && hp.faction != ownerFaction) hp.Damage(damage);
                Destroy(gameObject);
                return;
            }
            transform.position += delta;
            life -= dt;
            if (life <= 0f) Destroy(gameObject);
        }
    }
}
