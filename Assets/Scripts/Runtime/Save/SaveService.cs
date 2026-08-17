using UnityEngine;

namespace EX360.Save
{
    [System.Serializable]
    public sealed class SaveData
    {
        public int bestScore;
        public int missionsCompleted;
        public int graphicsTier = 2;
        public long updatedUtc;
    }

    public static class SaveService
    {
        const string Key = "EX360_SAVE_V1";

        public static SaveData Load()
        {
            string json = PlayerPrefs.GetString(Key, string.Empty);
            if (string.IsNullOrEmpty(json)) return new SaveData();
            try { return JsonUtility.FromJson<SaveData>(json) ?? new SaveData(); }
            catch { return new SaveData(); }
        }

        public static void Store(SaveData data)
        {
            data.updatedUtc = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }
    }
}
