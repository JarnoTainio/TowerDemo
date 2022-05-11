using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public static int ID;
  public int id;
  
  [Header("Unity")]
  public PathTraveler pathTraveler;
  public LifeBar lifeBar;
  public MeshRenderer meshRenderer;
  public MeshFilter meshFilter;
  private EnemyManager enemyManager;

  [Header("Effects")]
  public EffectIntensity burnEffectPrefab;
  private EffectIntensity burnEffect;
  public EffectIntensity slowEffectPrefab;
  private EffectIntensity slowEffect;
  public EffectIntensity valueEffectPrefab;
  private EffectIntensity valueEffect;
  
  // Attributes
  [HideInInspector]
  public EnemyBlueprint blueprint;
  [HideInInspector]

  private int maxLife;
  private float life;
  private int armor;
  public float speed;
  private int value;
  private int danger;
  public int level;
  [HideInInspector]
  public float burn;

  // Other
  [HideInInspector]
  public List<EnemyEffect> activeEffects;
  [HideInInspector]
  public List<EnemyEffect> waitingEffects;

  [HideInInspector]
  public delegate void LifeModified();
  [HideInInspector]
  public LifeModified lifeModified;
  
  private float dotDamage;
  private bool showingDotDamage;
  public float dotMessageFrequency = .5f;

  // ==========================
  // Unity
  // ==========================
  public void Awake(){
    activeEffects = new List<EnemyEffect>();
    waitingEffects = new List<EnemyEffect>();
    id = ID++;
  }

  public void Update(){
    float deltaTime = Time.deltaTime;
    pathTraveler.Travel(deltaTime);
    if (pathTraveler.endReached){
      GameManager.instance.ModifyLives(-danger);
      Die(false);
    }

    TickEffects(deltaTime);
  }

  void OnMouseDown ()
  {
    // Over UI
    if (EventSystem.current.IsPointerOverGameObject()){
      return;
    }
    enemyManager.EnemyClicked(this);
  }



  // ==========================
  // Public
  // ==========================

  public void Init(EnemyManager enemyManager, EnemyBlueprint blueprint, int tier){
    this.enemyManager = enemyManager;
    this.blueprint = blueprint;
    this.level = tier;
    life = maxLife = blueprint.GetLife(tier);
    armor = blueprint.GetArmor(tier);
    speed = blueprint.GetSpeed(tier);
    pathTraveler.SetSpeed(speed);
    value = blueprint.GetValue(tier);
    danger = blueprint.GetDanger(tier);
    lifeBar.SetScale(1);

    meshFilter.mesh = blueprint.mesh;
    meshRenderer.material = blueprint.GetMaterial(tier);
    transform.localScale = blueprint.GetScale(tier);
  }

  public void Damage(float damage, DamageType damageType){
    if (armor > 0){
      damage = ApplyArmor(damage, damageType);
    }
    life -= damage;
    CreateDamageText(damageType, damage);

    if (life < 0){ 
        life = 0;
      }
    if (lifeModified != null){
      lifeModified();
    }
    if (life <= 0){
      Die(true);
      return;
    }

    lifeBar.SetScale(life / maxLife);
  }

  public void ApplyEffect(EnemyEffect effect){
    if (effect.effectType == EnemyEffectType.damage){
      Damage(effect.intensity, DamageType.normal);
      return;
    }

    bool isActive = EffectHelper.AddEffect(this, effect);
    if (!isActive){
      return;
    }

    if (effect.effectType == EnemyEffectType.burn && burnEffect == null){
      burnEffect = CreateEffect(burnEffectPrefab, effect);
      return;
    }

    if (effect.effectType == EnemyEffectType.slow && slowEffect == null){
      slowEffect = CreateEffect(slowEffectPrefab, effect);
      return;
    }

    if (effect.effectType == EnemyEffectType.value && valueEffect == null){
      valueEffect = CreateEffect(valueEffectPrefab, effect);
      return;
    }
  }

  public void SetSlow(float s){
    s = Mathf.Clamp(s, 0f, 1f);
    float moveSpeed = speed * (1 - s);
    pathTraveler.SetSpeed(moveSpeed);
  }

  public void DefaultRouteChanged(Route newRoute, bool oldDefaultIsValid){
    pathTraveler.DefaultRouteChanged(newRoute, oldDefaultIsValid);
  }

  public float GetTraveledDistance(){
    return pathTraveler.GetTraveledDistance();
  }

  public float GetLife(){ return life > 0 ? life : 0; }
  public float GetArmor(){ return armor; }

  public string GetDescription(){
    string str = $"{Translation.GetTranslation("life")}: {(int)life} / {maxLife}";
    if (armor > 0){
      str += $"\n{Translation.GetTranslation("armor")}: {armor}";
    }
    str += $"\n{Translation.GetTranslation("speed")}: {(int)(speed * 10)}";
    str += $"\n{Translation.GetTranslation("value")}: {value}";
    return str;
  }

  // ==========================
  // Private
  // ==========================

  private void TickEffects(float deltaTime){
    // Waiting effects
    if (waitingEffects.Count > 0){
      
      foreach(EnemyEffect e in waitingEffects){
        e.Tick(this, deltaTime);
      }
      waitingEffects.RemoveAll(e => e.duration <= 0);
    }

    // Active effects
    if (activeEffects.Count > 0){
      List<EnemyEffect> expiredEffects = new List<EnemyEffect>();
      foreach(EnemyEffect e in activeEffects){
        bool expired = e.Tick(this, deltaTime);
        if (expired){
          expiredEffects.Add(e);
        }
      }
      
      foreach(EnemyEffect e in expiredEffects){
        EffectHelper.CurrentExpired(this, e);

        if (e.effectType == EnemyEffectType.burn && burnEffect != null){
          Destroy(burnEffect.gameObject);
          burnEffect = null;
        }
        else if (e.effectType == EnemyEffectType.slow && slowEffect != null){
          Destroy(slowEffect.gameObject);
          slowEffect = null;
        }
        else if (e.effectType == EnemyEffectType.value && valueEffect != null){
          Destroy(valueEffect.gameObject);
          valueEffect = null;
        }
      }
    }

    if (burn > 0){
      Damage(burn * deltaTime, DamageType.damageOverTime);
    }
  }

  private void Die(bool killed){
    EnemyManager.RemoveEnemy(this);

    if (killed){
      int value = GetKillValue();
      GameManager.instance.ModifyResources(gameObject, value);
      
      if (blueprint.deathEffect != null){
        GameObject effect = Instantiate(blueprint.deathEffect, transform.position, Quaternion.identity);
        effect.GetComponentInChildren<ParticleSystemRenderer>().material = GetComponent<MeshRenderer>().material;
        effect.transform.localScale = transform.localScale;
        Destroy(effect, 3f);
      }
    }

    Destroy(lifeBar.gameObject);
    Destroy(gameObject);
  }

  private int GetKillValue(){
    EnemyEffect effect = activeEffects.Find(e => e.effectType == EnemyEffectType.value);
    if (effect != null){
      value = (int) (value * (1 + effect.intensity / 100f));
    }
    return value;
  }

  private float ApplyArmor(float damage, DamageType damageType){
    float armorReduction = armor;
    if (damageType == DamageType.damageOverTime){
      armorReduction = damage * (armorReduction / 60f);
    } else if (armorReduction > damage * 0.9f){
      armorReduction = damage * 0.9f;
    }
    return damage - armorReduction;
  }

  private EffectIntensity CreateEffect(EffectIntensity prefab, EnemyEffect effect){
    EffectIntensity e = Instantiate(prefab, transform.position, transform.rotation, transform);
    e.SetIntensity(effect.intensity);
    return e;
  }

  private void CreateDamageText(DamageType damageType, float damage){
    if (!DataManager.instance.settings.showDamageNumbers){
      return;
    }

    if (DataManager.instance.settings.minimumDamageShown > damage){
      return;
    }

    if (damageType == DamageType.normal){
      GameManager.instance.levelUIManager.CreateDamageText(gameObject, (int)damage);
    } else if (damageType == DamageType.damageOverTime){
      dotDamage += damage;
      if (!showingDotDamage){
        showingDotDamage = true;
        Invoke("ShowDotDamage", dotMessageFrequency);
      }
    }
  }

  private void ShowDotDamage(){
    showingDotDamage = false;
    dotDamage = (int) dotDamage;
    if (dotDamage < 1){ 
      dotDamage = 0f;
      return; 
    }

    GameManager.instance.levelUIManager.CreateDamageText(gameObject, (int)dotDamage);
    dotDamage = 0f;
  }
}

