using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
  public string Id { get; private set; }
  public string Name { get; private set; }
  public string AvatarUrl { get; private set; }

  public event Action<PlayerDataSO> OnDataChanged;

  public void UpdateFromDto(PlayerDataDto dto)
  {
    Id = dto.Id;
    Name = dto.Name;
    AvatarUrl = dto.AvatarUrl;

    OnDataChanged?.Invoke(this);
  }
}
