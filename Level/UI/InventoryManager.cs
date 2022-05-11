using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IPanelObject
{
  [Header("References")]
  public PanelManager panelManager;
  public TowerSelect towerSelect;
  public SelectionManager storageManager;
  public TowerInfo towerInfo;
  public Button inventoryButton;

  [Header("Container")]
  public GameObject towerContainer;
  public SelectionManager storageContainer;
  public Image handImage;

  [Header("Prefabs")]
  public InventoryTower inventoryTowerPrefab;
  public InventorySlot inventorySlotPrefab;

  // Private
  private TowerBlueprint[] towerBlueprints;
  private Dictionary<string, Rune> runeDictionary;
  private List<InventoryTower> towerList;
  private List<InventorySlot> storageList;
  private Rune handRune;
  private Vector2Int handRunePreviousSlot;
  private bool initDone;

  // ================================
  // Unity functions
  // ================================

  void Update(){
    if (handRune != null){
      handImage.transform.position = Input.mousePosition;
      if (Input.GetMouseButtonDown(1)){
        Clicked(handRunePreviousSlot.x, handRunePreviousSlot.y);
      }
    }
  }

  // ================================
  // Public functions
  // ================================

  public void Open(){
    if (!initDone){
      runeDictionary = new Dictionary<string, Rune>();
      towerBlueprints = DataManager.instance.runData.GetTowerBlueprints();
      Init();
    } else {
      foreach(InventoryTower inventoryTower in towerList){
        inventoryTower.UpdateStatus();
      }
    }

    /* TODO: Hide inventory if player has no runes
    if (runeDictionary.Keys.Count == 0){
      inventoryButton.gameObject.SetActive(false);
      ConfirmAndClose();
    } else {

    }
    */

    gameObject.SetActive(true);
    panelManager.Open(this);
  }

  public void Clicked(int towerIndex, int runeIndex){
    if (towerIndex >= 0 && handRune != null){
      TowerBlueprint blueprint = towerBlueprints[towerIndex];
      if (!handRune.CanEquip(blueprint)){
        return;
      }
    }

    Rune tempRune = handRune;
    handRune = GetRune(towerIndex, runeIndex);
    SetRune(tempRune, towerIndex, runeIndex);

    handImage.gameObject.SetActive(handRune != null);
    handImage.sprite = handRune?.icon;
    handRunePreviousSlot = new Vector2Int(towerIndex, runeIndex);

    if (towerIndex >= 0 ){
      towerSelect.UpdateTowerCost(towerBlueprints[towerIndex]);
    }
    UpdateTowerCanEquipRune(handRune);
  }

  public void Hovering(int towerIndex, int runeIndex){
    Rune rune = GetRune(towerIndex, runeIndex);
    towerInfo.Show(rune);
  }

  public void EndHovering(int towerIndex, int runeIndex){
    towerInfo.Hide();
  }

  public void ConfirmAndClose(){
    gameObject.SetActive(false);
    towerSelect.LoadTowerButtons();
    panelManager.Close();
    GameManager.instance.InventoryUpdated();
  }

  public void Close(){}

  // ================================
  // Private functions
  // ================================

  private void Init(){
    CreateTowerButtons();
    CreateStorage();
    InitHandSlot();
    initDone = true;
  }

  private void CreateTowerButtons(){
    towerList = new List<InventoryTower>();
    for(int i = 0; i < towerBlueprints.Length; i++){
      InventoryTower inventoryTower = CreateTowerItem(towerBlueprints[i], i);
      towerList.Add(inventoryTower);

      Rune[] runes = DataManager.instance.runData.GetRunes(i);
      for(int r = 0; r < runes.Length; r++){
        if (runes[r] != null){
          SetRune(runes[r], i, r, false);
        }
      }
    }
  }

  private void InitHandSlot(){
    handImage.gameObject.SetActive(false);
    handRune = null;
  }

  private InventoryTower CreateTowerItem(TowerBlueprint blueprint, int index){
    InventoryTower inventoryTower = Instantiate(inventoryTowerPrefab, towerContainer.transform);
    inventoryTower.Init(this, blueprint, index);
    return inventoryTower;
  }

  private void CreateStorage(){
    storageList = new List<InventorySlot>();
    RunData runData = DataManager.instance.runData;
    Rune[] runes = runData.GetRunes(-1);
    for(int i = 0; i < runes.Length; i++){
      InventorySlot inventorySlot = Instantiate(inventorySlotPrefab, storageContainer.transform);
      storageList.Add(inventorySlot);
      inventorySlot.Init(storageContainer, i);
      SetRune(runes[i], -1, i, false);
    }
  }

  private Rune GetRune(int towerIndex, int runeIndex){
    string key = $"{towerIndex}_{runeIndex}";
    if (runeDictionary.ContainsKey(key)){
      return runeDictionary[key];
    }
    return null;
  }

  private void SetRune(Rune rune, int towerIndex, int runeIndex, bool save = true){
    string key = $"{towerIndex}_{runeIndex}";
    runeDictionary[key] = rune;
    if (towerIndex >= 0){
      towerList[towerIndex].SetRune(runeIndex, rune);
    } else{
      storageList[runeIndex].SetIcon(rune?.icon);
    }

    if (save){
      towerInfo.Show(rune);
      DataManager.instance.runData.SetRune(towerIndex, runeIndex, rune?.nameKey);
    }
  }

  private void UpdateTowerCanEquipRune(Rune rune){
    foreach(InventoryTower iTower in towerList){
      iTower.UpdateStatus(rune);
    }
  }
}
