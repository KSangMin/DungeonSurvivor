using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private Coroutine _waveRoutine;

    [SerializeField] private List<GameObject> _enemyPrefabs;

    [SerializeField] List<Rect> spawnAreas;
    [SerializeField] private Color _gizmoColor = new Color(1, 0, 0, 0.3f);
    private List<EnemyController> _activeEnemies = new List<EnemyController>();

    private bool _enemySpawnComplete;

    [SerializeField] private float _timeBetweenSpawns = 0.2f;
    [SerializeField] private float _timeBetweenWaves = 1f;

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

    private void SpawnRandomEnemy()
    {
        if (_enemyPrefabs.Count == 0 || spawnAreas.Count == 0) return;

        Debug.Log("Àû »ý¼ºµÊ");

        GameObject randomPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];

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
        if(_enemySpawnComplete && _activeEnemies.Count <= 0)
        {
            GameManager.Instance.EndWave();
        }
    }
}
