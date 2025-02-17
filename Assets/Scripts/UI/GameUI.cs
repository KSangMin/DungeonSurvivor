using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private Slider _hpSlider;

    private void Start()
    {
        
    }

    public void UpdateHPSlider(float percentage)
    {
        _hpSlider.value = percentage;
    }

    public void UpdateWaveText(int wave)
    {
        _waveText.text = wave.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
}
