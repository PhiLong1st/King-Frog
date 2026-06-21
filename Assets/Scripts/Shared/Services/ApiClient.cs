using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

public class UserData
{
  [JsonProperty("id")]
  public string Id { get; set; }

  [JsonProperty("name")]
  public string Name { get; set; }

  [JsonProperty("avatarUrl")]
  public string AvatarUrl { get; set; }
}

public class ApiClient : Singleton<ApiClient>
{
  private string _baseUrl = "https://6a3659e7766b831960f92698.mockapi.io";

  public UnityWebRequestAsyncOperation GetUserByIdAsync(string userId, Action<UserData> onSuccess = null, Action<string> onError = null)
  {
    string url = $"{_baseUrl}/users/{userId}";
    UnityWebRequest webRequest = UnityWebRequest.Get(url);

    UnityWebRequestAsyncOperation operation = webRequest.SendWebRequest();
    operation.completed += (asyncOperation) =>
    {
      if (webRequest.result == UnityWebRequest.Result.Success)
      {
        string jsonResponse = webRequest.downloadHandler.text;
        var userData = JsonConvert.DeserializeObject<UserData>(jsonResponse);

        onSuccess?.Invoke(userData);
      }
      else
      {
        onError?.Invoke(webRequest.error);
      }

      webRequest.Dispose();
    };

    return operation;
  }

  public IEnumerator DownloadImage(string imagUrl, Action<Texture2D> onSuccess = null, Action<string> onError = null)
  {
    UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imagUrl);

    yield return webRequest.SendWebRequest();

    if (webRequest.result == UnityWebRequest.Result.Success)
    {
      Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
      onSuccess?.Invoke(texture);
    }
    else
    {
      onError?.Invoke(webRequest.error);
    }

    webRequest.Dispose();
  }
}