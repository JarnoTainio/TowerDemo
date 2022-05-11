using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHostileAura : Tower
{
  private float range;
  private EnemyEffectType effectType;
  private float effectIntensitiy;
  private float effectDuration;
  public GameObject visualEffectPrefab;
  private GameObject visualEffect;
  
  public override void Init(TowerBlueprint blueprint, int level)
  {
    base.Init(blueprint, level);
    range = blueprint.GetRange(level);
    effectType = blueprint.GetEffectType(level);
    effectIntensitiy = blueprint.GetEffectIntensity(level);
    effectDuration = blueprint.GetEffectDuration(level); 

    if (visualEffect == null){
      visualEffect = Instantiate(visualEffectPrefab, transform.position + new Vector3(0,1,0), Quaternion.identity, transform);
      visualEffect.SetActive(false);
    }
  }

   protected override void Tick(float deltaTime)
  {
    if (activationCounter > 0){
      activationCounter -= deltaTime;
    }

    if (EnemyInRange()){
      if (activationCounter <= 0){
        Activate();
        activationCounter = cooldown;
      }

      EnableVisualEffect();
    } else {
      DisableVisualEffect();
    }
  }

  protected override void Activate()
  {
    Enemy[] enemies = GetEnemiesInRange();
    ApplyEffect(enemies);
  }

  private bool EnemyInRange(){
    Enemy[] enemies = EnemyManager.GetEnemies();
    foreach(Enemy enemy in enemies){
      float distance = Vector3.Distance(transform.position, enemy.transform.position);
      if (distance <= range){
        return true;
      }
    }
    return false;
  }

  private Enemy[] GetEnemiesInRange(){
    Enemy[] enemies = EnemyManager.GetEnemies();
    List<Enemy> enemiesInRange = new List<Enemy>();
    foreach(Enemy enemy in enemies){
      float distance = Vector3.Distance(transform.position, enemy.transform.position);
      if (distance <= range){
        enemiesInRange.Add(enemy);
      }
    }
    return enemiesInRange.ToArray();
  }

  private void ApplyEffect(Enemy[] enemies){
    foreach(Enemy enemy in enemies){
      enemy.ApplyEffect(CreateEffect());
    }
  }

  private EnemyEffect CreateEffect(){
    return new EnemyEffect(this, effectType, effectIntensitiy, effectDuration);
  }

  private void EnableVisualEffect(){
    visualEffect.SetActive(true);
  }

  private void DisableVisualEffect(){
    visualEffect.SetActive(false);
  }

  public override float GetRange(){ return range;}

  public override void Disable()
  {
    base.Disable();
    visualEffectPrefab.gameObject.SetActive(false);
  }

  public override List<UpgradeStatus> GetDescriptionValues(){
    List<UpgradeStatus> list = base.GetDescriptionValues();
    string key = EnemyEffect.GetTranslationKey(blueprint.GetEffectType(level), false);
    list.Add(new UpgradeStatus(key, blueprint.GetEffectIntensity(level), blueprint.GetEffectIntensity(level + 1)));
    list.Add(new UpgradeStatus("info_range", blueprint.GetRange(level), blueprint.GetRange(level + 1)));
    return list;
  }
}
