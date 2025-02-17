using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Home,
    Game,
    GameOver
}

public class UIManager : Singleton<UIManager>
{
    HomeUI homeUI;
    GameUI gameUI;
    GameOverUI gameOverUI;

    private UIState curState;

    private void Awake()
    {
        base.Awake();

        homeUI = GetComponentInChildren<HomeUI>(true);
        homeUI.Init();
        gameUI = GetComponentInChildren<GameUI>(true);
        gameUI.Init();
        gameOverUI = GetComponentInChildren<GameOverUI>(true);
        gameOverUI.Init();

        ChangeState(UIState.Home);
    }

    public void SetPlayGame()
    {
        ChangeState(UIState.Game);
    }

    public void SetGameOver()
    {
        ChangeState(UIState.GameOver);
    }

    public void ChangeWave(int waveId)
    {
        gameUI.UpdateWaveText(waveId);
    }

    public void ChangePlayerHP(float curHP, float maxHP)
    {
        gameUI.UpdateHPSlider(curHP / maxHP);
    }

    public void ChangeState(UIState state)
    {
        curState = state;
        homeUI.SetActive(curState);
        gameUI.SetActive(curState);
        gameOverUI.SetActive(curState);
    }
}
