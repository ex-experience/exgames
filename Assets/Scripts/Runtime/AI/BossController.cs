using UnityEngine;
using EX360.Combat;

namespace EX360.AI
{
    [RequireComponent(typeof(Health))]
    public sealed class BossController : MonoBehaviour
    {
        EnemyAgent agent;
        Health hp;
        int phase = 1;
        float pulse;

        void Awake()
        {
            hp = GetComponent<Health>();
            agent = gameObject.AddComponent<EnemyAgent>();
            agent.speed = 2.2f;
            agent.preferredRange = 15f;
            agent.fireInterval = 0.8f;
        }

        public void SetTarget(Transform t) => agent.target = t;

        void Update()
        {
            int next = hp.Normalized > 0.66f ? 1 : hp.Normalized > 0.33f ? 2 : 3;
            if (next != phase)
            {
                phase = next;
                agent.speed = 2.2f + phase * 0.65f;
                agent.fireInterval = Mathf.Max(0.28f, 0.85f - phase * 0.16f);
                transform.localScale *= 1.05f;
            }
            pulse += Time.deltaTime;
            if (phase == 3 && pulse > 3.6f)
            {
                pulse = 0f;
                Explosion.Create(transform.position + transform.forward * 2f, 4f, 28f, Faction.Hostile);
            }
        }
    }
}
