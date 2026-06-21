using Newtonsoft.Json;

public class PlayerDataDto
{
  [JsonProperty("id")]
  public string Id { get; set; }

  [JsonProperty("name")]
  public string Name { get; set; }

  [JsonProperty("avatarUrl")]
  public string AvatarUrl { get; set; }
}