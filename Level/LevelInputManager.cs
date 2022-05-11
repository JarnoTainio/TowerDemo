using UnityEngine;

public class LevelInputManager : MonoBehaviour
{
  public BuildManager buildManager;
  public TimeManager timeManager;
  public TowerUI towerUI;
  public TowerRange towerRange;
  public bool debug;

  public bool movementEnabled;
  public bool selectionEnabled;
  public bool menuEnabled;

  void Update(){
    Mouse();
    NumberKeys();
    Debug();
  }

  private void Mouse(){
    if (!movementEnabled){
      return;
    }
  if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)){
      buildManager.ExitBuildMode();
      GameManager.instance.HideTowerInfo();
    }
  }

  private void NumberKeys(){
    if (!selectionEnabled){
      return;
    }
    if      (Input.GetKeyDown(KeyCode.Alpha1)){ buildManager.SelectTowerToBuild(0); }
    else if (Input.GetKeyDown(KeyCode.Alpha2)){ buildManager.SelectTowerToBuild(1); }
    else if (Input.GetKeyDown(KeyCode.Alpha3)){ buildManager.SelectTowerToBuild(2); }
    else if (Input.GetKeyDown(KeyCode.Alpha4)){ buildManager.SelectTowerToBuild(3); }
    else if (Input.GetKeyDown(KeyCode.Alpha5)){ buildManager.SelectTowerToBuild(4); }
    else if (Input.GetKeyDown(KeyCode.Alpha6)){ buildManager.SelectTowerToBuild(5); }
    else if (Input.GetKeyDown(KeyCode.Alpha7)){ buildManager.SelectTowerToBuild(6); }
    else if (Input.GetKeyDown(KeyCode.Alpha8)){ buildManager.SelectTowerToBuild(7); }
    else if (Input.GetKeyDown(KeyCode.Alpha9)){ buildManager.SelectTowerToBuild(8); }

    if      (Input.GetKeyDown(KeyCode.F1)){ timeManager.OptionSelected(0); }
    else if (Input.GetKeyDown(KeyCode.F2)){ timeManager.OptionSelected(1); }
    else if (Input.GetKeyDown(KeyCode.F3)){ timeManager.OptionSelected(2); }
    else if (Input.GetKeyDown(KeyCode.F4)){ timeManager.OptionSelected(3); }
    else if (Input.GetKeyDown(KeyCode.F5)){ timeManager.OptionSelected(4); }
  }

  private void Debug(){
    if (!debug){
      return;
    }
    if (Input.GetKey(KeyCode.M)){
      GameManager.instance.ModifyResources(5);
    }
    if (Input.GetKeyDown(KeyCode.X)){
      GameManager.instance.ModifyLives(-1);
    }
  }

  public void SetControlsEnabled(bool enabled){
    movementEnabled = enabled;
    selectionEnabled = enabled;
    LevelCameraController.movementEnabled = enabled;
  }
}
