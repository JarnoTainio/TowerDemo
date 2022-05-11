using UnityEngine;

public class CooldownManager : MonoBehaviour
{
  [Header("References")]
  public BuildManager buildManager;
  public LevelUIManager levelUIManager;

  [Header("TowerBuilding")]
  public TowerBlueprint towerBlueprint;
  public int towerRequiredTime;
  public float towerTimer;
  public bool blueprintSelected;

  void Start(){
    blueprintSelected = false;
  }

  void Update(){
    if (towerBlueprint == null){
      return;
    }

    if (WaveManager.levelStarted){
      towerTimer += Time.deltaTime;
      levelUIManager.towerSelect.UpdateTowerCooldown(towerTimer / towerRequiredTime);
      if (blueprintSelected){
        buildManager.SetGhostStatus(towerTimer >= towerRequiredTime);
      }
    }
  }

  public void BlueprintSelected(TowerBlueprint blueprint){
    blueprintSelected = towerBlueprint == blueprint;
  }

  public void InitTower(TowerBlueprint blueprint){
    if (blueprint == towerBlueprint){
      return;
    }
    towerBlueprint = blueprint;
    towerRequiredTime = blueprint.GetBuildingCooldown();
    towerTimer = 0f;
    levelUIManager.towerSelect.InitTowerCooldown(blueprint);
  }

  public bool IsCooldownTower(TowerBlueprint blueprint){
    return towerBlueprint == blueprint;
  }

  public bool TowerIsReady(){
    return towerTimer >= towerRequiredTime;
  }

  public void ResetTowerCooldown(int towerCount){
    towerRequiredTime = towerBlueprint.GetBuildingCooldown(towerCount);
    towerTimer = 0f;
  }
}
