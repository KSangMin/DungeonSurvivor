using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player {  get; private set; }
    private ResourceController _playerResourceController;

    [SerializeField] private int curWaveId = 0;

    private void Awake()
    {
        base.Awake();

        player = FindObjectOfType<PlayerController>();
        player.Init();
    }

    public void StartGame()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        curWaveId++;
        EnemyManager.Instance.StartWave(1 + curWaveId / 5);
    }

    public void EndWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        EnemyManager.Instance.StopWave();
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.Space))
       {
            StartGame();
       }
    }
}
