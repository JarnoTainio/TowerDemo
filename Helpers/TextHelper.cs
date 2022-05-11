using System.Collections;
using UnityEngine;
using TMPro;
public class TextHelper
{
  public static IEnumerator PulseColor(TextMeshProUGUI text, Color color, float speed){
    Color defaulColor = text.color;
    text.color = color;
    while (text.color != defaulColor){
      text.color = Color.Lerp(text.color, defaulColor, Time.deltaTime * speed);
      yield return null;
    }
    text.color = defaulColor;
  }
}
