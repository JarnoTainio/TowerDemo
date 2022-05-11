using System;
using UnityEngine;

public class LevelNode : MonoBehaviour
{
  public int index;
  public Action<int> callback;
  public Renderer rend;
  private Color defaultColor;
  public Color hoveringColor;
  public bool selected;

  void Start(){
    defaultColor = rend.material.color;
  }

  private void SetColor(Color color){ rend.material.color = color; }

  public void Init(int index, Action<int> callback){
    this.index = index;
    this.callback = callback;
  }

  void OnMouseEnter(){ 
    SetColor(hoveringColor);
  }

  void OnMouseExit(){ 
    SetColor(defaultColor);
    selected = false; 
  }

  public void OnMouseDown(){
    callback(index);
  }
}
