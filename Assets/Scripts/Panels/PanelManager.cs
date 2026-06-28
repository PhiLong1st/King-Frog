using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public enum PanelName
{
  MainMenuPanel,
  SettingPanel,
  LevelPanel,
  GameWinPanel,
  GameOverPanel,
}

[Serializable]
public class PanelData
{
  public PanelName Name;
  public Panel Panel;
}

public class PanelManager : Singleton<PanelManager>
{
  [SerializeField] private PanelData[] _panelDatas;
  private Dictionary<PanelName, Panel> _panelDictionary;

  protected override void Awake()
  {
    base.Awake();

    _panelDictionary = new();

    foreach (PanelData data in _panelDatas)
    {
      if (_panelDictionary.ContainsKey(data.Name))
      {
        Debug.LogWarning($"Duplicate Panel name detected: {data.Name}. Skipping this entry.");
        continue;
      }

      Panel Panel = data.Panel;
      _panelDictionary.Add(data.Name, Panel);
    }
  }

  public void OpenPanel(PanelName name)
  {
    if (!_panelDictionary.TryGetValue(name, out var panel))
    {
      Debug.LogError($"Panel '{name}' not found in the Panel dictionary.");
      return;
    }

    panel.ShowPanel();
  }

  public void ClosePanel(PanelName name)
  {
    if (!_panelDictionary.TryGetValue(name, out var panel))
    {
      Debug.LogError($"Panel '{name}' not found in the Panel dictionary.");
      return;
    }

    panel.HidePanel();
  }
}