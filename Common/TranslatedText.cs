using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TranslatedText : MonoBehaviour
{
  public TextMeshProUGUI text;
  public StringFormat format;
  public string key;
  void Start(){
    if (text == null){
      TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
    }
    if (key != ""){
      SetKey(key);
    }
  }

  public void SetKey(string key, Dictionary<string, string> options = null){
    this.key = key;
    text.text = Translation.GetTranslation(key, format, options);
  }

  public void SetText(string text){
    this.key = "";
    this.text.text = text;
  }
}
