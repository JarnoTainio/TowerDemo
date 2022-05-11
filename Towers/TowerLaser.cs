using System.Collections.Generic;
using UnityEngine;

public class TowerLaser : TowerShooting
{
  public LineRenderer lineRenderer;
  private EnemyEffectType effectType;
  private float effectIntensitiy;
  private float effectDuration;

  public override void Init(TowerBlueprint blueprint, int level)
  {
    base.Init(blueprint, level);
    effectType = blueprint.GetEffectType(level);
    effectIntensitiy = blueprint.GetEffectIntensity(level);
    effectDuration = blueprint.GetEffectDuration(level);
  }

  protected override void Tick(float deltaTime)
  {
    UpdateTargeting(deltaTime);

    if (target){
      if (!RotateToTarget(target.transform.position, deltaTime)){
        DisableLine();
        return;
      }
      if (!targetInRange){
        DisableLine();
        return;
      }

      UpdateLine();
      Activate();
    } else {
        DisableLine();
    }
  }

  private void DisableLine(){
    if (lineRenderer.enabled)
    {
      lineRenderer.enabled = false;
      //impactEffect.Stop();
      //impactLight.enabled = false;
    }
  }

  protected override void Fire()
  {
    DamageTarget(DamageType.damageOverTime);
    ApplyEffect();
  }

  protected void DamageTarget(DamageType damageType){
    float dmg = damage;
    if (damageType == DamageType.damageOverTime){
      dmg *= Time.deltaTime;
    } 
    target.Damage(dmg, damageType);
  }

  protected void ApplyEffect(){
    if (effectType == EnemyEffectType.none){
      return;
    }
    EnemyEffect effect = new EnemyEffect(this, effectType, effectIntensitiy, effectDuration);
    target.ApplyEffect(effect);
  }

  protected void UpdateLine(){
    if (!lineRenderer.enabled)
    {
      lineRenderer.enabled = true;
      //impactEffect.Play();
      //impactLight.enabled = true;
    }

    Vector3 startPoint = shootingPoint.transform.position;
    Vector3 endPoint = target.transform.position;
    lineRenderer.SetPosition(0, startPoint);
    lineRenderer.SetPosition(1, endPoint);

    //Vector3 dir = startPoint - endPoint;
    //impactEffect.transform.position = endPoint + dir.normalized;
    //impactEffect.transform.rotation = Quaternion.LookRotation(dir);
  }

  public override List<UpgradeStatus> GetDescriptionValues(){
    List<UpgradeStatus> list = base.GetDescriptionValues();
    if (blueprint.GetEffectType(level) != EnemyEffectType.none){
      string key = EnemyEffect.GetTranslationKey(blueprint.GetEffectType(level), true);
      list.Add(new UpgradeStatus(key, blueprint.GetEffectIntensity(level), blueprint.GetEffectIntensity(level + 1)));
    }
    if (blueprint.GetEffectDuration(level) > 0.5f){
      list.Add(new UpgradeStatus("info_duration", blueprint.GetEffectDuration(level), blueprint.GetEffectDuration(level + 1)));
    }
    return list;
  }
}
