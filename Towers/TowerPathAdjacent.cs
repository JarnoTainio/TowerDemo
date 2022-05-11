using System.Collections.Generic;
using UnityEngine;

public class TowerPathAdjacent : Tower
{
  public GameObject activationEffect;
  public GameObject isReadyEffect;
  private ITile path;
  private bool isReady;

  public override void Init(TowerBlueprint blueprint, int level)
  {
    base.Init(blueprint, level);
    isReady = activationCounter == 0;
    SetReadyEffect(isReady);
  }
  protected override void Tick(float deltaTime){
    if (!isReady){
      TickCounter(deltaTime);
    }
    else{
      Activate();
    }
  }

  private void TickCounter(float deltaTime){
    activationCounter -= deltaTime;
      if (activationCounter <= 0){
        SetReadyEffect(true);
      }
  }

  protected override void Activate(){
    Enemy enemy = FindTarget();
    if (enemy != null){
      activationCounter = cooldown;
      TeleportEnemy(enemy);
      SetReadyEffect(false);
    }
  }

  private void SetReadyEffect(bool _isReady){
    isReadyEffect.SetActive(_isReady);
    isReady = _isReady;
  }

  private void TeleportEnemy(Enemy enemy){
    if (activationEffect != null){
      GameObject effect = Instantiate(activationEffect, enemy.transform.position, Quaternion.identity, transform);
      Destroy(effect, 2f);
    }
    enemy.transform.position = GameManager.instance.pathManager.GetDefaultRoute().GetWorlPoint(0) + new Vector3(0, 0.5f, 0);
    enemy.pathTraveler.FindRoute();
  }

  public override void SetPath(ITile path)
  {
    this.path = path;
    Vector3 dir = path.GetTransform().position - transform.position;
    Quaternion lookRotation = Quaternion.LookRotation(dir);
    Vector3 rotation = lookRotation.eulerAngles;
    transform.rotation = Quaternion.Euler(0f, rotation.y + 90, 0f);
  }

  public override ITile GetPath(){ return path; }

  private Enemy FindTarget(){
    Enemy[] enemies = EnemyManager.GetEnemies();
    float bestScore = Mathf.Infinity;
    Enemy bestTarget = null;
    float range = 0.4f;

    foreach(Enemy enemy in enemies){
      float score = Vector3.Distance(path.GetTransform().position, enemy.transform.position);
      if (score > range){
        continue;
      }
      if (score < bestScore) {
        bestScore = score;
        bestTarget = enemy;
      }
    }
    return bestTarget;
  }

  public override void Disable()
  {
    base.Disable();
    isReadyEffect.SetActive(false);
  }

  public override List<UpgradeStatus> GetDescriptionValues(){
    List<UpgradeStatus> list = base.GetDescriptionValues();
    list.Add(new UpgradeStatus("info_teleport"));
    return list;
  }
}
