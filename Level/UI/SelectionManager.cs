using UnityEngine;

public class SelectionManager : MonoBehaviour, ISelectionManager
{
  public InventoryManager inventoryManager;
  public int index;

  public void Hovering(int index){
    inventoryManager.Hovering(this.index, index);
  }

  public void Exit(int index){
    inventoryManager.EndHovering(this.index, index);
  }

  public void Select(int index){
    inventoryManager.Clicked(this.index, index);
  }
}
