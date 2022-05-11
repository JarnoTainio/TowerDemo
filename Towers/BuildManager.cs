using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
  [HideInInspector]
  public static BuildManager instance;

  [Header("References")]
  public LevelUIManager uiManager;
  public CooldownManager cooldownManager;
  public TowerGhost towerGhost;
  
  [Header("Data")]
  public TowerBlueprint[] towerList;
  public GameObject buildEffect;

  // Private
  private GameManager gameManager;
  private Dictionary<TowerBlueprint,int> dictionary;
  private TowerBlueprint selectedBlueprint;
  private Node hoveredNode;
  private bool tileTargeting;
  private List<ITile> targetablePaths;
  private ITile currentPath;

  void Awake()
  {
    if (instance != null)
    {
      Destroy(gameObject);
      return;
    }
    instance = this;
    UpdateTowerList();
  }

  void Start(){
    gameManager = GameManager.instance;
    dictionary = new Dictionary<TowerBlueprint,int>();
  }

  void Update(){
    towerGhost.UpdatePosition(hoveredNode);

    if (tileTargeting){
      TileTargeting();
    }
  }


  /*========================
   * Public functions
   *========================*/

  public void SelectTowerToBuild(int index){
    if (index < towerList.Length){
      SelectTowerBlueprint(towerList[index]);
      uiManager.towerSelect.ShowTowerInfo(index);
    }
  }

  public void ExitBuildMode(){
    if (tileTargeting){
      tileTargeting = false;
      return;
    }

    towerGhost.Destroy();

    selectedBlueprint = null;
    cooldownManager.BlueprintSelected(null);
    uiManager.ExitBuildMode();

    if(hoveredNode != null){ 
      hoveredNode.Select();
    }
  }

  public Tower BuildSelectedTower(Node node){
    TowerBlueprint blueprint = GetTowerToBuild();
    if (blueprint.towerType == TowerType.pathAdjacent){
      if (tileTargeting){
        StopTileTargeting();
      } else {
        StartTileTargeting(node);
        return null;
      }
    }

    Tower tower = BuildTower(node, blueprint, 0, currentPath);
    WaveManager.levelStarted = true;
    GameManager.instance.pathManager.TowerBuilt(node.position);

    return tower;
  }

  public Tower UpgradeTower(Node node){
    Tower tower = node.tower;
    TowerBlueprint blueprint = tower.GetBlueprint();
    int upgradedLevel = tower.GetLevel() + 1;

    Tower upgradedTower = BuildTower(node, blueprint, upgradedLevel);
    return upgradedTower;
  }

  public void SellTower(Node node){
    Tower tower = node.tower;
    TowerBlueprint blueprint = tower.GetBlueprint();
    
    int sellValue = tower.GetSellValue();
    GameManager.instance.ModifyResources(sellValue);
    ModifyTowerCount(blueprint, -1);

    int towerCost = GetTowerCost(blueprint, 0);
    uiManager.UpdateTowerPrice(blueprint, towerCost);
    GameManager.instance.HideTowerInfo();
    GameManager.instance.pathManager.TowerSold(node.position, !node.isBlocking);
  }

  public void SelectTowerBlueprint(TowerBlueprint blueprint){
    if (blueprint == selectedBlueprint) {
      return;
    }

    GameManager.instance.HideTowerInfo();
    selectedBlueprint = blueprint;
    cooldownManager.BlueprintSelected(blueprint);
    tileTargeting = false;

    towerGhost.Create(blueprint);

    if (hoveredNode != null){
      hoveredNode.UpdateBuildingColor();
    }
  }

  public bool InBuildingMode(){ return selectedBlueprint != null; }
  public bool InTargetingMode(){ return tileTargeting; }

  public bool CanBuildTower(Node node, bool showError = false){
    if (node.tower != null){
      return false;
    }
    if (!HasResourcesForTower(showError)){
      return false;
    }
    if (!selectedBlueprint.CanBuild(node)){
      return false;
    }
    
    return true;
  }

  public void SelectNode(Node node){
    if (InTargetingMode()){
      return;
    }
    GameManager.instance.ToggleTowerInfo(node);
  }

  public void ShowNode(Node node){
    GameManager.instance.ShowTowerInfo(node);
  }

  public void Hovering(Node node){
    if (InTargetingMode()){
      return;
    }
    if (hoveredNode != null && hoveredNode != node){
      hoveredNode.Deselect();
    }
    hoveredNode = node;

    bool canBuild = InBuildingMode() &&  CanBuildTower(node);
    SetGhostStatus(canBuild);
  }

  public void StopHovering(Node node){
    if (InTargetingMode()){
      return;
    }
    if (hoveredNode == node){
      hoveredNode = null;
    }
  }

  public void PathClicked(ITile path){
    if (tileTargeting && targetablePaths.Contains(path)){
      hoveredNode.tower = BuildSelectedTower(hoveredNode);
      hoveredNode.Deselect();
    }
  }

  public void UpdateTowerList(){
    towerList = DataManager.instance.runData.GetTowerBlueprints();
  }

  public int GetTowerCost(TowerBlueprint blueprint, int tier){
    int count = GetTowerCount(blueprint);
    return blueprint.GetCost(tier, count);
  }

  public void SetGhostStatus(bool canBuild){
    towerGhost.SetMaterial(canBuild);
  }

  /*========================
   * Private functions
   *========================*/

  private Tower BuildTower(Node node, TowerBlueprint blueprint, int level, ITile path = null){
    Tower tower = CreateTower(node, blueprint, level, path);
    CreateBuildEffect(node);
    HandleBuildingCost(blueprint, level);
    if (level == 0){
      ModifyTowerCount(blueprint, 1);
    }

    if (cooldownManager.IsCooldownTower(blueprint)){
      int towerCount = GetTowerCount(blueprint);
      cooldownManager.ResetTowerCooldown(towerCount);
    }

    int towerCost = GetTowerCost(blueprint, 0);
    uiManager.UpdateTowerPrice(blueprint, towerCost);
    return tower;
  }

  private Tower CreateTower(Node node, TowerBlueprint blueprint, int level, ITile path){
    Tower tower = node.tower;
    if (tower == null){
      tower = Instantiate(blueprint.towerPrefab, node.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity, transform);
      tower.SetPath(path);
    }
    tower.Init(blueprint, level);
    return tower;
  }

  private void CreateBuildEffect(Node node){
    GameObject effect = Instantiate(buildEffect, node.transform.position, Quaternion.identity, transform);
    Destroy(effect, 2.5f);
  }

  private void HandleBuildingCost(TowerBlueprint blueprint, int level){
    if (cooldownManager.IsCooldownTower(blueprint)){
      return;
    }
    int towerCost = blueprint.GetCost(level, GetTowerCount(blueprint));
    gameManager.ModifyResources(-towerCost);
  }

  private bool HasResourcesForTower(bool showError){
    int towerCount = GetTowerCount(selectedBlueprint);

    if (cooldownManager.IsCooldownTower(selectedBlueprint)){
      return cooldownManager.TowerIsReady();
    }

    if (selectedBlueprint.GetCost(0, towerCount) > gameManager.GetResources()){
      if (showError){
        gameManager.ShowResourceError();
      } 
      return false;
    }
    return true;
  }

  private TowerBlueprint GetTowerToBuild(){
    return selectedBlueprint;
  }

  private void TileTargeting(){
    ITile path = GameManager.instance.pathManager.GetSelectedPath();
    if (path != null){
      if (targetablePaths.Contains(path)){
        currentPath = path;
        path.Targeting();
      }
    }
  }

  private void ModifyTowerCount(TowerBlueprint blueprint, int change){
    if (dictionary.ContainsKey(blueprint)){
      dictionary[blueprint] += change;
    } else {
      dictionary[blueprint] = change;
    }
  }

  public int GetTowerCount(TowerBlueprint blueprint){
    if (dictionary.ContainsKey(blueprint)){
      return dictionary[blueprint];
    }
    return 0;
  }

  public bool HasBeenBuilt(TowerBlueprint blueprint){
    return dictionary.ContainsKey(blueprint);
  }

  private void StartTileTargeting(Node node){
    tileTargeting = true;
    Vector2Int pos = node.position;
    targetablePaths = GameManager.instance.pathManager.GetAdjacentPaths(pos);
  }

  private void StopTileTargeting(){
    tileTargeting = false;
    targetablePaths = null;
  }

}
