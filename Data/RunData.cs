using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
  public List<TowerRunData> towerList;
  public List<string> runeStorage;
  public int level;
  public int maximumTowerCount = 9;

  public RunData(string[] towers){
    towerList = new List<TowerRunData>();
    foreach(string towerName in towers){
      TowerRunData towerRunData = new TowerRunData(towerName);
      towerList.Add(towerRunData);
    }
    runeStorage = new List<string>();
    for(int i = 0; i < 16; i++){
      runeStorage.Add(null);
    }
  }

  // =======================
  // LEVELS
  // =======================

  public void NewRun(){
    level = 0;
    towerList = new List<TowerRunData>();
    TowerRunData towerRunData = new TowerRunData(DataManager.instance.towerList.list[0].nameKey);
    towerList.Add(towerRunData);
  }

  public void NextLevel(){
    level++;
  }

  public int GetDifficulty(){
    return level;
  }

  // =======================
  // TOWERS
  // =======================

  public TowerBlueprint[] GetTowerBlueprints(){
    TowerBlueprintList list = DataManager.instance.towerList;
    List<TowerBlueprint> res = new List<TowerBlueprint>();
    foreach(TowerRunData towerData in towerList){
      TowerBlueprint towerBlueprint = list.GetTower(towerData.towerName);
      if (towerBlueprint != null){
        res.Add(towerBlueprint);
      }
    }
    return res.ToArray();
  }

  public TowerBlueprint[] GetTowerOptions(){
    TowerBlueprintList list = DataManager.instance.towerList;
    List<TowerBlueprint> res = new List<TowerBlueprint>(list.list.ToArray());
    foreach(TowerRunData towerData in towerList){
      TowerBlueprint towerBlueprint = list.GetTower(towerData.towerName);
      if (towerBlueprint != null){
        res.Remove(towerBlueprint);
      }
    }
    return res.ToArray();
  }

  public void AddTower(string nameKey){
    TowerRunData towerRunData = new TowerRunData(nameKey);
    towerList.Add(towerRunData);
  }

  public bool HasRoomForTower(){
    return towerList.Count < maximumTowerCount;
  }

  // =======================
  // RUNES
  // =======================

  public Rune[] GetRunes(string towerName){
    for(int i = 0; i < towerList.Count; i++){
      if (towerList[i].towerName == towerName){
        return GetRunes(i);
      }
    }
    return null;
  }

  public Rune[] GetRunes(int towerIndex){
    RuneList list = DataManager.instance.runeList;
    List<Rune> runes = new List<Rune>();
    string[] runeNames = null;
    if (towerIndex == -1){
      runeNames = runeStorage.ToArray();
    }else{
      runeNames = towerList[towerIndex].runeNames;
    }
    for(int i = 0; i < runeNames.Length; i++){
      Rune rune = list.GetRune(runeNames[i]);
      runes.Add(rune);
    }
    return runes.ToArray();
  }

  public void SetRune(int towerIndex, int runeIndex, string runeName){
    if (towerIndex == -1){
      runeStorage[runeIndex] = runeName;
    } else {
      towerList[towerIndex].runeNames[runeIndex] = runeName;
    }
  }

  public float GetRuneModifier(string towerName, Attribute attribute){
    Rune[] runes = GetRunes(towerName);
    if (runes == null){
      return 1f;
    }

    float value = 1f;
    foreach(Rune rune in runes){
      if (rune != null){
        value += rune.GetAttributeModifier(attribute);
      }
    }
    return value;
  }

  public WeightedList<Rune> GetRuneOptions(){
    RuneList list = DataManager.instance.runeList;
    WeightedList<Rune> res = new WeightedList<Rune>(list.list);
    float weightModify = 0.65f;

    // Towers
    foreach(TowerRunData towerData in towerList){
      // Runes
      Rune[] runes = GetRunes(towerData.towerName);
      foreach(Rune rune in runes){
        if (rune != null){
          if (rune.isUnique){
            res.Remove(rune);
          } else {
            res.ModifyWeight(rune, weightModify);
          }
        }
      }
    }

    // Storage
    Rune[] storageRunes = GetRunes(-1);
    foreach(Rune rune in storageRunes){
      if (rune != null){
        if (rune.isUnique){
          res.Remove(rune);
        } else {
          res.ModifyWeight(rune, weightModify);
        }
      }
    }

    return res;
  }

  public void AddRune(string runeName){
    for(int i = 0; i < runeStorage.Count; i++){
      string str = runeStorage[i];
      if (str == null || str == ""){
        runeStorage[i] = runeName;
        return;
      }
    }
    Debug.LogError("Couldn't add rune! Rune slots: " + (runeStorage?.Count));
  }

  public bool HasRoomForRune(){
    for(int i = 0; i < runeStorage.Count; i++){
      if (runeStorage[i] == null || runeStorage[i] == ""){
        return true;
      }
    }
    return false;
  }
}
