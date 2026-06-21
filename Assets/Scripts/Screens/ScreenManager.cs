using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public enum ScreenName
{
  MainMenuScreen,
}

[Serializable]
public class ScreenData
{
  public ScreenName ScreenName;
  public BaseScreen Screen;
}

public class ScreenManager : MonoBehaviour
{
  [SerializeField] private ScreenData[] _scenes;
  [SerializeField] private BaseScreen _loadingScreen;
  private Dictionary<ScreenName, BaseScreen> _screenDictionary;

  private BaseScreen _currentScreen;

  private void Awake()
  {
    _screenDictionary = new();

    foreach (var screenData in _scenes)
    {
      if (_screenDictionary.ContainsKey(screenData.ScreenName))
      {
        Debug.LogWarning($"Duplicate screen name detected: {screenData.ScreenName}. Skipping this entry.");
        continue;
      }

      _screenDictionary.Add(screenData.ScreenName, screenData.Screen);
    }
  }

  private void Start()
  {
    // Debug.Log("ScreenLoader: Start called. Loading MainMenuScreen.");
    // StartCoroutine(SwitchScreen(ScreenName.MainMenuScreen));
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      Debug.Log("Space key pressed. Switching to MainMenuScreen.");
      StartCoroutine(SwitchScreen(ScreenName.MainMenuScreen));
    }
  }

  public IEnumerator SwitchScreen(ScreenName screenName)
  {
    if (!_screenDictionary.TryGetValue(screenName, out var newScreen))
    {
      Debug.LogError($"Screen '{screenName}' not found in the screen dictionary.");
      yield break;
    }

    Sequence inTransition = DOTween.Sequence();
    if (_currentScreen != null) inTransition.Join(_currentScreen.AnimateUnload());
    inTransition.Join(_loadingScreen.AnimateLoad());
    yield return inTransition.WaitForCompletion();

    yield return ApiClient.Instance.GetUserByIdAsync("1", OnLoadDataSuccess, OnLoadDataFailure);

    _currentScreen = newScreen;

    Sequence outTransition = DOTween.Sequence();
    outTransition.Join(_loadingScreen.AnimateUnload());
    outTransition.Join(_currentScreen.AnimateLoad());

    yield return outTransition.WaitForCompletion();
  }

  private void OnLoadDataSuccess(UserData userData)
  {
    Debug.Log($"User ID: {userData.Id}, Name: {userData.Name}, Avatar URL: {userData.AvatarUrl}");
  }

  private void OnLoadDataFailure(string error)
  {
    Debug.LogError($"Failed to load data: {error}");
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