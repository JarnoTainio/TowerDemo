using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerSelectButton : SelectButton
{
  [Header("References")]
  public TextMeshProUGUI costText;
  public TextMeshProUGUI buttonText;
  public GameObject buttonTextGO;
  public Image fillImage;

  [Header("Attributes")]
  public TowerBlueprint blueprint;

  // Private
  private int cost;
  private bool canBuy;

  public void Init(ISelectionManager selectionManager, TowerBlueprint blueprint, int index){
    Init(selectionManager, index);
    this.blueprint = blueprint;
    SetSprite(blueprint.icon);
    cost = blueprint.GetCost(0);
    costText.text = cost.ToString();
    buttonText.text = index < 9 ? (index + 1).ToString() : "-";
  }

  public void ShowIndex(bool show){
    buttonTextGO.SetActive(show);
  }

  public void SelectTower(){
    selectionManager.Select(index);
  }

  public override Color SetSelected(bool isSelected){
    Color color = base.SetSelected(isSelected);
    buttonText.color = color;
    return color;
  }

  public void UpdateResources(int amount){
    canBuy = cost <= amount;
    costText.color = canBuy ? costPositiveColor : costNegativeColor;
  }

  public bool HasBlueprint(TowerBlueprint blueprint){
    return this.blueprint == blueprint;
  }

  public void UpdateCost(int resources, int cost){
    this.cost = cost;
    costText.text = cost.ToString();
    UpdateResources(resources);
  }

  public void SetFill(float f){
    fillImage.fillAmount = 1 - f;
    iconImage.color = f < 1f ? Color.gray : Color.white;
  }
}
