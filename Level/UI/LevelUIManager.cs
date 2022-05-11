using UnityEngine;
using TMPro;

public class LevelUIManager : MonoBehaviour
{
  [Header("Player UI")]
  public TextMeshProUGUI resourceText;
  public TextMeshProUGUI livesText;
  public TowerSelect towerSelect;

  [Header("Dialogs")]
  public TowerRewardDialog towerRewardDialog;
  public RuneRewardDialog runeRewardDialog;

  [Header("References")]
  public FadeIn fadeIn;
  public FadingText fadingTextPrefab;

  // ================================
  // Unity functions
  // ================================

  void Awake(){
    fadeIn.StartFadeIn();
  }

  // ================================
  // Public functions
  // ================================

  public void UpdateResources(int amount){
    resourceText.text = amount.ToString();
    towerSelect.UpdateResources(amount);
  }

  public void ShowResourceError(){
    StartCoroutine(TextHelper.PulseColor(resourceText, Color.red, 5f));
  }

  public void UpdateLives(int amount){
    livesText.text = amount.ToString();
  }

  public void ExitBuildMode(){
    towerSelect.DeselectAllButtons();
  }

  public void UpdateTowerPrice(TowerBlueprint blueprint, int cost){
    towerSelect.UpdateTowerCost(blueprint, cost);
  }

  public void CreateResourceGainedText(GameObject worldObject, int value){
    if (!DataManager.instance.settings.showResourcesGainedNumbers){
      return;
    }
    Vector3 canvasPosition = Camera.main.WorldToScreenPoint(worldObject.transform.position);
    FadingText fadingText = Instantiate(fadingTextPrefab, canvasPosition, Quaternion.identity, transform);
    fadingText.Init($"+{value}", worldObject.transform.position, Color.yellow, 12 + (value / 5) * 2);
  }

  public void CreateDamageText(GameObject worldObject, int value){
    Vector3 canvasPosition = Camera.main.WorldToScreenPoint(worldObject.transform.position);
    FadingText fadingText = Instantiate(fadingTextPrefab, canvasPosition, Quaternion.identity, transform);
    fadingText.Init($"{value}", worldObject.transform.position, Color.red, 8 + (value / 10) );
    fadingText.fadingTime = 0.5f;
    fadingText.movingSpeed = .2f;
  }
}
