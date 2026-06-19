using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
  [Header("Audio Sources")]
  [SerializeField] private AudioSource _musicSource;
  [SerializeField] private AudioSource _sfxSource;

  [Header("Audio Data")]
  [SerializeField] private SoundSFXData[] _audioSFXClips;

  [SerializeField] private SoundMusicData[] _audioMusicClips;

  [Header("Volume Settings")]
  [SerializeField] private float _musicVolume = 1f;
  [SerializeField] private float _sfxVolume = 1f;

  private Dictionary<AudioSFXEnum, SoundSFXData> _audioSFXDictionary;
  private Dictionary<AudioMusicEnum, SoundMusicData> _audioMusicDictionary;

  private void Start()
  {
    _audioSFXDictionary = new();
    _audioMusicDictionary = new();

    foreach (var soundData in _audioSFXClips)
    {
      if (soundData.clip is null)
      {
        continue;
      }

      _audioSFXDictionary[soundData.key] = soundData;
    }

    foreach (var soundData in _audioMusicClips)
    {
      if (soundData.clip is null)
      {
        continue;
      }

      _audioMusicDictionary[soundData.key] = soundData;
    }

    SetMusicVolume(_musicVolume);
    SetSFXVolume(_sfxVolume);
  }

  public void PlayMusic(AudioMusicEnum key)
  {
    if (!_audioMusicDictionary.ContainsKey(key))
    {
      Debug.LogWarning($"Music key '{key}' not found in AudioManager");
      return;
    }

    var soundData = _audioMusicDictionary[key];
    if (_musicSource.isPlaying)
    {
      _musicSource.Stop();
    }

    _musicSource.clip = soundData.clip;
    _musicSource.Play();
  }

  public void PlaySFX(AudioSFXEnum key)
  {
    if (!_audioSFXDictionary.ContainsKey(key))
    {
      Debug.LogWarning($"SFX key '{key}' not found in AudioManager");
      return;
    }

    var soundData = _audioSFXDictionary[key];
    _sfxSource.PlayOneShot(soundData.clip, _sfxVolume);
  }

  public void PlaySFX(AudioSFXEnum key, float customVolume)
  {
    if (!_audioSFXDictionary.ContainsKey(key))
    {
      Debug.LogWarning($"SFX key '{key}' not found in AudioManager");
      return;
    }

    var soundData = _audioSFXDictionary[key];
    _sfxSource.PlayOneShot(soundData.clip, customVolume);
  }

  public void PlaySFX(AudioClip clip, float customVolume)
  {
    if (clip is null)
    {
      Debug.LogWarning("AudioClip is null in PlaySFX");
      return;
    }

    _sfxSource.PlayOneShot(clip, customVolume);
  }

  public void StopMusic()
  {
    if (_musicSource != null && _musicSource.isPlaying)
    {
      _musicSource.Stop();
    }
  }

  public void StopSFX()
  {
    if (_sfxSource != null && _sfxSource.isPlaying)
    {
      _sfxSource.Stop();
    }
  }

  public void PauseMusic()
  {
    if (_musicSource != null && _musicSource.isPlaying)
    {
      _musicSource.Pause();
    }
  }

  public void ResumeMusic()
  {
    if (_musicSource != null && !_musicSource.isPlaying)
    {
      _musicSource.UnPause();
    }
  }

  public void SetMusicVolume(float volume) => _musicSource.volume = Mathf.Clamp01(volume);

  public void SetSFXVolume(float volume) => _sfxSource.volume = Mathf.Clamp01(volume);

  public float GetMusicVolume() => _musicSource.volume;

  public float GetSFXVolume() => _sfxSource.volume;

  public void ToggleMusic() => _musicSource.mute = !_musicSource.mute;

  public void ToggleSFX() => _sfxSource.mute = !_sfxSource.mute;
}