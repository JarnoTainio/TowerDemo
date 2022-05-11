using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
  public static DataManager instance;
  [Header("Options")]
  public bool useDefaultData = false;

  [Header("Data")]
  public SettingsData settings;
  public RunData runData;
  
  [Header("LevelData")]
  public LevelData levelData;
  public TowerBlueprintList towerList;
  public RuneList runeList;

  void Awake(){
    if (instance != null){
      Destroy(gameObject);
      return;
    }
    
    if (useDefaultData){
      settings = new SettingsData();
      runData = new RunData(new string[]{towerList.list[0].nameKey});
    }
    
    DontDestroyOnLoad(gameObject);
    instance = this;
  }

  // ================================
  // Functions
  // ================================

  public LevelData GetLevelData(){ return levelData; }

  // ================================
  // Scene Management
  // ================================

  public void StartLevel(LevelData levelData){
    this.levelData = levelData;
    Time.timeScale = 1f;
    LoadScene(SceneName.combatScene);
  }

  public void LoadMapScene(){
    LoadScene(SceneName.mapScene);
  }

  public void ExitApplication(){
    Application.Quit();
  }

  // ================================
  // Private functions
  // ================================

  private void LoadScene(SceneName sceneName){
    StartCoroutine(LoadSceneIEnumerator(sceneName));
  }
  private IEnumerator LoadSceneIEnumerator(SceneName sceneNameEnum){
    string sceneName = sceneNameEnum.ToString();
    SceneManager.LoadScene(sceneName);
    yield return 0;
    SceneManager.SetActiveScene( SceneManager.GetSceneByName(sceneName) );
  }
}

enum SceneName {mapScene, combatScene}