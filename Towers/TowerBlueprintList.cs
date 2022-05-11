using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerList", menuName = "TowerDefence/TowerBlueprintList")]
public class TowerBlueprintList : ScriptableObject
{
  public List<TowerBlueprint> list;

  public TowerBlueprint GetTower(int index){
    return list[index % list.Count];
  }

  public TowerBlueprint GetTower(string nameKey){
    return list.Find(t => t.nameKey == nameKey);
  }
}