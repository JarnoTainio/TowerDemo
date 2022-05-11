using UnityEngine;

public class Bullet : MonoBehaviour
{
  public int damage = 5;
  public float radius = 0f;
  public float speed = 20f;
  public GameObject impactEffect;
  private Enemy target;
  private Vector3 targetVector;
  private TargetingType targetingType;

  public void SetTarget(Enemy target, TargetingType targetingType){
    this.target = target;
    this.targetingType = targetingType;
    this.targetVector = target.transform.position;
  }

  public void Update(){
    if (targetingType == TargetingType.target && target == null){
      Destroy(gameObject);
      return;
    }
    
    Vector3 targetPoint = GetTargetPoint();
    transform.position = Vector3.MoveTowards(transform.position, targetPoint, Time.deltaTime * speed);
    if (Vector3.Distance(transform.position, targetPoint) < 0.1f){
      if (radius > 0){
        Explode();
      } else {
        Damage(target);
      }
      Die();
    }
  }

  private Vector3 GetTargetPoint(){
    if (targetingType == TargetingType.point){
      return targetVector;
    }
    return target.transform.position;
  }

  private void Explode(){
    foreach(Enemy enemy in EnemyManager.GetEnemies()){
      if (Vector3.Distance(transform.position, enemy.transform.position) < radius){
        Damage(enemy);
      }
    }
  }

  private void Damage(Enemy enemy){
    enemy.Damage(damage, DamageType.normal);
  }

  private void Die(){
    if (impactEffect != null){
        GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
    }
    Destroy(gameObject);
  }
}
