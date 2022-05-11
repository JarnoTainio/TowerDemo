using System.Collections.Generic;
using UnityEngine;

public abstract class Rune : ScriptableObject
{
  public string nameKey;
  public Sprite icon;
  
  [Header("Tags")]
  public List<TowerTag> requiredTags;
  public List<TowerTag> forbiddenTags;

  [Header("Values")]
  public bool isUnique;

  public bool CanEquip(TowerBlueprint blueprint){
    foreach(TowerTag tag in requiredTags){
      if (!blueprint.HasTag(tag)){
        return false;
      }
    }
    return true;
  }

  public abstract string GetDescription();
  public virtual float GetAttributeModifier(Attribute attribute){ return 0f; }

  public override string ToString()
  {
    return nameKey;
  }
}
