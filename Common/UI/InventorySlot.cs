using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
  public Image icon;
  public GameObject disabledImage;
  private ISelectionManager selectionManager;
  private int index;

  public void Init(ISelectionManager selectionManager, int index){
    this.selectionManager = selectionManager;
    this.index = index;
  }

  public void SetIcon(Sprite sprite){
    icon.gameObject.SetActive(sprite != null);
    icon.sprite = sprite;
  }

  public void SetDisabled(bool disabled){
    disabledImage.SetActive(disabled);
  }

  public void OnMouseDown(){
    selectionManager.Select(index);
  }

  public void OnMouseEnter(){
    selectionManager.Hovering(index);
  }

  public void OnMouseExit(){
    selectionManager.Exit(index);
  }
}
