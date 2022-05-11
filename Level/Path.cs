using UnityEngine;

public class Path : MonoBehaviour, ITile
{
  public Vector2Int position;
  public Renderer rend;
  public Color targetingColor;
  private Color defaultColor;
  public bool selected;
  void Start(){
    defaultColor = rend.material.color;
  }

  private void SetColor(Color color){ rend.material.color = color; }

  void OnMouseEnter(){ 
    selected = true; 
  }

  void OnMouseExit(){ 
    SetColor(defaultColor);
    selected = false; 
  }

  public void Targeting(){
    SetColor(targetingColor);
  }

  void OnMouseDown(){
    GameManager.instance.buildManager.PathClicked(this);
  }

  public Vector2Int GetPosition(){return position;}

  public bool Equals(ITile path){
    return position == path.GetPosition();
  }

  public bool IsTargeted(){
    return selected;
  }

  public Transform GetTransform(){
    return transform;
  }
}
