using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuScene : BaseScene
{
  [Tooltip("Time in seconds to wait before starting the fade-in of the main menu.")]
  [SerializeField] private float _fadeInDuration = 1f;

  [Tooltip("Time in seconds to wait before starting the fade-out of the main menu.")]
  [SerializeField] private float _fadeOutDuration = 1f;

  [Tooltip("CanvasGroup for the main menu scene, used to control its visibility and interactivity.")]
  [SerializeField] private CanvasGroup _mainMenuCanvasGroup;

  private void Start()
  {
    _mainMenuCanvasGroup.alpha = 0f;
  }

  // private void Update()
  // {
  //   if (Input.GetKeyDown(KeyCode.Space))
  //   {
  //     StartCoroutine(AnimateLoad());
  //   }

  //   if (Input.GetKeyDown(KeyCode.Escape))
  //   {
  //     StartCoroutine(AnimateUnload());
  //   }
  // }

  public override IEnumerator AnimateLoad()
  {
    Debug.Log("MainMenuScene: AnimateLoad called.");
    AudioManager.Instance.PlayMusic(AudioMusicEnum.MainMenuMusic);
    yield return StartCoroutine(AnimateEnter());
  }

  public override IEnumerator AnimateUnload()
  {
    Debug.Log("MainMenuScene: AnimateUnload called.");
    AudioManager.Instance.StopMusic();
    yield return StartCoroutine(AnimateExit());
  }

  private IEnumerator AnimateEnter()
  {
    float elapsedTime = 0f;
    while (elapsedTime < _fadeInDuration)
    {
      elapsedTime += Time.deltaTime;

      float alpha = Mathf.Clamp01(elapsedTime / _fadeInDuration);
      _mainMenuCanvasGroup.alpha = alpha;

      yield return null;
    }

    _mainMenuCanvasGroup.alpha = 1f;
  }

  private IEnumerator AnimateExit()
  {
    float elapsedTime = _fadeOutDuration;
    while (elapsedTime > 0f)
    {
      elapsedTime -= Time.deltaTime;

      float alpha = Mathf.Clamp01(elapsedTime / _fadeOutDuration);
      _mainMenuCanvasGroup.alpha = alpha;

      yield return null;
    }

    _mainMenuCanvasGroup.alpha = 0f;
  }
}
