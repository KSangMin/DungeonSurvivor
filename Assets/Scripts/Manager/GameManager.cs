using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player {  get; private set; }
    private ResourceController _playerResourceController;

    [SerializeField] private int curWaveId = 0;

    public static bool isFirstLoading = true;

    private void Awake()
    {
        base.Awake();

        player = FindObjectOfType<PlayerController>();
        player.Init();

        _playerResourceController = player.GetComponent<ResourceController>();
        _playerResourceController.RemoveHealthChangeEvent(UIManager.Instance.ChangePlayerHP);
        _playerResourceController.AddHealthChangeEvent(UIManager.Instance.ChangePlayerHP);
    }
    private void Start()
    {
        if (!isFirstLoading)
        {
            StartGame();
        }
        else
        {
            isFirstLoading = false;
        }
    }

    public void StartGame()
    {
        UIManager.Instance.SetPlayGame();
        StartNextWave();
    }

    void StartNextWave()
    {
        curWaveId++;
        EnemyManager.Instance.StartWave(1 + curWaveId / 5);
        UIManager.Instance.ChangeWave(curWaveId);
    }

    public void EndWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        EnemyManager.Instance.StopWave();
        UIManager.Instance.SetGameOver();
    }
}
