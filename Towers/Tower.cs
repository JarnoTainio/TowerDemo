using UnityEngine;
using System.Collections.Generic;

public abstract class Tower : MonoBehaviour
{
  public static int ID;
  public int id;

  [Header("Unity")]
  public MeshRenderer[] renderers;
  [HideInInspector]
  public Node node;
  
  // Attributes
  public TowerBlueprint blueprint;
  public int level;
  protected float cooldown;
  
  // activation
  protected float activationCounter;
  public TargetingPriority targetingPriority;

  public virtual void Init(TowerBlueprint blueprint, int level)
  {
    this.blueprint = blueprint;
    this.level = level;

    Material material = blueprint.GetMaterial(level);
    foreach(MeshRenderer mr in renderers){
      mr.material = material;
    }
    
    if (level == 0){
      this.targetingPriority = TargetingPriority.furthest;
      this.activationCounter = blueprint.startReady ? 0 : this.cooldown;
    } else {
      this.activationCounter = blueprint.startReady ? 0 : this.activationCounter;
    }

    this.cooldown = blueprint.GetCooldown(level);
  }

  // ==========================
  // Unity functions
  // ==========================
  
  public void Start(){
    id = ID++;
  }
  public void Update(){
    Tick(Time.deltaTime);
  }

  public void OnMouseDown(){ if (node != null){ node.OnMouseDown(); } }
  public void OnMouseEnter(){ if (node != null){ node.OnMouseEnter(); } }
  public void OnMouseExit(){ if (node != null){ node.OnMouseExit(); } }

  public void OnDrawGizmosSelected(){
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, GetRange());
    DrawAdditionalGizmos();
  }

  protected virtual void DrawAdditionalGizmos(){}

  // ==========================
  // Private
  // ==========================

  protected virtual void Tick(float deltaTime){
    ActivationTick(deltaTime);
  }

  protected void ActivationTick(float deltaTime){
    activationCounter -= deltaTime;
    if (activationCounter <= 0){
      Activate();
      activationCounter = cooldown;
    }
  }

  protected abstract void Activate();

  public virtual float GetRange(){ return 0;}

  // ==========================
  // Public
  // ==========================

  public int GetSellValue(){ 
    int towerCount = GameManager.instance.buildManager.GetTowerCount(blueprint);
    return blueprint.GetSellValue(level, towerCount - 1); 
  }
  public bool CanUpgrade(){ return blueprint.CanUpgrade(level); }
  public int GetUpgradeCost(){ return blueprint.GetCost(level + 1); }
  public TowerBlueprint GetBlueprint(){return blueprint; }
  public int GetLevel(){ return level; }
  public virtual void SetFacing(Quaternion quaternion){}
  public virtual Quaternion GetFacing(){return transform.rotation; }
  public virtual void SetPath(ITile path){}
  public virtual ITile GetPath(){return null; }
  public virtual bool UseTargetingPriority(){return false; }

  public virtual List<UpgradeStatus> GetDescriptionValues(){
    List<UpgradeStatus> list = new List<UpgradeStatus>();
    if (blueprint.GetCooldown(level) > 0.2f){
      list.Add(new UpgradeStatus(
          "info_cooldown", 
          blueprint.GetCooldown(level), 
          blueprint.GetCooldown(level + 1), 
          false
      ));
    }
    return list;
  }

  public float GetCooldownPercentage(){
    if (cooldown > 0.25f){
      return 1f - activationCounter / cooldown;
    }
    return 0f;
  }

  public void SetMaterials(Material material){
    if (gameObject == null){
      return;
    }
    foreach (Renderer r in GetComponentsInChildren<MeshRenderer>())
    {
      r.material = material;
    }
  }

  public virtual void Disable(){
    enabled = false;
  }
}