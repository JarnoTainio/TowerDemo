using UnityEngine;

public class TowerRange : MonoBehaviour
{
  public GameObject upgradeRange;
  public Tower currentTower;
  public void Hide(){
    gameObject.SetActive(false);
    currentTower = null;
  }

  public void Show(Tower tower, bool showUpgrade){
    gameObject.SetActive(true);
    Vector3 newPosition = tower.transform.position + new Vector3(0, 0.2f, 0);
    if (showUpgrade == upgradeRange.activeSelf && !ShouldUpdate(tower, newPosition)){
      return;
    }
    currentTower = tower;
    transform.position = newPosition;
    float range = tower.GetRange() * 2;
    transform.localScale = new Vector3(range, 0.01f, range);

    upgradeRange.SetActive(showUpgrade);
    if (showUpgrade && range > 0){
      float upgradedRange = 2 * tower.GetBlueprint().GetRange(tower.GetLevel() + 1) / range;
      upgradeRange.transform.localScale = new Vector3(upgradedRange, 1f, upgradedRange);
    }
  }

  public void Show(TowerBlueprint blueprint, Vector3 position){
    gameObject.SetActive(true);
    if (!ShouldUpdate(blueprint.towerPrefab, position)){
      return;
    }
    currentTower = blueprint.towerPrefab;
    gameObject.SetActive(true);
    float range = blueprint.GetRange(0) * 2;
    transform.position = position;
    transform.localScale = new Vector3(range, 0.01f, range);
    upgradeRange.SetActive(false);

    }
    private bool ShouldUpdate(Tower tower, Vector3 position){
      if (currentTower == tower){
        if (position == transform.position){
          return false;
        }
      }
      else {
        currentTower = tower;
      }
      return true;
  }
}
