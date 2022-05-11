using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeIn : MonoBehaviour
{
  public Image image;
  public float duration = 1f;
  public float wait = 0f;
  public List<TextMeshProUGUI> fadingTexts = new List<TextMeshProUGUI>();

  public void StartFadeIn(Action<bool> FadeCompleted = null)
  {
    image.gameObject.SetActive(true);
    StartCoroutine(Fade(duration, true, FadeCompleted));
  }

  public void StartFadeOut(Action<bool> FadeCompleted)
  {
    image.gameObject.SetActive(true);
    StartCoroutine(Fade(duration, false, FadeCompleted));
  }

  public IEnumerator Fade(float duration, bool fadeIn, Action<bool> FadeCompleted)
  {
    float f = duration + (fadeIn ? wait : 0);
    while (f > 0)
    {
      f -= Time.unscaledDeltaTime;
      float value = f / duration;
      image.color = new Color(0, 0, 0, fadeIn ? value : 1 - value);
      if (fadeIn)
      {
        foreach (TextMeshProUGUI txt in fadingTexts)
        {
          txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, f);
        }
      }
      yield return null;
    }
    image.gameObject.SetActive(fadeIn == false);
    if (FadeCompleted != null)
    {
      FadeCompleted(true);
    }
  }
}
