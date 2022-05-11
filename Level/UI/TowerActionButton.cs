using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerActionButton : MonoBehaviour
{
  public Button button;
  public TextMeshProUGUI costText;
  public Image frameImage;
  public Image backgroundImage;
  public Image iconImage;
  public Color costColor;
  public Color disabledCostColor;
  public Color disabledColor;

  public void Init(int cost){
    costText.text = cost.ToString();
  }

  public void SetEnabled(bool isEnabled){
    if (isEnabled){
      Enable();
    } else {
      Disable();
    }
  }

  public void Enable(){
    costText.color = costColor;
    SetImageColors(Color.white);
    button.enabled = true;
  }

  public void Disable(){
    costText.color = disabledCostColor;
    SetImageColors(disabledColor);
    button.enabled = false;
  }

  private void SetImageColors(Color color){
    frameImage.color = color;
    backgroundImage.color = color;
    iconImage.color = color;
  }
}
