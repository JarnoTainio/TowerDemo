using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
  public Image frameImage;
  public Image iconImage;
  public Color selectedColor = Color.green;
  public Color costPositiveColor = Color.yellow;
  public Color costNegativeColor = Color.red;
  public int index;
  public bool showInfoOnHover;

  protected ISelectionManager selectionManager;
  protected Color defaulColor;
  protected bool isSelected;

  public void Init(ISelectionManager selectionManager, int index){
    defaulColor = frameImage.color;
    this.selectionManager = selectionManager;
    this.index = index;
  }

  public void SetSprite(Sprite sprite){
    iconImage.sprite = sprite;
  }

  public void Select(){
    selectionManager.Select(index);
  }

  public virtual Color SetSelected(bool isSelected){
    this.isSelected = isSelected;
    Color color = isSelected ? selectedColor : defaulColor;
    frameImage.color = color;
    return color;
  }

  public void MouseEnter(){
    if (!showInfoOnHover){
      return;
    }
    selectionManager.Hovering(index);
  }

  public void MouseExit(){
    if (!showInfoOnHover){
      return;
    }
    selectionManager.Exit(index);
  }
}
