using UnityEngine;

[System.Serializable]
public class StageInstance
{
    public int stageKey;
    public int curWave;
    public StageInfo curStageInfo;

    public StageInstance(int stageKey, int curWave)
    {
        this.stageKey = stageKey;
        this.curWave = curWave;
    }

    public void SetStageInfo(StageInfo stageInfo)
    {
        this.curStageInfo = stageInfo;
    }

    public bool CheckEndofWave()
    {
        if (curStageInfo == null) return false;
        if (curWave >= curStageInfo.waves.Length - 1) return false;

        return true;
    }
}

public class StageSaveManager
{
    private const string saveKey = "StageInstance";

    public static void SaveStageInstance(StageInstance instance)
    {
        string json = JsonUtility.ToJson(instance);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    public static StageInstance LoadStageInstance()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            string json = PlayerPrefs.GetString(saveKey);
            return JsonUtility.FromJson<StageInstance>(json);
        }

        return null;
    }

    public static void ClearSavedStage()
    {
        PlayerPrefs.DeleteKey(saveKey);
        PlayerPrefs.Save();
    }
}
