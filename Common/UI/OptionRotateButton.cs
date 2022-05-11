using UnityEngine;
using UnityEngine.UI;

public class OptionRotateButton : MonoBehaviour
{
  public IOptionGroupManager optionGroupManager;
  public Image iconImage;
  public TranslatedText text;
  public Sprite[] icons;
  public string[] strings;
  int index;

  public void OnClick(){
    index++;
    SelectIndex(index);
  }

  public void SelectIndex(int i){
    index = i;
    if (index >= icons.Length){
      index = 0;
    }
    iconImage.sprite = icons[index];
    if (text != null){
      text.SetKey(strings[index]);
    }
    optionGroupManager.OptionSelected(index);
  }
}
