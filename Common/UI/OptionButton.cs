using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
  private IOptionGroupManager groupManager;
  private int index;
  public Image frame;
  private Color defaultColor;

  void Awake(){
    defaultColor = frame.color;
  }
  
  public void Init(IOptionGroupManager groupManager, int index){
    this.groupManager = groupManager;
    this.index = index;
  }

  public void OnClick(){
    groupManager.OptionSelected(index);
  }

  public void SetSelected(int selectedindex){
    if (index == selectedindex){
      frame.color = groupManager.GetSelectedColor();
    } else {
      frame.color = defaultColor;
    }
  }
}
