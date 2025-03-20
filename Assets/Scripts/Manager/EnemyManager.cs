using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private Coroutine _waveRoutine;

    [SerializeField] private List<GameObject> _enemyPrefabs;
    private Dictionary<string , GameObject> _enemyPrefabDict;

    [SerializeField] List<Rect> spawnAreas;
    [SerializeField] private Color _gizmoColor = new Color(1, 0, 0, 0.3f);
    private List<EnemyController> _activeEnemies = new List<EnemyController>();

    private bool _enemySpawnComplete;

    [SerializeField] private float _timeBetweenSpawns = 0.2f;
    [SerializeField] private float _timeBetweenWaves = 1f;

    [SerializeField] private List<GameObject> _itemPrefabs;

    public override void Awake()
    {
        base.Awake();

        _enemyPrefabDict = new Dictionary<string , GameObject>();
        foreach(GameObject prefab in _enemyPrefabs)
        {
            _enemyPrefabDict[prefab.name] = prefab;
        }
    }

    public void StartWave(int waveCount)
    {
        if(waveCount <= 0)
        {
            GameManager.Instance.EndWave();
            return;
        }

        if(_waveRoutine != null)
        {
            StopCoroutine(_waveRoutine);
        }
        _waveRoutine = StartCoroutine(SpawnWave(waveCount));
    }

    public void StopWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        _enemySpawnComplete = false;
        yield return new WaitForSeconds(_timeBetweenWaves);

        for(int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(_timeBetweenSpawns);
            SpawnRandomEnemy();
        }

        _enemySpawnComplete = true;
    }

    private void SpawnRandomEnemy(string prefabName = null)
    {
        if (_enemyPrefabs.Count == 0 || spawnAreas.Count == 0) return;

        GameObject randomPrefab;
        if(prefabName == null)
        {
            randomPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
        }
        else
        {
            randomPrefab = _enemyPrefabDict[prefabName];
        }

        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        Vector2 randomPos = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax));

        GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPos.x, randomPos.y), Quaternion.identity);
        EnemyController enemyController = spawnEnemy.GetComponent<EnemyController>();
        enemyController.Init(GameManager.Instance.player.transform);

        _activeEnemies.Add(enemyController);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        Gizmos.color = _gizmoColor;
        foreach(var area in spawnAreas)
        {
            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, area.height);

            Gizmos.DrawCube(center, size);
        }
    }

    public void RemoveEnemyOnDeath(EnemyController enemy)
    {
        _activeEnemies.Remove(enemy);
        CreateRandomItem(enemy.transform.position);
        if (_enemySpawnComplete && _activeEnemies.Count <= 0)
        {
            GameManager.Instance.EndWave();
        }
    }

    public void CreateRandomItem(Vector3 pos)
    {
        GameObject item = Instantiate(_itemPrefabs[Random.Range(0, _itemPrefabs.Count)], pos, Quaternion.identity);
    }

    public void StartStage(StageInstance stage)
    {
        if(_waveRoutine != null) StopCoroutine(_waveRoutine);

        _waveRoutine = StartCoroutine(SpawnStart(stage));
    }

    IEnumerator SpawnStart(StageInstance stage)
    {
        _enemySpawnComplete = false;

        yield return new WaitForSeconds(_timeBetweenWaves);

        WaveData wave = stage.curStageInfo.waves[stage.curWave];

        for(int i = 0; i < wave.monsters.Length; i++)
        {
            yield return new WaitForSeconds(_timeBetweenWaves);

            MonsterSpawnData monster = wave.monsters[i];
            for(int j = 0; j < monster.spawnCount; j++)
            {
                SpawnRandomEnemy(monster.monsterType);
            }
        }

        if (wave.hasBoss)
        {
            yield return new WaitForSeconds(_timeBetweenWaves);

            GameManager.Instance.MainCameraShake();
            SpawnRandomEnemy(wave.bossType);
        }

        _enemySpawnComplete = true;
    }
}
