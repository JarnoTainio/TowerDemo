using System.Collections.Generic;
using UnityEngine;

public class TowerLaserChain : TowerLaser
{
  public EffectIntensity chargeEffect;
  private int maxCharges;
  private float charges;
  private float chargeTimer;
  private float lineDuration;

  public override void Init(TowerBlueprint blueprint, int level)
  {
    base.Init(blueprint, level);
    maxCharges = blueprint.GetCharges(level);
  }

  protected override void Tick(float deltaTime)
  {
    TickLineDuration(deltaTime);
    UpdateTargeting(deltaTime);
    TargetingTick(deltaTime);
  }

  protected override void IdlingTick(float deltaTime){
    //Charge(deltaTime);
  }

  protected override void ReadyToActivate(){
    UpdateChargeEffect();
  }

  private void TickLineDuration(float deltaTime){
    if (lineRenderer.enabled){
      lineDuration -= deltaTime;
      if (lineDuration <= 0){
        lineRenderer.enabled = false;
      }
    }
  }

  private void Charge(float deltaTime){
    if (charges >= maxCharges){
      return;
    }
    chargeTimer += deltaTime;
    if (chargeTimer >= cooldown){
      chargeTimer -= cooldown;
      charges++;
      UpdateChargeEffect();

      if (charges == maxCharges){
        chargeTimer = 0;
      }
    }
  }

  protected override void Fire()
  {
    chargeTimer = 0;
    Enemy[] targets = FindTargets();
    foreach(Enemy enemy in targets){
      target = enemy;
      DamageTarget(DamageType.normal);
    }
    UpdateLines(targets);
    UpdateChargeEffect();
  }

  private Enemy[] FindTargets(){
    List<Enemy> enemies = new List<Enemy>(EnemyManager.GetEnemies());
    enemies.Remove(target);
    charges = maxCharges;

    List<Enemy> targets = new List<Enemy>();
    targets.Add(target);

    Vector3 startingPoint = target.transform.position;
    while (charges > 0){
      Enemy bestTarget = GetClosesEnemy(enemies, startingPoint);

      if (bestTarget != null){
        charges--;
        targets.Add(bestTarget);
        enemies.Remove(bestTarget);
        startingPoint = bestTarget.transform.position;
      }else{
        break;
      }
    }
    return targets.ToArray();
  }

  private Enemy GetClosesEnemy(List<Enemy> enemies, Vector3 startingPoint){
    float jumpRange = range;
    float shortestDistance = Mathf.Infinity;
    Enemy bestTarget = null;

    foreach(Enemy enemy in enemies){
      float distance = Vector3.Distance(startingPoint, enemy.transform.position);
      if (distance > jumpRange){
        continue;
      }
      if (distance < shortestDistance) {
        shortestDistance = distance;
        bestTarget = enemy;
      }
    }
    return bestTarget;
  }

  private void UpdateChargeEffect(){
    //chargeEffect.gameObject.SetActive(charges > 0);
    //chargeEffect.SetIntensity(readyToActivate ? charges + 1 : 0);
  }

  protected void UpdateLines(Enemy[] enemies){
    if (!lineRenderer.enabled)
    {
      lineRenderer.enabled = true;
    }

    lineRenderer.positionCount = enemies.Length + 1;
    Vector3[] points = new Vector3[lineRenderer.positionCount];

    points[0] = shootingPoint.transform.position;
    for(int i = 0; i < enemies.Length; i++){
      points[i + 1] = enemies[i].transform.position;
    }

    lineRenderer.SetPositions(points);
    lineDuration = 0.2f;
  }

  public override List<UpgradeStatus> GetDescriptionValues(){
    List<UpgradeStatus> list = base.GetDescriptionValues();
    float radius = blueprint.GetEffectRadius(level);
    list.Add(new UpgradeStatus("info_chain", blueprint.GetCharges(level), blueprint.GetCharges(level + 1)));
    return list;
  }
}
