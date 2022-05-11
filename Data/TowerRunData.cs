using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerRunData
{
  public string towerName;
  public string[] runeNames;

  public TowerRunData(string towerName){
    this.towerName = towerName;
    runeNames = new string[3];
  }

  public void SetRune(int index, string nameKey){
    runeNames[index] = nameKey;
  }
}
