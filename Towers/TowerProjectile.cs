using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : TowerShooting
{
  public Bullet bulletPrefab;

  protected override void Fire(){
    Bullet bullet = Instantiate(bulletPrefab, shootingPoint.transform.position, Quaternion.identity);
    bullet.damage = damage;
    bullet.radius = blueprint.GetEffectRadius(level);
    bullet.SetTarget(target, blueprint.targetingType);
  }

  public override List<UpgradeStatus> GetDescriptionValues(){
    List<UpgradeStatus> list = base.GetDescriptionValues();
    float radius = blueprint.GetEffectRadius(level);
    if (radius > 0f){
      list.Add(new UpgradeStatus("info_explosionRadius", blueprint.GetEffectRadius(level), blueprint.GetEffectRadius(level + 1)));
    }
    return list;
  }
  
}
