using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerRewardDialog : MonoBehaviour, ISelectionManager, IPanelObject
{
  [Header("References")]
  public PanelManager panelManager;
  public TowerInfo towerInfo;

  [Header("Components")]
  public GameObject buttonContainer;
  public TowerSelectButton towerSelectButtonPrefab;
  public Button confirmButton;

  [Header("Options")]
  public int optionCount = 3;
  
  private TowerBlueprint[] towerOptions;
  private List<TowerSelectButton> towerButtons;
  private int selectedIndex;

  void Init()
  {
    if (!DataManager.instance.runData.HasRoomForTower()){
      Close();
      return;
    }

    towerOptions = DataManager.instance.runData.GetTowerOptions();
    CreateButtons(towerOptions);
    confirmButton.interactable = false;
    panelManager.Open(this);
    gameObject.SetActive(true);
  }

  private void CreateButtons(TowerBlueprint[] options){
    towerButtons = new List<TowerSelectButton>();
    List<TowerBlueprint> optionList = new List<TowerBlueprint>(options);
    for(int i = 0; i < optionCount; i++){
      int roll = Random.Range(0, optionList.Count);
      TowerBlueprint towerBlueprint = optionList[roll];
      optionList.Remove(towerBlueprint);

      int index = ArrayHelper.GetIndex<TowerBlueprint>(towerOptions, towerBlueprint);
      
      TowerSelectButton towerSelectButton = Instantiate(towerSelectButtonPrefab, buttonContainer.transform);
      towerSelectButton.Init(this, towerBlueprint, index);
      towerSelectButton.ShowIndex(false);

      towerButtons.Add(towerSelectButton);
    }
  }

  public void Select(int index){
    selectedIndex = index;
    foreach(TowerSelectButton button in towerButtons){
      button.SetSelected(index == button.index);
      
    }
    towerInfo.Show(towerOptions[index]);
    confirmButton.interactable = true;
  }

  public void ConfirmSelection(){
    DataManager.instance.runData.AddTower(towerOptions[selectedIndex].nameKey);
    GameManager.instance.UpdateTowerOptions();
    gameObject.SetActive(false);
    panelManager.Close();
  }

  public void Open(){
    Init();
  }

  public void Close(){
    panelManager.Close();
  }
  
  public void Hovering(int index){}
  public void Exit(int index){}
}
