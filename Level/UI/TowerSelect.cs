using UnityEngine;

public class TowerSelect : MonoBehaviour, ISelectionManager
{
  public TowerUI towerUI;
  public BuildManager buildManager;
  public TowerSelectButton towerButtonPrefab;
  private TowerSelectButton[] towerButtons;
  private TowerSelectButton cooldownTowerButton;
  private TowerBlueprint cooldownTower;

  void Awake(){
    LoadTowerButtons();
  }

  public void LoadTowerButtons(){
    TowerBlueprint[] list = DataManager.instance.runData.GetTowerBlueprints();
    if (towerButtons != null && list.Length > towerButtons.Length){
      DestroyButtons();
      towerButtons = null;
    }
    if (towerButtons == null){
      towerButtons = new TowerSelectButton[list.Length];
    }
    for(int i = 0; i < list.Length; i++){
      TowerSelectButton button = towerButtons[i];
      if (towerButtons[i] == null){
        button = Instantiate(towerButtonPrefab, transform);
        towerButtons[i] = button;
      }
      button.Init(this, list[i], i);
    }

    if (cooldownTower != null){
      InitTowerCooldown(cooldownTower);
    }
  }

  public void Select(int index){
    buildManager.SelectTowerToBuild(index);
  }

  public void ShowTowerInfo(int index){
    TowerSelectButton selectedButton = towerButtons[index];
    foreach(TowerSelectButton button in towerButtons){
      if (button == selectedButton){
        towerUI.TowerBlueprintSelected(button.blueprint);
      }
      button.SetSelected(button == selectedButton);
    }
  }

  public void DeselectAllButtons(){
    foreach(TowerSelectButton button in towerButtons){
      button.SetSelected(false);
    }
  }

  public void UpdateResources(int amount){
    if (towerButtons == null){
      return;
    }

    foreach(TowerSelectButton button in towerButtons){
      button.UpdateResources(amount);
    }
  }

  public void UpdateTowerCost(TowerBlueprint blueprint, int cost = -1){
    if (cost == -1){
      cost = buildManager.GetTowerCost(blueprint, 0);
    }
    foreach(TowerSelectButton button in towerButtons){
      if (button.HasBlueprint(blueprint)){
        button.UpdateCost(GameManager.instance.GetResources(), cost);
        break;
      }
    }
  }
  
  public void Hovering(int index){
    towerUI.towerInfo.Show(towerButtons[index].blueprint);
  }
  public void Exit(int index){
    towerUI.towerInfo.Hide();
  }

  public void UpdateTowerCooldown(float f){
    cooldownTowerButton.SetFill(f);
  }

  public void InitTowerCooldown(TowerBlueprint blueprint){
    cooldownTower = blueprint;
    foreach(TowerSelectButton button in towerButtons){
      if (button.HasBlueprint(blueprint)){
        button.costText.gameObject.SetActive(false);
        button.fillImage.gameObject.SetActive(true);
        cooldownTowerButton = button;
      } else {
        button.costText.gameObject.SetActive(true);
        button.fillImage.gameObject.SetActive(false);
      }
    }
  }

  private void DestroyButtons(){
    foreach(TowerSelectButton button in towerButtons){
      Destroy(button.gameObject);
    }
  }
}
