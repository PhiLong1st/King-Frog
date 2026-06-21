using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Collections;

public enum SceneName
{
  MainMenuScene,
}

[Serializable]
public class SceneData
{
  public SceneName SceneName;
  public BaseScene Scene;
}

public class SceneLoader : MonoBehaviour
{
  [SerializeField] private SceneData[] _scenes;
  [SerializeField] private BaseScene _loadingScene;
  private Dictionary<SceneName, BaseScene> _sceneDictionary;

  private BaseScene _currentScene;

  private void Awake()
  {
    _sceneDictionary = new();

    foreach (var sceneData in _scenes)
    {
      if (_sceneDictionary.ContainsKey(sceneData.SceneName))
      {
        Debug.LogWarning($"Duplicate scene name detected: {sceneData.SceneName}. Skipping this entry.");
        continue;
      }

      _sceneDictionary.Add(sceneData.SceneName, sceneData.Scene);
    }
  }

  private void Start()
  {
    Debug.Log("SceneLoader: Start called. Loading MainMenuScene.");
    StartCoroutine(SwitchScene(SceneName.MainMenuScene));
  }

  public IEnumerator SwitchScene(SceneName sceneName)
  {
    var isSceneExists = _sceneDictionary.TryGetValue(sceneName, out var newScene);
    if (!isSceneExists)
    {
      Debug.LogError($"Scene '{sceneName}' not found in the scene dictionary.");
      yield return null;
    }

    if (_currentScene is not null) StartCoroutine(_currentScene?.AnimateUnload());
    StartCoroutine(_loadingScene.AnimateLoad());

    _currentScene = newScene;
    yield return ApiClient.Instance.GetUserByIdAsync("1", OnLoadDataSuccess, OnLoadDataFailure);
  }

  private void OnLoadDataSuccess(UserData userData)
  {
    Debug.Log($"User ID: {userData.Id}, Name: {userData.Name}, Avatar URL: {userData.AvatarUrl}");
    StartCoroutine(_loadingScene.AnimateUnload());
    StartCoroutine(_currentScene.AnimateLoad());
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