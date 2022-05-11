using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneRewardDialog : MonoBehaviour, ISelectionManager, IPanelObject
{
  [Header("References")]
  public PanelManager panelManager;
  public TowerInfo towerInfo;

  [Header("Components")]
  public GameObject buttonContainer;
  public SelectButton selectButtonPrefab;
  public Button confirmButton;

  [Header("Options")]
  public int optionCount = 3;
  
  private Rune[] runeOptions;
  private List<SelectButton> runeButtons;
  private int selectedIndex;

  // ================================
  // Private functions
  // ================================

  private void Init()
  {
    WeightedList<Rune> weightedList = DataManager.instance.runData.GetRuneOptions();
    runeOptions = weightedList.ToArray();
    if (runeOptions.Length == 0){
      Close();
      return;
    }
    CreateButtons(weightedList);
    confirmButton.interactable = false;
    gameObject.SetActive(true);
    panelManager.Open(this);
  }

  private void CreateButtons(WeightedList<Rune> weightedList){
    if (runeOptions.Length < optionCount){
      optionCount = runeOptions.Length;
    }

    runeButtons = new List<SelectButton>();
    for(int i = 0; i < optionCount; i++){
      Rune rune = weightedList.GetRandom();
      weightedList.Remove(rune);
      int index = ArrayHelper.GetIndex<Rune>(runeOptions, rune);
      
      SelectButton selectButton = Instantiate(selectButtonPrefab, buttonContainer.transform);
      selectButton.Init(this, index);
      selectButton.SetSprite(rune.icon);

      runeButtons.Add(selectButton);
    }
  }

  // ================================
  // Public functions
  // ================================

  public void Select(int index){
    selectedIndex = index;
    foreach(SelectButton button in runeButtons){
      button.SetSelected(index == button.index);
      
    }
    towerInfo.Show(runeOptions[index]);
    confirmButton.interactable = true;
  }

  public void ConfirmSelection(){
    DataManager.instance.runData.AddRune(runeOptions[selectedIndex].nameKey);
    GameManager.instance.UpdateTowerOptions();
    towerInfo.Hide();
    gameObject.SetActive(false);
    panelManager.Close();
  }

  public void Open(){
    Init();
  }

  public void Close(){
    gameObject.SetActive(false);
    panelManager.Close();
  }
  
  public void Hovering(int index){}
  public void Exit(int index){}
}
