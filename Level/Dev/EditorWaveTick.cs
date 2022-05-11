using UnityEngine;

public class EditorWaveTick : MonoBehaviour
{
  private WaveEditor waveEditor;
  public int x;
  public int y;
  public Color hoverColor;
  private Color defaultColor;
  private MeshRenderer meshRenderer;

  public void Init(WaveEditor waveEditor, int x, int y){
    this.waveEditor = waveEditor;
    this.x = x;
    this.y = y;
    meshRenderer = GetComponent<MeshRenderer>();
    UpdateDefaulColor();
  }

  void OnMouseDown()
  {
    waveEditor.TickClicked(this);
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