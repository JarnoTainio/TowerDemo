using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyEffect
{
  public EnemyEffectType effectType;
  public float intensity;
  public float duration;
  public Tower source;

  public EnemyEffect(Tower source, EnemyEffectType effectType, float intensity, float duration){
    this.source = source;
    this.effectType = effectType;
    this.intensity = intensity;
    this.duration = duration;
  }

  public void ApplyEffect(Enemy enemy){
    switch(effectType){
      case EnemyEffectType.slow:
        enemy.SetSlow(intensity / 100f);
        return;
      case EnemyEffectType.burn:
        enemy.burn = intensity;
        return;
      case EnemyEffectType.value:
        return;
    }
    throw new System.Exception(effectType.ToString());
  }

  public void RemoveEffect(Enemy enemy){
    switch(effectType){
      case EnemyEffectType.slow:
        enemy.SetSlow(0);
        return;
      case EnemyEffectType.burn:
        enemy.burn = 0;
        return;
      case EnemyEffectType.value:
        return;
    }
    throw new System.Exception(effectType.ToString());
  }

  public bool Tick(Enemy enemy, float deltaTime){
    duration -= deltaTime;
    return duration <= 0;
  }

  /*========================
   * Static functions
   *========================*/

  public static string GetTranslationKey(EnemyEffectType type, bool singleTarget){
    if (type == EnemyEffectType.slow){
      return singleTarget ? "info_slow" : "info_auraSlow";
    }
    if (type == EnemyEffectType.burn){
      return singleTarget ? "info_burn" : "info_auraBurn";
    }
    if (type == EnemyEffectType.teleport){
      return "info_teleport";
    }
    if (type == EnemyEffectType.value){
      return "info_auraValue";
    }
    if (type == EnemyEffectType.damage){
      return "info_auraDamage";
    }
    return "";
  }

}
