using UnityEngine;
using EX360.Combat;
using EX360.Player;

namespace EX360.Mission
{
    public sealed class MissionDirector : MonoBehaviour
    {
        public int HostilesRemaining { get; private set; }
        public int Score { get; private set; }
        public bool Complete { get; private set; }
        public PlayerMotor player;
        public Health playerHealth;
        float reviveAt = -1f;

        void OnEnable() => Health.AnyDied += OnAnyDied;
        void OnDisable() => Health.AnyDied -= OnAnyDied;

        public void RegisterHostile(Health hp)
        {
            if (hp != null && hp.faction == Faction.Hostile) HostilesRemaining++;
        }

        void OnAnyDied(Health hp)
        {
            if (hp.faction == Faction.Hostile)
            {
                HostilesRemaining = Mathf.Max(0, HostilesRemaining - 1);
                Score += hp.maxHealth >= 500f ? 5000 : 500;
                if (HostilesRemaining == 0) Complete = true;
            }
            else if (hp.faction == Faction.Player)
            {
                reviveAt = Time.time + 2f;
                if (player != null) player.enabled = false;
            }
        }

        void Update()
        {
            if (reviveAt > 0f && Time.time >= reviveAt)
            {
                reviveAt = -1f;
                if (playerHealth != null) playerHealth.Revive(1f);
                if (player != null) { player.Respawn(); player.enabled = true; }
            }
        }
    }
}
