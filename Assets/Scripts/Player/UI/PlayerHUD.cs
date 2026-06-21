using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
  [Header("Player Data Store")]
  [SerializeField] private PlayerDataSO _playerData;

  [Header("User Data UI References")]
  [Tooltip("Text component to display the player's name.")]
  [SerializeField] private TextMeshProUGUI _nameText;

  [Tooltip("Text component to display the player's ID.")]
  [SerializeField] private TextMeshProUGUI _idText;

  [Tooltip("Image component to display the player's avatar.")]
  [SerializeField] private Image _avatarImage;

  private void OnEnable()
  {
    if (_playerData is null)
    {
      Debug.LogError("[Player HUD] PlayerDataSO reference is not assigned.");
      return;
    }

    _playerData.OnDataChanged += OnPlayerDataChanged;
  }

  private void OnPlayerDataChanged(PlayerDataSO data)
  {
    _idText.text = $"ID: {data.Id}";
    _nameText.text = data.Name;

    StartCoroutine(ApiClient.Instance.DownloadImage(data.AvatarUrl, texture =>
    {
      _avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }));

    Debug.Log($"[Player HUD] User ID: {data.Id}, Name: {data.Name}, Avatar URL: {data.AvatarUrl}");
  }

  private void OnDisable()
  {
    if (_playerData is null)
    {
      Debug.LogError("[Player HUD] PlayerDataSO reference is not assigned.");
      return;
    }

    _playerData.OnDataChanged -= OnPlayerDataChanged;
  }
}