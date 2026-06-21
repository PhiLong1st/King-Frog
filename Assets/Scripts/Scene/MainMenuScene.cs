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
  [SerializeField] private CanvasGroup _canvasGroup;

  private void Start()
  {
    _canvasGroup.alpha = 0f;
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

    IEnumerator fadeInCoroutine = AnimationUtils.FadeAnimation(0f, 1f, _fadeInDuration, alpha =>
    {
      AudioManager.Instance.SetMusicVolume(alpha);
      _canvasGroup.alpha = alpha;
    });
    yield return StartCoroutine(fadeInCoroutine);
  }

  public override IEnumerator AnimateUnload()
  {
    Debug.Log("MainMenuScene: AnimateUnload called.");

    IEnumerator fadeOutCoroutine = AnimationUtils.FadeAnimation(1f, 0f, _fadeOutDuration, alpha =>
    {
      AudioManager.Instance.SetMusicVolume(alpha);
      _canvasGroup.alpha = alpha;
    });

    yield return StartCoroutine(fadeOutCoroutine);
  }
}
