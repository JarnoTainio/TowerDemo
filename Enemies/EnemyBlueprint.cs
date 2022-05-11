using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "TowerDefence/EnemyBlueprint")]
public class EnemyBlueprint : ScriptableObject
{
  public string nameKey;
  public GameObject deathEffect;
  public EnemyTag tag;

  [Header("Visual")]
  public Mesh mesh;
  public Material[] material;
  public Vector3[] scale;

  [Header("Attributes")]
  public int[] life;
  public int[] armor;
  public float[] speed;
  [Tooltip("Resources gained when killed")]
  public int[] value;
  [Tooltip("Lives lost when the enemy reaches end")]
  public int[] danger;

  public Material GetMaterial(int tier){ return ArrayHelper.GetValue<Material>(tier, material);}
  public Vector3 GetScale(int tier){ return ArrayHelper.GetValue<Vector3>(tier, scale);}
  public int GetLife(int tier){ return ArrayHelper.GetValue<int>(tier, life);}
  public int GetArmor(int tier){ return ArrayHelper.GetValue<int>(tier, armor);}
  public float GetSpeed(int tier){ return ArrayHelper.GetValue<float>(tier, speed);}
  public int GetValue(int tier){ return ArrayHelper.GetValue<int>(tier, value);}
  public int GetDanger(int tier){ return ArrayHelper.GetValue<int>(tier, danger);}

  public string GetName(int tier) {
    return Translation.GetTranslation(nameKey) + TowerBlueprint.tierStrings[tier];
  }
}