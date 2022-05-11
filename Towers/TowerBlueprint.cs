using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "TowerDefence/TowerBlueprint")]
public class TowerBlueprint : ScriptableObject
{
  public Tower towerPrefab;
  public Sprite icon;
  public string nameKey;
  
  [Header("Attributes")]
  public Material[] materials;
  public int[] cost;
  public int costGrowth;
  public int[] damage;
  public float[] cooldown;
  public float[] range;
  public float[] rotateSpeed;

  [Header("Options")]
  public TowerTag[] tags;
  public TowerType towerType;
  public TargetingType targetingType;
  public int[] charges;
  public bool startReady;

  [Header("Effect")]
  public EnemyEffectType[] effectType;
  public float[] effectDuration;
  public float[] effectIntensity;
  public float[] effectRadius;

  public bool CanBuild(Node node) {
    if (towerType == TowerType.pathAdjacent){
      return node.isAdjacentToPath;
    }
    return true;
  }
  public bool CanUpgrade(int currentTier){ return currentTier + 1 < cost.Length; }

  public bool HasTag(TowerTag tag){ return ArrayHelper.Contains<TowerTag>(tags, tag); }
  
  public Material GetMaterial(int tier){ return ArrayHelper.GetValue<Material>(tier, materials);}
  
  public int GetCost(int tier, int towerCount = 0){ 
    float costModifier = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.cost);
    int value = ArrayHelper.GetValue<int>(tier, cost);

    if (tier == 0){
      value = (int)(value * costModifier);
      float costGrowthModifier = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.costIncrease);
      value += (int)(costGrowth * costGrowthModifier) * towerCount;
    }
    return value;
  }

  public string GetBaseCost(){
    float costModifier = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.cost);
    float buildingCooldown = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.buildingCooldown);
    int value = (int)(ArrayHelper.GetValue<int>(0, cost) * costModifier);
    if (buildingCooldown > 1f){
      return $"{value}s"; 
    }
    return value.ToString();
  }

  public string GetBaseCostGrowth(){
    float costGrowthModifier = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.costIncrease);
    float buildingCooldown = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.buildingCooldown);
    int value = (int)(costGrowth * costGrowthModifier);
    if (buildingCooldown > 1f){
      return $"{value}"; 
    }
    return value.ToString();
  }

  public int GetBuildingCooldown(int towerCount = 0){
    float buildingCooldown = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.buildingCooldown);
    if (buildingCooldown <= 1f){
      return 0;
    }
    int value = GetCost(0, towerCount);
    return value;
  }

  public int GetDamage(int tier){ 
    float modifier = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.damage);
    return (int)(ArrayHelper.GetValue<int>(tier, damage) * modifier);
  }

  public float GetCooldown(int tier){ 
    float modifier = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.reloadSpeed);
    return ArrayHelper.GetValue<float>(tier, cooldown) / modifier;
  }

  public float GetRange(int tier){ 
    float modifier = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.range);
    return ArrayHelper.GetValue<float>(tier, range) * modifier;
  }

  public int GetCharges(int tier){ return ArrayHelper.GetValue<int>(tier, charges);}
  public EnemyEffectType GetEffectType(int tier){ return ArrayHelper.GetValue<EnemyEffectType>(tier, effectType);}
  public float GetEffectRadius(int tier){ return ArrayHelper.GetValue<float>(tier, effectRadius);}
  public float GetEffectDuration(int tier){ return ArrayHelper.GetValue<float>(tier, effectDuration);}
  public float GetEffectIntensity(int tier){ return ArrayHelper.GetValue<float>(tier, effectIntensity);}
  public float GetRotateSpeed(int tier){ return ArrayHelper.GetValue<float>(tier, rotateSpeed);}

  public static string[] tierStrings = new string[]{"", " II", " III"};
  public string GetName(int tier) {
    return Translation.GetTranslation(nameKey) + tierStrings[tier];
  }

  public int GetSellValue(int tier, int towerCount){
    int value = 0;
    for(int i = 0; i <= tier; i++){
      value += GetCost(i, towerCount);
    }
    float sellValue = DataManager.instance.runData.GetRuneModifier(nameKey, Attribute.sellValue);
    if (sellValue > 1f){
      return value;
    }
    return (int)(value * 0.75f);
  }


}

