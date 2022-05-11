using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGhost : MonoBehaviour
{
  private GameManager gameManager;
  public Material positiveMaterial;
  public Material negativeMaterial;
  public Tower ghost;
  bool ghostExists;
  bool isPositive;

  void Start(){
    gameManager = GameManager.instance;
  }

  public void Create(TowerBlueprint blueprint){
    if (ghost != null){
      Destroy(ghost.gameObject);
    }

    ghost = Instantiate(blueprint.towerPrefab, Vector3.zero, Quaternion.identity, transform);
    ghost.Init(blueprint, 0);
    ghost.SetMaterials(positiveMaterial);
    ghost.Disable();
    ghostExists = true;
    isPositive = true;
  }

  public void Destroy(){
    if (ghost == null){
      return;
    }
    Destroy(ghost.gameObject);
    ghostExists = false;
  }

  public void UpdatePosition(Node hoveredNode){
    if (!ghostExists){
      return;
    }

    if (hoveredNode != null){
      ghost.gameObject.SetActive(true);
      Vector3 nodePosition = hoveredNode.transform.position;
      ghost.transform.position = nodePosition + new Vector3(0, 0.25f, 0);
      gameManager.towerUI.ShowTowerRange(ghost);
    }else{
      ghost.gameObject.SetActive(false);
      gameManager.towerUI.HideTowerRange();
    }
  }

  public void SetMaterial(bool canBuild){
    if (ghost == null){
      return;
    }
    if (isPositive == canBuild){
      return;
    }
    isPositive = canBuild;
    ghost.SetMaterials(canBuild ? positiveMaterial : negativeMaterial);
  }
}
