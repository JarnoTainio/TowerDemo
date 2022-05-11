using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTile : MonoBehaviour
{
  private LevelEditor levelEditor;
  public int x;
  public int y;
  public Color hoverColor;
  private Color defaultColor;
  private MeshRenderer meshRenderer;

  public void Init(LevelEditor levelEditor, int x, int y){
    this.levelEditor = levelEditor;
    this.x = x;
    this.y = y;
    meshRenderer = GetComponent<MeshRenderer>();
    UpdateDefaulColor();
  }
  void OnMouseDown()
  {
    levelEditor.TileClicked(this);
  }

  public void UpdateDefaulColor(){
    defaultColor = meshRenderer.material.color;
  }

  public void SetColor(Color color){
    meshRenderer.material.color = color;
    UpdateDefaulColor();
  }

  void OnMouseEnter(){
    if (Input.GetMouseButton(0)){
      OnMouseDown();
      return;
    }
    meshRenderer.material.color = Color.Lerp(defaultColor, hoverColor, 1f);
  }

  void OnMouseExit(){
    meshRenderer.material.color = defaultColor;
  }
}
