using UnityEngine;
using EX360.Core;

namespace EX360.Performance
{
    public sealed class AdaptiveQuality : MonoBehaviour
    {
        float sampleTime;
        int frames;
        float lastFps = 60f;
        int tier = 2;
        public float LastFps => lastFps;
        public int Tier => tier;

        void Start()
        {
            Application.targetFrameRate = GameConfig.TargetFps;
            QualitySettings.vSyncCount = 0;
        }

        void Update()
        {
            sampleTime += Time.unscaledDeltaTime;
            frames++;
            if (sampleTime < 2f) return;
            lastFps = frames / sampleTime;
            frames = 0;
            sampleTime = 0f;

            if (lastFps < 42f && tier > 0) ApplyTier(tier - 1);
            else if (lastFps > 57f && tier < 2) ApplyTier(tier + 1);
        }

        void ApplyTier(int newTier)
        {
            tier = Mathf.Clamp(newTier, 0, 2);
            QualitySettings.shadowDistance = tier == 0 ? 0f : tier == 1 ? 18f : 32f;
            QualitySettings.lodBias = tier == 0 ? 0.65f : tier == 1 ? 0.9f : 1.1f;
        }
    }
}
