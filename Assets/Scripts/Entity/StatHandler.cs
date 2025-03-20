using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    public StatData statData;
    private Dictionary<StatType, float> curStats = new Dictionary<StatType, float>();

    private void Awake()
    {
        InitStats();
    }

    void InitStats()
    {
        foreach(StatEntry entry in statData.stats)
        {
            curStats[entry.statType] = entry.baseValue;
        }
    }

    public float GetStat(StatType type)
    {
        return curStats.ContainsKey(type) ? curStats[type] : 0f;
    }

    public void ModifyStat(StatType type, float amount, bool isPermanent = true, float durtaion = 0)
    {
        if(!curStats.ContainsKey(type)) return;

        curStats[type] += amount;

        if (!isPermanent)
        {
            StartCoroutine(RemoveStatAfterDuration(type, amount, durtaion));
        }
    }

    IEnumerator RemoveStatAfterDuration(StatType type, float amount, float duration)
    {
        yield return new WaitForSeconds(duration);
        curStats[type] -= amount;
    }
}


