using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneList", menuName = "TowerDefence/RuneList")]
public class RuneList : ScriptableObject
{
  public List<Rune> list;

  public Rune GetRune(int index){
    return list[index % list.Count];
  }

  public Rune GetRune(string nameKey){
    return list.Find(t => t.nameKey == nameKey);
  }
}