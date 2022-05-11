using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  [Header("Level")]
  public LevelData levelData;
  private int resources;
  private int lives;

  [Header("References")]
  public LevelInputManager inputManager;
  public PathManager pathManager;
  public BuildManager buildManager;
  
  [Header("UI")]
  public LevelUIManager levelUIManager;
  public GameOverUI gameOverUI;
  public TowerUI towerUI;
  public InventoryManager inventoryManager;

  [HideInInspector]
  public static GameManager instance;

  // Delegates
  public delegate void ResourceModified(int currentResources);
  public ResourceModified resourceModified;

  // ================================
  // Unity
  // ================================
  void Awake ()
  {
    if (instance != null)
    {
      Destroy(gameObject);
      return;
    }
    instance = this;
    AfterAwake();
  }

  private void AfterAwake(){
    levelData = DataManager.instance.GetLevelData();

    resourceModified = levelUIManager.UpdateResources;
    WaveManager.hasWavesLeft = true;
    WaveManager.levelStarted = false;
  }

  void Start(){
    lives = 0;
    ModifyLives(levelData.lives);

    GameManager.instance.resourceModified += towerUI.ResourcesModified;
    resources = 0;
    ModifyResources(levelData.resources);

    int size = 30;
    Camera.main.transform.position = new Vector3(size /2, size / 2, size /3);

    ShowRewardDialog();
    InventoryUpdated();
  }

  // ================================
  // Public functions
  // ================================

  public void ShowRewardDialog(){
    RunData runData = DataManager.instance.runData;
    if (DataManager.instance.runData.level % 2 == 0 && runData.HasRoomForTower()){
      levelUIManager.towerRewardDialog.Open();
    } else if (runData.HasRoomForRune()) {
      levelUIManager.runeRewardDialog.Open();
    } else {
      levelUIManager.towerRewardDialog.Close();
    }
  }

  public void ModifyResources(GameObject source, int amount){
    levelUIManager.CreateResourceGainedText(source, amount);
    ModifyResources(amount);
  }

  public void ModifyResources(int amount){
    resources += amount;

    if (resourceModified != null){
      resourceModified(resources);
    }
  }

  public void ShowResourceError(){
    levelUIManager.ShowResourceError();
  }

  public int GetResources(){ return resources; }
  public Wave[] GetWaveList(){ return levelData.waves.ToArray(); }

  public void NoEnemiesLeft(){
    if (!WaveManager.hasWavesLeft){
      GameOver(lives > 0);
    }
  }

  public void ModifyLives(int amount){
    lives += amount;
    if (lives <= 0){
      lives = 0;
      GameOver(false);
      
    }
    levelUIManager.UpdateLives(lives);
  }

  public void HideTowerInfo(){
    towerUI.Close();
  }

  public void ShowTowerInfo(Node node){
    towerUI.Open(node);
  }

  public void ToggleTowerInfo(Node node){
    towerUI.Toggle(node);
  }

  public void RouteChanged(Route newDefaultRoute, bool oldDefaultIsValid){
    foreach(Enemy enemy in EnemyManager.GetEnemies()){
      enemy.DefaultRouteChanged(newDefaultRoute, oldDefaultIsValid);
    }
  }

  public Route GetRoute(Vector2Int pos){
    return pathManager.GetRoute(pos);
  }

  public void SetGameSpeed(float f){
    Time.timeScale = f;
  }

  public void UpdateTowerOptions(){
    buildManager.UpdateTowerList();
    levelUIManager.towerSelect.LoadTowerButtons();
  }

  public Tower GetGhostTower(){
    return buildManager.towerGhost.ghost;
  }

  public void InventoryUpdated(){
    // Cooldown tower
    TowerBlueprint[] blueprints = DataManager.instance.runData.GetTowerBlueprints();
    foreach(TowerBlueprint blueprint in blueprints){
      if (blueprint.GetBuildingCooldown() > 0){
        buildManager.cooldownManager.InitTower(blueprint);
        break;
      }
    }
  }

  // ================================
  // Scene management
  // ================================

  public void NextLevel(){
    DataManager.instance.runData.NextLevel();
    levelUIManager.fadeIn.StartFadeOut((done) => DataManager.instance.StartLevel(levelData));
  }

  public void RestartLevel(){
    levelUIManager.fadeIn.StartFadeOut((done) => DataManager.instance.StartLevel(levelData));
  }

  public void NewGame(){
    DataManager.instance.runData.NewRun();
    levelUIManager.fadeIn.StartFadeOut((done) => DataManager.instance.StartLevel(levelData));
  }

  public void QuitLevel(){
    levelUIManager.fadeIn.StartFadeOut((done) => DataManager.instance.LoadMapScene());
  }

  public void ExitApplication(){
    levelUIManager.fadeIn.StartFadeOut((done) => DataManager.instance.ExitApplication());
  }

  // ================================
  // Private functions
  // ================================

  private void GameOver(bool victory){
    SetGameSpeed(0f);
    gameOverUI.Open(victory);
    inputManager.enabled = false;
  }

}
