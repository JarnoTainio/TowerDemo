using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup
{
  public int size;
  public float difficulty;
  public EnemyTag preferredTag;
  public float spawnDelay;

  public EnemyGroup(int size, int difficulty){
    this.size = size;
    this.difficulty = difficulty;

    int roll = Random.Range(0, 10);
    preferredTag = EnemyTag.basic;
    if (roll == 11 && difficulty > 4){
      preferredTag = EnemyTag.boss;
      this.difficulty -= 3;
    }
    else if (roll >= 9 && difficulty > 3){
      preferredTag = EnemyTag.armored;
      this.difficulty -= 2;
    }
    else if (roll >= 7 && difficulty > 2){
      preferredTag = EnemyTag.fast;
      this.difficulty -= 1;
    }

    spawnDelay = Mathf.Clamp(Random.Range(0, (int)(difficulty / 2)), 0, 3);
    this.difficulty -= spawnDelay;
    spawnDelay = 3f / (spawnDelay + 1);
  }

  public void AddWaveEntries(List<WaveEntry> waveEntries){
    for(int i = 0; i < size; i++){
      WaveEntry waveEntry = CreateWaveEntry(i, size);
      waveEntries.Add(waveEntry);
    }
  }

  private WaveEntry CreateWaveEntry(int i, int size){
    float points = difficulty + 2 * i / size;
    EnemyTag tag = preferredTag;
    if (i >= (size - 1 - points / 3) && points >= 2 && Random.Range(0, 10) == 0){
      tag = EnemyTag.boss;
      points -= 2;
      points /= 2;
    }

    // Enemy tier
    int tier = (int)Random.Range(points / 5, points / 3);
    Tier enemyTier = (Tier)Mathf.Clamp(tier, 0, 2);

    // Create enemy
    WaveEntry waveEntry = new WaveEntry(){
        enemyTag = tag, tier = enemyTier, delay = spawnDelay
    };
    return waveEntry;
  }

}