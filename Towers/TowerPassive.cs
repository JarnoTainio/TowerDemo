using System.Collections.Generic;
public class TowerPassive : Tower
{
  private int resourcesGained;
  public override void Init(TowerBlueprint blueprint, int level)
  {
    base.Init(blueprint, level);
    resourcesGained = blueprint.GetDamage(level);
  }

  protected override void Activate(){
    GameManager.instance.ModifyResources(gameObject, resourcesGained);
  }

  public override List<UpgradeStatus> GetDescriptionValues(){
    List<UpgradeStatus> list = base.GetDescriptionValues();
    float radius = blueprint.GetEffectRadius(level);
    if (blueprint.GetDamage(level) > 0){
      list.Add(new UpgradeStatus("info_resourceGain", blueprint.GetDamage(level), blueprint.GetDamage(level + 1)));
    }
    return list;
  }

}
