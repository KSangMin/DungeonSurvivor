using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCam;
    private CinemachineBasicMultiChannelPerlin _perlin;
    private float _shakeTimeRemaining;

    private void Awake()
    {
        _virtualCam = GetComponent<CinemachineVirtualCamera>();
        _perlin = _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if(_shakeTimeRemaining > 0)
        {
            _shakeTimeRemaining -= Time.deltaTime;
            if (_shakeTimeRemaining <= 0)
            {
                StopShake();
            }
        }
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        if (_shakeTimeRemaining > duration) return;

        _shakeTimeRemaining = duration;

        _perlin.m_AmplitudeGain = amplitude;
        _perlin.m_FrequencyGain = frequency;
    }

    public void StopShake()
    {
        _shakeTimeRemaining = 0;
        _perlin.m_FrequencyGain = 0;
        _perlin.m_AmplitudeGain = 0;
    }
}
