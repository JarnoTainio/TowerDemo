using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
  [Header("Level")]
  public int waveCount = 6;

  [Header("Wave")]
  public float waveDelay = 5f;
  public float waveTickDuration = 0.5f;
  private float waveCounter;
  private int waveIndex;
  private int difficulty;

  [Header("UI")]
  public TextMeshProUGUI waveText;

  private Vector3 spawningPoint;
  private EnemyManager enemyManager;
  private PathManager pathManager;
  private Wave[] waves;
  public static bool hasWavesLeft;
  public static bool levelStarted;

  void Awake(){
    hasWavesLeft = true;
    levelStarted = false;
  }

  public void Start(){
    enemyManager = GetComponent<EnemyManager>();
    pathManager = Object.FindObjectOfType<PathManager>();
    spawningPoint = pathManager.GetSpawningPoint();
    waveIndex = 0;
    waveCounter = 3f;
    waves = GameManager.instance.GetWaveList();
    waveText.text = Translation.GetTranslation("buildTower");
    difficulty = DataManager.instance.runData.GetDifficulty();

    waveCount = Mathf.Min(waveCount + difficulty / 3, difficulty + 2);
  }

  public void Update(){
    if (levelStarted == false)
      return;

    if (hasWavesLeft == false)
      return;

    waveCounter += Time.deltaTime;
    if (waveCounter >= waveDelay){
      StartCoroutine(SpawnWave());
      waveCounter = -3600f;
    }
  }

  private IEnumerator SpawnWave(){
    UpdateWaveText();
    //Wave wave = waves[waveIndex];
    Wave wave = CreateWave(difficulty, waveIndex);
    foreach(WaveEntry waveEntry in wave.enemies){
      yield return new WaitForSeconds(waveEntry.delay);
      if (waveEntry.enemyTag != EnemyTag.none){
        Enemy enemy = enemyManager.SpawnEnemy(waveEntry.enemyTag, (int)waveEntry.tier, spawningPoint);
        enemy.pathTraveler.SetRoute(pathManager.GetDefaultRoute());
      }
    }
    waveIndex++;
    waveCounter = 0f;

    if (waveIndex >= waveCount){
      hasWavesLeft = false;
    }
  }

  public bool HasWavesLeft(){
    return hasWavesLeft;
  }

  private void UpdateWaveText(){
    waveText.text = $"{Translation.GetTranslation("wave")} {waveIndex + 1} / {waveCount}";
  }

  private Wave CreateWave(int difficulty, int waveIndex){
    WaveGenerator waveGenerator = new WaveGenerator();
    return waveGenerator.CreateWave(difficulty + waveIndex * 2);

    /*
    Wave wave = new Wave();
    wave.enemies = new List<WaveEntry>();
    int enemyCount = Random.Range(10, 20);
    
    int groupCount = Random.Range(1, 3);
    int enemiesPerGroup = enemyCount / groupCount;
    int challenge = difficulty + waveIndex;
    for(int i = 0; i < groupCount; i++){
      int diff = Mathf.Min(challenge, (waveIndex + 1) * 2 + i);
      EnemyGroup enemyGroup = new EnemyGroup(enemiesPerGroup, diff);
      enemyGroup.AddWaveEntries(wave.enemies);
    }
    return wave;
    */

  }


  
}
