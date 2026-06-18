using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneLoader : MonoBehaviour
{
  private const string MainMenuSceneName = "MainMenuScene";
  private const string GameplaySceneName = "GameplayScene";

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