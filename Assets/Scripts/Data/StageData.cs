using UnityEngine;

enum MonsterName
{
    Goblin_Knife,
    Goblin_Bow,
    Orc_Shaman
}

[System.Serializable]
public class StageInfo
{
    public int stageKey;
    public WaveData[] waves;

    public StageInfo(int stageKey, WaveData[] waves)
    {
        this.stageKey = stageKey;
        this.waves = waves;
    }
}

[System.Serializable]
public class WaveData
{
    public MonsterSpawnData[] monsters;
    public bool hasBoss;
    public string bossType;

    public WaveData(MonsterSpawnData[] monsters, bool hasBoss = false, string bossType = "")
    {
        this.monsters = monsters;
        this.hasBoss = hasBoss;
        this.bossType = bossType;
    }
}

[System.Serializable]
public class MonsterSpawnData
{
    public string monsterType;
    public int spawnCount;

    public MonsterSpawnData(string monsterType, int spawnCount)
    {
        this.monsterType = monsterType;
        this.spawnCount = spawnCount;
    }
}

public static class StageData
{
    public static readonly StageInfo[] Stages = new StageInfo[]
    {
        new StageInfo(0, new WaveData[]
        {
            new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData(MonsterName.Goblin_Knife.ToString(), 1)
            })

            , new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData(MonsterName.Goblin_Knife.ToString(), 2)
            })

            , new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData(MonsterName.Goblin_Knife.ToString(), 1)
                , new MonsterSpawnData(MonsterName.Goblin_Knife.ToString(), 1)
                , new MonsterSpawnData(MonsterName.Goblin_Bow.ToString(), 2)
            }, true, MonsterName.Orc_Shaman.ToString())
        })

        , new StageInfo(1, new WaveData[]
        {
            new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData(MonsterName.Goblin_Knife.ToString(), 5)
            })

            , new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData(MonsterName.Goblin_Bow.ToString(), 7)
            })

            , new WaveData(new MonsterSpawnData[]
            {
                new MonsterSpawnData(MonsterName.Goblin_Knife.ToString(), 3)
                , new MonsterSpawnData(MonsterName.Goblin_Knife.ToString(), 3)
                , new MonsterSpawnData(MonsterName.Goblin_Bow.ToString(), 3)
            }, true, MonsterName.Orc_Shaman.ToString())
        })
    };
}
