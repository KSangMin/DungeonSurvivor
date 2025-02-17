using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    public override void Init()
    {
        base.Init();

        _startButton.onClick.AddListener(OnClickStartButton);
        _exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        
#endif
        Application.Quit();
    }

    protected override UIState GetUIState()
    {
        return UIState.Home;
    }
}
