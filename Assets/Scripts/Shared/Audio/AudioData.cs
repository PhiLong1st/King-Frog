using System;
using UnityEngine;

public enum AudioSFXEnum
{
  LoadDone,
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