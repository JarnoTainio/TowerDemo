using UnityEngine;
using TMPro;

public class FadingText : MonoBehaviour
{
  public TextMeshProUGUI textObject;
  public float movingSpeed;
  public float timeBeforeFading;
  public float fadingTime;
  private float totalFadingTime;
  private Color color;
  private Vector3 worldPosition;

  public void Init(string message, Vector3 worldPosition, Color color, float fontSize = 18){
    this.worldPosition = worldPosition;
    this.color = color;
    textObject.color = color;
    textObject.text = message;
    totalFadingTime = fadingTime;
    textObject.fontSize = fontSize;
    worldPosition = this.transform.position;
  }

  void Update()
  {
    float time = Time.unscaledDeltaTime;
    worldPosition += new Vector3(0, 0, movingSpeed * time);
    transform.position = Camera.main.WorldToScreenPoint(worldPosition);

    if (timeBeforeFading > 0f){
      timeBeforeFading -= time;
      return;
    }

    if (fadingTime > 0f){
      fadingTime -= time;
      textObject.color = new Color(color.r, color.g, color.b, fadingTime / totalFadingTime);
      return;
    }

    Destroy(gameObject);
  }
}
