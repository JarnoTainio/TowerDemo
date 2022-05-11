using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour, IOptionGroupManager
{
  public Canvas canvas;
  public GameObject towerButtons;
  public TranslatedText towerName;
  public TowerActionButton sellButton;
  public TowerActionButton upgradeButton;
  public OptionRotateButton targetingSelect;
  public Image cooldownProgressImage;
  private Node selectedNode;
  public TowerInfo towerInfo;
  public TowerRange towerRange;
  private Node node;

  void Start(){
    targetingSelect.optionGroupManager = this;
  }
  void Update(){
    if (node?.tower != null){
      float progress = node.tower.GetCooldownPercentage();
      cooldownProgressImage.transform.localScale = new Vector3(progress, 1, 1);
    }
  }

  // ================================
  // Private
  // ================================

  private void SelectNode(Node node){
    selectedNode = node;
  }

  private void DeselectNode(){
    selectedNode = null;
  }

  private void UpdateUpgradeButton(Tower tower){
    if (tower.CanUpgrade()){
      upgradeButton.gameObject.SetActive(true);
      upgradeButton.Init(tower.GetUpgradeCost());
      upgradeButton.SetEnabled(tower.GetUpgradeCost() <= GameManager.instance.GetResources());
    }else{
      upgradeButton.gameObject.SetActive(false);
    }
  }

  private void ShowTowerInfo(Tower tower, bool upgraded = false){
    if (tower != null){
      towerRange.Show(tower, upgraded);
      towerInfo.Show(tower, upgraded);
    }else{
      towerRange.Hide();
      towerInfo.Hide();
    }
  }

  // ================================
  // Public
  // ================================

  public bool Toggle(Node node){
    if (selectedNode != node){
      Open(node);
      return true;
    } else{
      Close();
      return false;
    }
  }

  public void Open(Node node){
    this.node = node;
    SelectNode(node);
    transform.position =  Camera.main.WorldToScreenPoint(node.transform.position);
    towerButtons.SetActive(true);
    
    Tower tower = node.tower;
    sellButton.Init(tower.GetSellValue());
    UpdateUpgradeButton(tower);
    targetingSelect.gameObject.SetActive(tower.UseTargetingPriority());
    targetingSelect.SelectIndex((int)(tower.targetingPriority - 1));

    int level = tower.GetLevel();
    towerName.SetText(tower.GetBlueprint().GetName(level));

    Tower ghost = GameManager.instance.GetGhostTower();
    ShowTowerInfo(ghost != null ? ghost : node.tower, false);
  }

  public void Close(){
    if (selectedNode != null){
      DeselectNode();
      towerButtons.SetActive(false);
    }
    ShowTowerInfo(null);
  }

  public void ShowTowerRange(Tower tower){
    towerRange.Show(tower, false);
  }

  public void HideTowerRange(){
    towerRange.Hide();
  }

  public void SellTower(){
    selectedNode.SellTower();
    Close();
  }

  public void ResourcesModified(int resources){
    if (selectedNode != null){
      UpdateUpgradeButton(selectedNode.tower);
    }
  }

  public void UpgradeTower(){
    selectedNode.UpgradeTower();
    if (selectedNode.tower.CanUpgrade()){
      MouseOverUpgradeButton();
    }
  }


  public void OptionSelected(int index){
    selectedNode.tower.targetingPriority = ((TargetingPriority)(index + 1));
  }

  public Color GetSelectedColor(){return Color.white; }

  public void MouseOverUpgradeButton(){
    ShowTowerInfo(node.tower, true);
  }
  public void MouseExitUpgradeButton(){ 
    ShowTowerInfo(node.tower);
  }

  public void TowerBlueprintSelected(TowerBlueprint blueprint){
    Tower tower = GameManager.instance.GetGhostTower();
    towerInfo.Show(tower, false);
  }
}
