using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTower : MonoBehaviour, ISelectionManager
{
  public InventoryManager inventoryManager;
  public TowerSelectButton towerSelectButton;
  public InventorySlot inventorySlotPrefab;
  public GameObject runeContainer;
  public Image background;
  public int runeSlotCount = 3;
  public Rune[] runes;
  private List<InventorySlot> runeSlots;
  private int index;
  private bool canBeEdited;
  public void Init(InventoryManager inventoryManager, TowerBlueprint towerBlueprint, int index){
    this.inventoryManager = inventoryManager;
    this.index = index;
    towerSelectButton.Init(this, towerBlueprint, -1);
    towerSelectButton.ShowIndex(false);
    towerSelectButton.showInfoOnHover = true;
    runes = new Rune[runeSlotCount];
    runeSlots = new List<InventorySlot>();
    for(int i = 0; i < runeSlotCount; i++){
      InventorySlot runeSlot = Instantiate(inventorySlotPrefab, runeContainer.transform);
      runeSlot.Init(this, i);
      runeSlot.SetIcon(null);
      runeSlots.Add(runeSlot);
    }
    UpdateStatus();
  }

  public void UpdateStatus(){
    TowerBlueprint blueprint = towerSelectButton.blueprint;
    canBeEdited = !GameManager.instance.buildManager.HasBeenBuilt(blueprint);
    background.color = canBeEdited ? Color.white : new Color(0.75f, 0.75f, 0.75f);
  }

  public void UpdateStatus(Rune rune){
    if (!canBeEdited){
      return;
    }
    bool canBeEquipepd = rune == null || rune.CanEquip(towerSelectButton.blueprint);
    SetRuneSlotsDisabled(!canBeEquipepd);
  }

  public void Select(int runeIndex){
    // Prevent towerSelectButton from triggering inventoryManager
    if (runeIndex < 0){
      return;
    }
    if (canBeEdited){
      inventoryManager.Clicked(this.index, runeIndex);
    }
  }

  public void Hovering(int runeIndex){
    if (runeIndex == -1){
      inventoryManager.towerInfo.Show(towerSelectButton.blueprint);
      return;
    }
    inventoryManager.Hovering(index, runeIndex);
  }
  public void Exit(int runeIndex){
    if (runeIndex == -1){
      inventoryManager.towerInfo.Hide();
      return;
    }
    inventoryManager.EndHovering(index, runeIndex);
  }

  public void SetRune(int runeIndex, Rune rune){
    runes[runeIndex] = rune;
    runeSlots[runeIndex].SetIcon(rune?.icon);
  }

  private void SetRuneSlotsDisabled(bool disabled){
    foreach(InventorySlot slot in runeSlots){
      slot.SetDisabled(disabled);
    }
  }
}
