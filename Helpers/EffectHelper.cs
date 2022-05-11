using System.Collections.Generic;
using UnityEngine;
public class EffectHelper
{
  public static bool AddEffect(Enemy enemy, EnemyEffect effect){
    if (AddAsActiveEffect(enemy, effect)){
      return true;
    }

    AddAsWaitingEffect(enemy, effect);
    return false;
  }

  private static bool AddAsActiveEffect(Enemy enemy, EnemyEffect effect){
    List<EnemyEffect> activeEffects = enemy.activeEffects;
    
    // Note: activeEffects only have one of each type
    EnemyEffect current = activeEffects.Find(e => e.effectType == effect.effectType);

    // No effect of this type
    if (current == null){
      activeEffects.Add(effect);
      effect.ApplyEffect(enemy);
      return true;
    }

    // Current is weaker
    if (current.intensity < effect.intensity){
      EffectHelper.ReplaceEffect(effect, current, enemy);
      return true;
    }

    if (current.intensity == effect.intensity && current.duration < effect.duration){
      current.duration = effect.duration;
      return  true;
    }

    return false;
  }

  private static void AddAsWaitingEffect(Enemy enemy, EnemyEffect effect){
    List<EnemyEffect> waitingEffects = enemy.waitingEffects
    ; 
    // Empty list
    if (waitingEffects.Count == 0){
      waitingEffects.Add(effect);
      return;
    }

    // Find waiting weaker effects
    List<EnemyEffect> weakerEffects = waitingEffects.FindAll(e => {
      if (e.intensity < effect.intensity && e.duration <= effect.duration){
        return true;
      }
      if (e.intensity <= effect.intensity && e.duration < effect.duration){
        return true;
      }
      return false;
    });

    // No weaker effects found
    if (weakerEffects.Count == 0){
      return;
    }

    // Remove weaker effects and add new effect
    foreach(EnemyEffect e in weakerEffects){
      waitingEffects.Remove(e);
    }
    waitingEffects.Add(effect);
  }

  public static void CurrentExpired(Enemy enemy, EnemyEffect expired){
    enemy.activeEffects.Remove(expired);
    expired.RemoveEffect(enemy);
    
    List<EnemyEffect> effects = enemy.waitingEffects.FindAll(e => e.effectType == expired.effectType);
    if (effects.Count == 0){
      return;
    }

    // Find best
    EnemyEffect best = null;
    foreach(EnemyEffect effect in effects){
      // Note: waitingList doesn't have useless effects
      if (best == null || effect.intensity > best.intensity){
        best = effect;
      }
    }

    if (best == null){
      return;
    }

    
    enemy.activeEffects.Add(best);
    enemy.waitingEffects.Remove(best);
    best.ApplyEffect(enemy);
  }

  private static void ReplaceEffect(EnemyEffect newEffect, EnemyEffect oldEffect, Enemy enemy){
    List<EnemyEffect> activeEffects = enemy.activeEffects;
    List<EnemyEffect> waitingEffects = enemy.waitingEffects;

    activeEffects.Remove(oldEffect);
    AddAsWaitingEffect(enemy, oldEffect);
    activeEffects.Add(newEffect);

    oldEffect.RemoveEffect(enemy);
    newEffect.ApplyEffect(enemy);
  }

}
