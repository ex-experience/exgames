using UnityEngine;
using EX360.Combat;
using EX360.Mission;
using EX360.Performance;

namespace EX360.UI
{
    public sealed class HudOverlay : MonoBehaviour
    {
        public Health playerHealth;
        public MissionDirector mission;
        public AdaptiveQuality quality;
        GUIStyle title;
        GUIStyle small;

        void EnsureStyles()
        {
            if (title != null) return;
            title = new GUIStyle(GUI.skin.label) { fontSize = Mathf.Max(16, Screen.height / 28), fontStyle = FontStyle.Bold };
            title.normal.textColor = Color.white;
            small = new GUIStyle(GUI.skin.label) { fontSize = Mathf.Max(12, Screen.height / 42) };
            small.normal.textColor = new Color(0.9f, 0.95f, 1f);
        }

        void OnGUI()
        {
            EnsureStyles();
            float pad = Mathf.Max(14f, Screen.width * 0.018f);
            GUI.Label(new Rect(pad, pad, 600, 50), "ARABIA STRIKE 360 // PRODUCTION CORE", title);
            string hp = playerHealth == null ? "--" : Mathf.CeilToInt(playerHealth.Current).ToString();
            string left = mission == null ? "--" : mission.HostilesRemaining.ToString();
            string score = mission == null ? "0" : mission.Score.ToString("D7");
            string fps = quality == null ? "--" : quality.LastFps.ToString("0");
            GUI.Label(new Rect(pad, pad + 48, 700, 80), $"HP {hp}   HOSTILES {left}   SCORE {score}   FPS {fps}", small);
            if (mission != null && mission.Complete)
                GUI.Label(new Rect(Screen.width * 0.32f, Screen.height * 0.42f, Screen.width * 0.5f, 70), "MISSION COMPLETE", title);

            if (UnityEngine.Input.touchSupported)
            {
                GUI.Label(new Rect(pad, Screen.height - 45, Screen.width * 0.45f, 35), "LEFT: MOVE   RIGHT: AIM/FIRE", small);
                GUI.Label(new Rect(Screen.width * 0.62f, Screen.height - 75, Screen.width * 0.36f, 65), "● FIRE   ↑ JUMP   ◆ GRENADE   USE", small);
            }
        }
    }
}
