using UnityEngine;

namespace EX360.Combat
{
    public static class Explosion
    {
        public static void Create(Vector3 position, float radius, float damage, Faction owner)
        {
            foreach (Collider c in Physics.OverlapSphere(position, radius))
            {
                var hp = c.GetComponentInParent<Health>();
                if (hp == null || hp.faction == owner) continue;
                float dist = Vector3.Distance(position, hp.transform.position);
                hp.Damage(damage * Mathf.Clamp01(1f - dist / radius));
            }
            var fx = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            fx.name = "ExplosionFX";
            fx.transform.position = position;
            fx.transform.localScale = Vector3.one * radius * 0.65f;
            Object.Destroy(fx.GetComponent<Collider>());
            var r = fx.GetComponent<Renderer>();
            r.material.color = new Color(1f, 0.35f, 0.05f, 1f);
            Object.Destroy(fx, 0.12f);
        }
    }
}
