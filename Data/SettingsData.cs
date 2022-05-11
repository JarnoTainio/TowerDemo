using UnityEngine;
[System.Serializable]
public class SettingsData
{
  public Language language = Language.EN;

  [Header("Level")]
  public bool showDamageNumbers = true;
  public int minimumDamageShown = 20;
  public bool showResourcesGainedNumbers = true;
}
