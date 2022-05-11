using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
  public FadeIn fadeIn;
  public LevelData[] levels;
  public List<LevelNode> levelNodes;

  public void Awake(){
    fadeIn.StartFadeIn();
  }
  
  void Start()
  {
    for(int i = 0; i < levelNodes.Count; i++){
      levelNodes[i].gameObject.SetActive(i < levels.Length);
      levelNodes[i].Init(i, LevelSelected);
    }
  }

  public void LevelSelected(int index){
    fadeIn.StartFadeOut((done) => DataManager.instance.StartLevel(levels[index]));
  }
}
