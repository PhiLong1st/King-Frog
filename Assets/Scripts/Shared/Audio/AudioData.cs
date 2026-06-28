using System;
using UnityEngine;

public enum AudioSFXEnum
{
  ButtonClick,
  KeyCollected,
  GameWin,
  GameOver,
}

public enum AudioMusicEnum
{
  MainMenuMusic,
  GameplayMusic,
}

[Serializable]
public struct SoundSFXData
{
  [SerializeField]
  public AudioSFXEnum key;

  [SerializeField]
  public AudioClip clip;
}

[Serializable]
public struct SoundMusicData
{
  [SerializeField]
  public AudioMusicEnum key;

  [SerializeField]
  public AudioClip clip;
}