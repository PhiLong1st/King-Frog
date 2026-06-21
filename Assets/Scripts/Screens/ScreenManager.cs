using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public enum ScreenName
{
  MainMenuScreen,
  GameplayScreen,
}

[Serializable]
public class ScreenData
{
  public ScreenName Name;
  public Screen Screen;
}

public class ScreenManager : MonoBehaviour
{
  [SerializeField] private ScreenData[] screenDatas;
  private Dictionary<ScreenName, Screen> _screenDictionary;

  private void Awake()
  {
    _screenDictionary = new();

    foreach (ScreenData data in screenDatas)
    {
      if (_screenDictionary.ContainsKey(data.Name))
      {
        Debug.LogWarning($"Duplicate screen name detected: {data.Name}. Skipping this entry.");
        continue;
      }

      Screen screen = data.Screen;
      screen.Initialize(this);
      _screenDictionary.Add(data.Name, screen);
    }
  }

  private void Start()
  {
    OpenScreen(ScreenName.MainMenuScreen);
  }

  public void OpenScreen(ScreenName screenName)
  {
    if (!_screenDictionary.TryGetValue(screenName, out var screen))
    {
      Debug.LogError($"Screen '{screenName}' not found in the screen dictionary.");
      return;
    }

    StartCoroutine(screen.OnLoad());
  }

  public void CloseScreen(ScreenName screenName)
  {
    if (!_screenDictionary.TryGetValue(screenName, out var screen))
    {
      Debug.LogError($"Screen '{screenName}' not found in the screen dictionary.");
      return;
    }

    StartCoroutine(screen.OnUnload());
  }

  public void Exit()
  {
#if UNITY_EDITOR
    EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
  }
}