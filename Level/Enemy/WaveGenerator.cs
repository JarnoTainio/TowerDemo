using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator
{
  EnemyTag preferredType;
  int groupCount;
  int groupSize;
  float spawnDelay;
  int difficulty;
  bool debug = true;

  public Wave CreateWave(int difficulty){
    this.difficulty = difficulty;
    Print($"=== CREATE WAVE === {difficulty}");
    Wave wave = SetupWave();

    SetPreferredType();
    SetGroupSize();
    SetSpawnDelay();
    CreateGroups(wave, groupCount, groupSize);
    Print($"{wave}, difficulty: {this.difficulty}");
    return wave;
  }

  private Wave SetupWave(){
    Wave wave = new Wave();
    wave.enemies = new List<WaveEntry>();
    return wave;
  }

  private void SetPreferredType(){
    EnemyTag enemyTag = EnemyTag.basic;
    int roll = Random.Range(0, 5);
         if (roll == 0){ enemyTag = EnemyTag.armored; }
    else if (roll == 1){ enemyTag = EnemyTag.fast;    }

    preferredType = enemyTag;
    Print($"Preferred type: {preferredType}");
  }

  private void SetGroupSize(){
    int totalCount = Random.Range(15, 25);
    groupCount = Random.Range(2, 5);
    groupSize = (totalCount / groupCount);
    Print($"Group size: {groupSize}, Groups: {groupCount}, Total: {groupSize * groupCount}");
  }

  private void SetSpawnDelay(){
    int roll = Random.Range(0, 3);
    roll = Mathf.Min(roll, difficulty);
    ReduceDifficulty("SpawnDelay", roll);
    spawnDelay = 3f / (roll + 1);
    Print($"Spawn delay: {spawnDelay} (roll: {roll})");
  }

  private void CreateGroups(Wave wave, int groupCount, int groupSize){
    WaveEntry[] waveEntries = InitWaveEntries(groupCount, groupSize);
    UpgradeGroups(waveEntries, groupCount, groupSize);
    wave.enemies = new List<WaveEntry>(waveEntries);
  }

  private WaveEntry[] InitWaveEntries(int groupCount, int groupSize){
    WaveEntry[] waveEntries = new WaveEntry[groupCount * groupSize];
    for(int i = 0; i < groupCount; i++){
      int index = i * groupSize;
      for(int j = 0; j < groupSize; j++){
        WaveEntry waveEntry = new WaveEntry();
        waveEntry.enemyTag = EnemyTag.basic;
        waveEntry.delay = spawnDelay;
        waveEntries[index + j] = waveEntry;
      }
    }
    return waveEntries;
  }

  private void UpgradeGroups(WaveEntry[] waveEntries, int groupCount, int groupSize){
    int i = 50;
    List<int> indexList = new List<int>();
    while(difficulty > 0 && --i > 0){
      // Get random index
      if (indexList.Count == 0){
        indexList = InitIndexList(groupSize);
      }
      int index = indexList[Random.Range(0, indexList.Count)];
      bool didUpgrade = UpgradeGroupIndexes(waveEntries, index);

      if (!didUpgrade){
        break;
      }
    }
  }

  private bool UpgradeGroupIndexes(WaveEntry[] waveEntries, int groupIndex){
    Print($"Upgrade index: {groupIndex}, difficulty: {difficulty}");
    WaveEntry[] targetEntries = new WaveEntry[groupCount];
    for(int i = 0; i < groupCount; i++){
      int index = i * groupSize + groupIndex;
      targetEntries[i] = waveEntries[index];
    }
    return UpgradeEnemies(targetEntries);
  }

  private bool UpgradeEnemies(WaveEntry[] waveEntries){
    WaveEntry first = waveEntries[0];
    int currentCost = GetCost(first.enemyTag, first.tier);

    // Upgrade type
    if (first.enemyTag != preferredType){
      int cost = (GetCost(preferredType, first.tier) - currentCost) * groupCount;
      Print($"Type cost: {cost} / {difficulty}");
      if (cost <= difficulty){
        foreach(WaveEntry entry in waveEntries){
          entry.enemyTag = preferredType;
        }
        ReduceDifficulty("UpgradeType", cost);
        return true;
      }
    }

    // Upgrade tier
    if (first.tier < Tier.third){
      int cost = (GetCost(preferredType, first.tier + 1) - currentCost) * groupCount;
      Print($"Tier cost: {cost} / {difficulty}");
      if (cost <= difficulty){
        foreach(WaveEntry entry in waveEntries){
          entry.tier += 1;
        }
        ReduceDifficulty("UpgradeType", cost);
        return true;
      }
    }

    Print($"Cant upgrade: {difficulty}");
    return false;
  }

  private List<int> InitIndexList(int size){
    List<int> list = new List<int>();
    for(int i = 0; i < size; i++){
      list.Add(i);
    }
    return list;
  }

  private void ReduceDifficulty(string key, int amount){
    this.difficulty -= amount;
    Print($"DifficultyUsed: {key} {amount} ({difficulty})");
  }

  private int GetCost(EnemyTag tag, Tier tier){
    int index = (int) tier;
    Print($"Get cost for {tag} {tier} ({index})");
    if (tag == EnemyTag.basic){
      return new int[]{0, 1, 2}[index];
    }
    if (tag == EnemyTag.fast){
      return new int[]{1, 2, 4}[index];
    }
    if (tag == EnemyTag.armored){
      return new int[]{1, 2, 4}[index];
    }
    if (tag == EnemyTag.boss){
      return new int[]{3, 6, 10}[index];
    }
    return 0;
  }

  private void Print(string str){
    if (!debug){
      return;
    }
    Debug.Log(str);
  }
}
