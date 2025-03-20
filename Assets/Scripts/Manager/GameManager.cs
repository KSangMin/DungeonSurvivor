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

    private StageInstance _curStageInstance;

    public override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<PlayerController>();
        player.Init();

        _playerResourceController = player.GetComponent<ResourceController>();
        _playerResourceController.RemoveHealthChangeEvent(UIManager.Instance.ChangePlayerHP);
        _playerResourceController.AddHealthChangeEvent(UIManager.Instance.ChangePlayerHP);

        _cameraShake = FindObjectOfType<CameraShake>();
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
        LoadOrStartNewStage();
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
        StageSaveManager.ClearSavedStage();
    }

    public void MainCameraShake()
    {
        _cameraShake.ShakeCamera(1, 1, 1);
    }

    private void LoadOrStartNewStage()
    {
        StageInstance savedInstace = StageSaveManager.LoadStageInstance();

        if (savedInstace != null) _curStageInstance = savedInstace;
        else _curStageInstance = new StageInstance(0, 0);

        StartStage(_curStageInstance);
    }

    public void StartStage(StageInstance stageInstance)
    {
        curStageId = stageInstance.stageKey;
        curWaveId = stageInstance.curWave;

        StageInfo stageInfo = GetStageInfo(curStageId);

        if(stageInfo == null)
        {
            Debug.Log("스테이지 정보가 없습니다.");
            StageSaveManager.ClearSavedStage();
            _curStageInstance = null;
            return;
        }

        stageInstance.SetStageInfo(stageInfo);

        UIManager.Instance.ChangeWave(curStageId + 1);
        EnemyManager.Instance.StartStage(_curStageInstance);
        StageSaveManager.SaveStageInstance(_curStageInstance);
    }

    public void StartNextWaveInStage()
    {
        Debug.Log("스테이지: " + curStageId);
        Debug.Log("웨이브: " + curWaveId);

        if(_curStageInstance.CheckEndofWave())
        {
            _curStageInstance.curWave++;
            StartStage(_curStageInstance);
        }
        else
        {
            CompleteStage();
        }
    }

    public void CompleteStage()
    {
        StageSaveManager.ClearSavedStage();

        if (_curStageInstance == null) return;

        _curStageInstance.stageKey++;
        _curStageInstance.curWave = 0;
        StartStage(_curStageInstance);
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
