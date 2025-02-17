using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] private float musicVolume;

    private AudioSource _musicAudioSource;
    public AudioClip musicClip;

    public SoundSource soundSourcePrefab;

    private void Awake()
    {
        base.Awake();
        _musicAudioSource = GetComponent<AudioSource>();
        _musicAudioSource.volume = musicVolume;
        _musicAudioSource.loop = true;
    }

    private void Start()
    {
        ChangeBackgroundMusic(musicClip);   
    }

    public void ChangeBackgroundMusic(AudioClip clip)
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = clip;
        _musicAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        SoundSource obj = Instantiate(Instance.soundSourcePrefab);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPitchVariance);
    }
}
