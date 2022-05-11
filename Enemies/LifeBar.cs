using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
  public float heightModifier = 2.5f;
  public float widthModifier = 25f;
  public GameObject target;
  public Vector3 offset;
  private Vector3 currentOffset;
  public RectTransform rectTransform;
  public Image background;
  public Image lifeBar;
  public Color fullLifeColor = Color.green;
  public Color damagedLifeColor = Color.yellow;
  public Color lowLifeColor = Color.red;
  private float targetSize;

  public void Start(){
    background.gameObject.SetActive(false);
  }

  public void Init(GameObject target, Vector3 offset, float targetSize){
    this.target = target;
    this.offset = offset;
    this.targetSize = targetSize;
    UpdateSize();
  }

  public void Update(){
    if (LevelCameraController.HasDeltaMoved){
      UpdateSize();
      
    }
    transform.position = Camera.main.WorldToScreenPoint(target.transform.position) + currentOffset;
  }

  private void UpdateSize(){
    float dist = Camera.main.transform.position.y - target.transform.position.y;
    float scale = (4 - (dist / 5));
    currentOffset = new Vector3(offset.x, offset.y * scale, offset.z);
    float width = widthModifier * targetSize * scale;
    float height = heightModifier + 2 * targetSize * scale;
    rectTransform.sizeDelta = new Vector3(width, height);
  }

  public void SetScale(float f){
    if (f < 1f){
      background.gameObject.SetActive(true);
    }
    f = Mathf.Clamp(f, 0f, 1f);
    lifeBar.transform.localScale = new Vector3(f, 1f, 1f);

    UpdateColor(f);
  }

  private void UpdateColor(float f){
    if (f > 0.66f){
      lifeBar.color = fullLifeColor;
    } else if (f > 0.33f){
      lifeBar.color = damagedLifeColor;
    } else {
      lifeBar.color = lowLifeColor;
    }
  }
}
