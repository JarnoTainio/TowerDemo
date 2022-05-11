using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveEditor : MonoBehaviour
{
  public LevelEditor levelEditor;
  public TextMeshProUGUI enemyText;
  public EnemyTag enemyTag;
  public Tier enemyTier;
  public EditorWaveTick tickPrefab;
  private List<EditorWaveTick> waveTicks;
  private int waveCount;
  private int waveLength = 30;
  public Vector3 offSet = new Vector3(100, 0, 0);
  private LevelData levelData;
  public EnemyBlueprintList enemyList;

  void Start(){
    waveTicks = new List<EditorWaveTick>();
    levelData = levelEditor.levelData;
    LoadWaves();
    UpdateEnemyText();
  }

  private void LoadWaves(){
    if (levelData.waves == null){
      levelData.waves = new List<Wave>();
    }
    waveCount = levelData.waves.Count;

    for(int y = 0; y < waveCount; y++){
      CreateWave(y, levelData.waves[y].enemies);
    }
  }

  void Update(){
    if (Input.GetKeyDown(KeyCode.Q)){
      enemyTag++;
      int tagLength = Enum.GetNames(typeof(EnemyTag)).Length;
      if ((int)enemyTag >= tagLength){
        enemyTag = EnemyTag.none;;
      }
      UpdateEnemyText();
    }
    else if (Input.GetKeyDown(KeyCode.W)){
      enemyTier++;
      int tagLength = Enum.GetNames(typeof(Tier)).Length;
      if ((int)enemyTier >= tagLength){
        enemyTier = Tier.first;;
      }
      UpdateEnemyText();
    }

    else if (Input.GetKeyDown(KeyCode.A)){
      AddWave();
    }
    else if (Input.GetKeyDown(KeyCode.S)){
      RemoveWave();
    }
  }

  private void UpdateEnemyText(){
    enemyText.text = $"{enemyTag} {enemyTier}";
    WaveEntry entry = new WaveEntry(){enemyTag = enemyTag, tier = enemyTier};
    enemyText.color = GetTileColor(entry);
  }

  public void RemoveWave(){
    if (waveCount <= 0){
      return;
    }
    waveCount--;
    levelData.waves.RemoveAt(levelData.waves.Count - 1);

    List<EditorWaveTick> list = new List<EditorWaveTick>();
    foreach(EditorWaveTick tick in waveTicks){
      if (tick.y == waveCount){
        list.Add(tick);
      }
    }

    foreach(EditorWaveTick tick in list){
      waveTicks.Remove(tick);
      Destroy(tick.gameObject);
    }
  }

  public void AddWave(){
    CreateWave(waveCount);
    waveCount++;

    List<Wave> waves = new List<Wave>(levelData.waves);
    List<WaveEntry> waveEntries = new List<WaveEntry>();
    for(int i = 0; i < waveLength; i++){
      waveEntries.Add(new WaveEntry());
    }
    Wave newWave = new Wave();
    newWave.enemies = waveEntries;

    waves.Add(newWave);
    levelData.waves = waves;
  }

  private void CreateWave(int y, List<WaveEntry> entries = null){
    for(int x = 0; x < waveLength; x++){
      EditorWaveTick tick = Instantiate(tickPrefab, new Vector3(x, 0, y * 2) + offSet, Quaternion.identity, transform);
      tick.Init(this, x, y);
      WaveEntry entry = entries != null ? entries[x] : new WaveEntry();
      SetTick(tick, entry);
      waveTicks.Add(tick);
    }
  }

  public void TickClicked(EditorWaveTick tick){
    WaveEntry entry = levelData.waves[tick.y].enemies[tick.x];
    entry.enemyTag = enemyTag;
    entry.tier = enemyTier;
    SetTick(tick, entry);
  }

  private void SetTick(EditorWaveTick tick, WaveEntry entry){
    Color color = GetTileColor(entry);
    tick.SetColor(color);
  }

  private Color GetTileColor(WaveEntry entry){
    Color color = Color.gray;
    foreach(EnemyBlueprint eb in enemyList.list){
      if (eb.tag == entry.enemyTag){
        color = eb.GetMaterial((int)entry.tier).color;
      }
    }
    return color;
  }
}
