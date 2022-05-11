using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyBlueprintList enemyBlueprints;
    public Enemy enemyPrefab;
    public Vector3 spawningOffset;
    static List<Enemy> enemyList;
    public GameObject lifeBarContainer;
    public LifeBar lifeBarPrefab;
    public TowerInfo towerInfo;

    public void Awake(){
      enemyList = new List<Enemy>();
    }

    public Enemy SpawnEnemy(EnemyTag enemyTag, int tier, Vector3 position){
      EnemyBlueprint blueprint = GetEnemy(enemyTag);
      Enemy enemy = Instantiate(enemyPrefab, position + spawningOffset, Quaternion.identity, transform);
      LifeBar lifebar = CreateLifebar(blueprint, tier, enemy);
      
      enemy.lifeBar = lifebar;
      enemy.Init(this, blueprint, tier);
      enemyList.Add(enemy);
      
      return enemy;
    }

    private LifeBar CreateLifebar(EnemyBlueprint blueprint, int tier, Enemy enemy){
      LifeBar lifebar = Instantiate(lifeBarPrefab, lifeBarContainer.transform);
      float s = blueprint.GetScale(tier).x;
      s -= 0.35f;
      Vector3 offset = new Vector3(1f, 20 * (1 + s), 1f);
      float size = 1f + s * 3f;
      lifebar.Init(enemy.gameObject, offset, size);
      return lifebar;
    }

    public static void RemoveEnemy(Enemy enemy){
      enemyList.Remove(enemy);
      if (enemyList.Count == 0){
        GameManager.instance.NoEnemiesLeft();
      }
    }

    public static Enemy[] GetEnemies(){
      return enemyList.ToArray();
    }

    private EnemyBlueprint GetEnemy(EnemyTag enemyTag){
      foreach(EnemyBlueprint blueprint in enemyBlueprints.list){
        if (blueprint.tag == enemyTag){
          return blueprint;
        }
      }
      throw new System.Exception("Unknown enemy type!");
    }

    public void EnemyClicked(Enemy enemy){
      GameManager.instance.HideTowerInfo();
      towerInfo.Show(enemy);
    }
}
