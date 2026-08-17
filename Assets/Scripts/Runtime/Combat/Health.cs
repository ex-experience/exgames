using System;
using UnityEngine;

namespace EX360.Combat
{
    public enum Faction { Player, Friendly, Hostile }

    public sealed class Health : MonoBehaviour
    {
        public static event Action<Health> AnyDied;
        public Faction faction = Faction.Hostile;
        public float maxHealth = 100f;
        public float Current { get; private set; }
        public bool IsAlive => Current > 0f;
        public float Normalized => maxHealth <= 0f ? 0f : Current / maxHealth;

        void Awake() => Current = maxHealth;

        public void Configure(Faction newFaction, float hp)
        {
            faction = newFaction;
            maxHealth = Mathf.Max(1f, hp);
            Current = maxHealth;
        }

        public void Damage(float amount)
        {
            if (!IsAlive || amount <= 0f) return;
            Current = Mathf.Max(0f, Current - amount);
            if (Current <= 0f) Die();
        }

        public void Heal(float amount)
        {
            if (!IsAlive) return;
            Current = Mathf.Min(maxHealth, Current + Mathf.Max(0f, amount));
        }

        public void Revive(float fraction = 1f)
        {
            Current = maxHealth * Mathf.Clamp01(fraction);
            gameObject.SetActive(true);
        }

        void Die()
        {
            AnyDied?.Invoke(this);
            if (faction != Faction.Player) Destroy(gameObject, 0.05f);
        }
    }
}
