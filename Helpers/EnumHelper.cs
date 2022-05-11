[System.Serializable]
public enum EnemyTag { none=0, basic, fast, armored, boss };

[System.Serializable]
public enum Tier { first=0, second=1, third=2 };

[System.Serializable]
public enum TargetingType { none=0, target, point, tile };

[System.Serializable]
public enum TargetingPriority { none=0, closest, furthest, life, lowLife, armor };

[System.Serializable]
public enum TowerType { normal, pathAdjacent };

[System.Serializable]
public enum TileType{none = 0, path = 1, pathNode = 2, blockingNode = 3, start = 100, exit = 200 }

[System.Serializable]
public enum PathType{none = 0, path = 1, tower = 2 }

[System.Serializable]
public enum DamageType { normal, damageOverTime };

[System.Serializable]
public enum EnemyEffectType { none=0, slow, burn, value, damage, teleport };

[System.Serializable]
public enum TowerTag { cooldown, shooting, damage, effect, range }
[System.Serializable]
public enum Attribute { cost, costIncrease, reloadSpeed, range, damage, sellValue, buildingCooldown };

[System.Serializable]
public enum StringFormat { normal, capitalized, upcase, lowcase };

[System.Serializable]
public enum Language {EN=0, FI=1}