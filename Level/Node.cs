using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, ITile
{
  public static int ID = 0;
  public int id;

  [Header("Attributes")]
  public Vector2Int position;
  public bool isBlocking;
  public bool isAdjacentToPath;

  [Header("Colors")]
  private Renderer rend;
  public Color canBuildColor;
  public Color errorColor;
  private Color defaultColor;

  [Header("Object")]
  public Tower tower;

  private BuildManager buildManager;

  private bool selected;

  // ==========================
  // Unity functions
  // ==========================
  public void Start(){
    id = ID++;
    rend = GetComponent<Renderer>();
    defaultColor = rend.material.color;
    buildManager = BuildManager.instance;
  }

  public void OnMouseEnter(){ 
    selected = true;
    Select(); 
  }

  public void OnMouseExit(){ 
    selected = false;
    Deselect(); 
  }

  public void OnMouseDown ()
  {
    // Over UI
    if (EventSystem.current.IsPointerOverGameObject()){
      return;
    }

    // Selecting path for building
    if (buildManager.InTargetingMode()){
      GameManager.instance.buildManager.PathClicked(this);
      return;
    }
    
    // Node has a tower
    bool InBuildingMode = buildManager.InBuildingMode();
    if (tower != null){
      // Target node and show turret info
      buildManager.ExitBuildMode();
      buildManager.SelectNode(this);
      return;
    }

    // Not in building mode so do nothing
    if (!InBuildingMode){
      return;
    }

    // Can't build a tower here
    if (!buildManager.CanBuildTower(this, true)){
      return;
    }

    // Build tower
    Tower t = buildManager.BuildSelectedTower(this);
    if (t == null) {
      return;
    }
    // Set tower
    tower = t;
    Deselect();
  }

  // ==========================
  // Private
  // ==========================

  private void SetColor(Color color){ rend.material.color = color; }

  private void UpdateColor(bool selected){
    if (selected){
      UpdateSelectedColor();
    } else {
      SetColor(defaultColor);
    }
  }

  private void UpdateSelectedColor(){
    if (tower != null){
      if (buildManager.InBuildingMode()){
        SetColor(errorColor);
        buildManager.SetGhostStatus(false);
      }else{
        SetColor(Color.yellow);
      }
    }
    else if (!buildManager.InBuildingMode()){
      SetColor(defaultColor);
    }
    else if (buildManager.InTargetingMode()){
      SetColor(defaultColor);
    }
    else if (buildManager.CanBuildTower(this)){
      SetColor(canBuildColor);
      buildManager.SetGhostStatus(true);
    } else {
      SetColor(errorColor);
      buildManager.SetGhostStatus(false);
    }
  }

  // ==========================
  // Public
  // ==========================

  public void Select(){
    buildManager.Hovering(this);
    UpdateColor(true);
    
    if (tower != null){
      return;
    }

    if (!buildManager.InBuildingMode()){
      return;
    }

    UpdateBuildingColor();
    GameManager.instance.resourceModified += ResourcesModified;
  }

  public void Deselect(){
    buildManager.StopHovering(this);
    UpdateColor(false);
    
    GameManager.instance.resourceModified -= ResourcesModified;
  }

  public void UpdateBuildingColor(){
    UpdateColor(true);
  }

  public void SellTower(){
    if (tower != null){
      buildManager.SellTower(this);
      Destroy(tower.gameObject);
      tower = null;
    }
  }

  public void UpgradeTower(){
    tower = buildManager.UpgradeTower(this);
    buildManager.ShowNode(this);
  }

  public void ResourcesModified(int resources){ UpdateBuildingColor(); }

  public Vector2Int GetPosition(){return position;}

  public bool Equals(ITile path){
    return position == path.GetPosition();
  }

  public bool IsTargeted(){
    return selected;
  }

  public void Targeting(){
    SetColor(Color.yellow);
  }

  public Transform GetTransform(){
    return transform;
  }
}
