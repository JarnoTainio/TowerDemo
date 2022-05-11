using System.Collections.Generic;
using UnityEngine;

public abstract class TowerShooting : Tower
{
  public GameObject partToRotate;
  public GameObject shootingPoint;
  protected int damage;
  protected float rotateSpeed;
  protected float range;
  protected Enemy target;
  protected bool targetInRange;
  protected float targetingCounter = 0f;
  protected bool readyToActivate;

  // ==========================
  // Unity functions
  // ==========================
  protected override void DrawAdditionalGizmos(){
    base.DrawAdditionalGizmos();
    if (target != null){
      Gizmos.color = Color.red;
      Gizmos.DrawLine(shootingPoint.transform.position, target.transform.position);
    }
  }

  // ==========================
  // Public functions
  // ==========================

  public override void Init(TowerBlueprint blueprint, int level)
  {
    base.Init(blueprint, level);
    rotateSpeed = blueprint.GetRotateSpeed(level);
    range = blueprint.GetRange(level);
    damage = blueprint.GetDamage(level);
  }

  protected override void Activate()
  {
    Fire();
  }

  protected abstract void Fire();

  protected override void Tick(float deltaTime)
  {
    TargetingTick(deltaTime);
  }

  protected void TargetingTick(float deltaTime){
    UpdateTargeting(deltaTime);
    if (!readyToActivate){
      activationCounter -= deltaTime;
      if (activationCounter <= 0){
        readyToActivate = true;
        ReadyToActivate();
      }
    }else{
      IdlingTick(deltaTime);
    }

    if (target){
      if (!RotateToTarget(target.transform.position, deltaTime)){
        return;
      }
      if (!targetInRange){
        return;
      }

      if (activationCounter <= 0){
        Activate();
        readyToActivate = false;
        activationCounter = cooldown;
      }
    }
  }

  protected virtual void IdlingTick(float deltatime){}
  protected virtual void ReadyToActivate(){}

  protected bool RotateToTarget(Vector3 target, float deltaTime){
    Vector3 dir = target - transform.position;
    Quaternion lookRotation = Quaternion.LookRotation(dir);
    Vector3 rotation = Quaternion.Lerp(partToRotate.transform.rotation, lookRotation, deltaTime * rotateSpeed).eulerAngles;
    partToRotate.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    
    float dotProd = VectorHelper.GetFacingLevel(partToRotate.transform.position, partToRotate.transform.forward, target);
    return dotProd > 0.90f;
  }

  protected void UpdateTargeting(float deltaTime){
    targetingCounter -= deltaTime;
    if (targetingCounter <= 0){
      target = FindTarget();
      targetingCounter = 0.4f;
    }
  }

  private Enemy FindTarget(){
    Enemy[] enemies = EnemyManager.GetEnemies();
    float bestScore = -Mathf.Infinity;
    Enemy bestTarget = null;
    float targetingRange = range * 1.25f;
    foreach(Enemy enemy in enemies){
      float score = GetTargetingScore(transform.position, enemy);
      float distance = Vector3.Distance(transform.position, enemy.transform.position);
      if (distance > targetingRange){
        continue;
      }
      if (distance > range){
        score -= 10000;
      }
      if (score > bestScore) {
        bestScore = score;
        bestTarget = enemy;
        targetInRange = distance <= range;
      }
    }
    return bestTarget;
  }

  protected float GetTargetingScore(Vector3 startPoint, Enemy enemy){
    if (targetingPriority == TargetingPriority.closest){
      return -Vector3.Distance(transform.position, enemy.transform.position);
    }
    if (targetingPriority == TargetingPriority.furthest){
      return enemy.GetTraveledDistance();
    }
    if (targetingPriority == TargetingPriority.life){
      return enemy.GetLife();
    }
    if (targetingPriority == TargetingPriority.lowLife){
      return -enemy.GetLife();
    }
    if (targetingPriority == TargetingPriority.armor){
      return enemy.GetArmor();
    }
    return 0;
  }

  // ==========================
  // Public
  // ==========================

  public override float GetRange(){ return range; }
  
  public override void SetFacing(Quaternion quaternion){ 
    partToRotate.transform.rotation = quaternion; 
  }

  public override Quaternion GetFacing(){
    return partToRotate.transform.rotation; 
  }

  public override bool UseTargetingPriority(){return true; }

  public override List<UpgradeStatus> GetDescriptionValues(){
    List<UpgradeStatus> list = base.GetDescriptionValues();

    int currentDamage = (int)(blueprint.GetDamage(level));
    int nextDamage = (int)(blueprint.GetDamage(level + 1));
    if (blueprint.GetCooldown(level) > 0f){
      list.Add(new UpgradeStatus("info_damage", currentDamage, nextDamage));
    }else{
      list.Add(new UpgradeStatus("info_damageOverTime", currentDamage, nextDamage));
    }
    list.Add(new UpgradeStatus("info_range", blueprint.GetRange(level), blueprint.GetRange(level + 1)));
    return list;
  }
}
