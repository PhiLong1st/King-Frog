using UnityEngine;

public enum GameState
{
  Playing,
  Paused,
  GameOver
}

public class GameManager : MonoBehaviour
{
  [Header("Player")]
  [SerializeField] private PlayerController _player;

  [Header("Key")]
  [SerializeField] private Key _key;

  [Header("Chest")]
  [SerializeField] private Chest _chest;

  public GameState CurrentGameState { get; private set; }
  private bool _hasKey = false;

  public void OnKeyCollected()
  {
    _hasKey = true;
    Debug.Log("Player has the key and can open the chest!");
  }

  public void OnChestOpened()
  {
    if (!_hasKey) return;

    OnGameWin();
    Debug.Log("Player has the key and can open the chest!");
  }

  public void ResetGame()
  {
    _hasKey = false;
    _player.Reset();
    _key.Reset();
    _chest.Reset();
  }

  private void OnEnable()
  {
    _player.OnPlayerDead += OnGameOver;
    _chest.OnChestOpened += OnChestOpened;
    _key.OnKeyCollected += OnKeyCollected;
  }

  private void OnDisable()
  {
    _player.OnPlayerDead -= OnGameOver;
    _chest.OnChestOpened -= OnChestOpened;
    _key.OnKeyCollected -= OnKeyCollected;
  }

  private void OnGameWin()
  {
    AudioManager.Instance.PlaySFX(AudioSFXEnum.GameWin);
    PanelManager.Instance.OpenPanel(PanelName.GameWinPanel);
    Debug.Log("Game Won!");
  }

  private void OnGameOver()
  {
    AudioManager.Instance.PlaySFX(AudioSFXEnum.GameOver);
    PanelManager.Instance.OpenPanel(PanelName.GameOverPanel);
    Debug.Log("Game Over!");
  }
}