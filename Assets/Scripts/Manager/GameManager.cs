using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player {  get; private set; }
    private ResourceController _playerResourceController;

    [SerializeField] private int curStageId = 0;
    [SerializeField] private int curWaveId = 0;

    public static bool isFirstLoading = true;

    private CameraShake _cameraShake;

    public override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<PlayerController>();
        player.Init();

        _playerResourceController = player.GetComponent<ResourceController>();
        _playerResourceController.RemoveHealthChangeEvent(UIManager.Instance.ChangePlayerHP);
        _playerResourceController.AddHealthChangeEvent(UIManager.Instance.ChangePlayerHP);

        _cameraShake = FindObjectOfType<CameraShake>();
        MainCameraShake();
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
        //StartNextWave();
        StartStage();
    }

    void StartNextWave()
    {
        curWaveId++;
        EnemyManager.Instance.StartWave(1 + curWaveId / 5);
        UIManager.Instance.ChangeWave(curWaveId);
    }

    public void EndWave()
    {
        //StartNextWave();
        StartNextWaveInStage();
    }

    public void GameOver()
    {
        EnemyManager.Instance.StopWave();
        UIManager.Instance.SetGameOver();
    }

    public void MainCameraShake()
    {
        _cameraShake.ShakeCamera(1, 1, 1);
    }

    public void StartStage()
    {
        StageInfo stage = GetStageInfo(curStageId);

        if(stage == null)
        {
            Debug.Log("스테이지 정보가 없습니다.");
            return;
        }

        UIManager.Instance.ChangeWave(curStageId + 1);
        EnemyManager.Instance.StartStage(stage.waves[curWaveId]);
    }

    public void StartNextWaveInStage()
    {
        StageInfo stage = GetStageInfo(curStageId);

        if(stage.waves.Length - 1> curWaveId)
        {
            curWaveId++;
            StartStage();
        }
        else
        {
            CompleteStage();
        }
    }

    public void CompleteStage()
    {
        curStageId++;
        curWaveId = 0;
        StartStage();
    }

    StageInfo GetStageInfo(int stageKey)
    {
        foreach(var stage in StageData.Stages)
        {
            if (stage.stageKey == stageKey) return stage;
        }

        return null;
    }
}
