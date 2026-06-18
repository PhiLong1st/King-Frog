using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneLoader : MonoBehaviour
{
  public const string MainMenuSceneName = "MainMenuScene";
  public const string GameplaySceneName = "GameplayScene";
  public const string LoadingSceneName = "LoadingScene";

  public void ReloadCurrentScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void LoadScene(string sceneName)
  {
    SceneManager.LoadScene(sceneName);
  }

  public void LoadMainMenuScene()
  {
    LoadScene(MainMenuSceneName);
  }

  public void LoadGamePlayScene()
  {
    LoadScene(GameplaySceneName);
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