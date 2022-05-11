using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerInfo : MonoBehaviour
{
  public TranslatedText nameText;
  public TextMeshProUGUI statsText;
  private Enemy enemy;
  private float enemyLife;

  // ==========================
  // Private
  // ==========================

  private string GetDescriptionText(Tower tower, bool showUpgrade){
    List<UpgradeStatus> list = tower.GetDescriptionValues();
    string str = "";
    str += Translation.GetTranslation("cost") +  $": {tower.blueprint.GetBaseCost()} +{tower.blueprint.GetBaseCostGrowth()}";
    foreach(UpgradeStatus sts in list){
      if (str.Length > 0){ str += "\n"; }

      if (!sts.hasValue){
        str += Translation.GetTranslation(sts.strKey);
      }
      else if (showUpgrade && sts.isUpgrade){
        str += Translation.GetTranslation(sts.strKey, $"<color=green>{sts.newValue}</color>");
      }else{
        str += Translation.GetTranslation(sts.strKey, sts.oldValue);
      }
    }
    return str;
  }

  private void UpdateEnemyInfo(){
    Show(enemy);
  }

  private void RemoveCurrentEnemy(){
    if (enemy != null){
      enemy.lifeModified -= UpdateEnemyInfo;
      enemy = null;
    }
  }

  // ==========================
  // Public
  // ==========================

  public void Show(Tower tower, bool showUpgrade){
    TowerBlueprint blueprint = tower.GetBlueprint();
    int level = tower.GetLevel();
    nameText.SetText(blueprint.GetName(level));
    statsText.text = GetDescriptionText(tower, showUpgrade);

    gameObject.SetActive(true);
    RemoveCurrentEnemy();
  }

  public void Show(TowerBlueprint blueprint, Rune[] runes = null){
    nameText.SetText(blueprint.GetName(0));
    statsText.text = GetDescriptionText(blueprint.towerPrefab, false);

    gameObject.SetActive(true);
    RemoveCurrentEnemy();
  }

  public void Show(Enemy enemy){
    RemoveCurrentEnemy();
    this.enemy = enemy;
    enemyLife = enemy.GetLife();
    EnemyBlueprint blueprint = enemy.blueprint;
    int level = enemy.level;
    nameText.SetText(blueprint.GetName(level));
    statsText.text = enemy.GetDescription();
    enemy.lifeModified += UpdateEnemyInfo;

    gameObject.SetActive(true);
  }

  public void Show(Rune rune){
    if (rune == null){
      Hide();
      return;
    }
    RemoveCurrentEnemy();
    nameText.SetKey(rune.nameKey);
    statsText.text = rune.GetDescription();

    gameObject.SetActive(true);
  }

  public void Hide(){
    RemoveCurrentEnemy();
    gameObject.SetActive(false);
  }

}
