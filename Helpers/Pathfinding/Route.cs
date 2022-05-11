using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Route
{
  public List<PathNode> nodeList;
  public bool isDefaultRoute;
  public int currentIndex;

  public Route(List<PathNode> nodeList, bool isDefaultRoute){
    this.nodeList = nodeList;
    this.isDefaultRoute = isDefaultRoute;
    currentIndex = 0;
  }

  public bool Contains(Vector2 pos, int startingIndex = 0){
    for(int i = startingIndex; i < nodeList.Count; i++)
    {
      PathNode node = nodeList[i];
      if (node.Vector() == pos){
        return true;
      }
    }
    return false;
  }

  public bool IsValid(Route newRoute, bool defaultIsValid){
    if (defaultIsValid && isDefaultRoute){
      return true;
    }
    return false;
  }

  public int GetRelativeIndex(Route route, int startingIndex){
    for(int i = 0; i < nodeList.Count; i++)
    {
      Vector2 pos = nodeList[i].Vector();
      if (route.Contains(pos, startingIndex)){
        return i;
      }
    }
    return -1;
  }

  public Vector3 GetWorlPoint(int index){
    return nodeList[index].worldPoint;
  }

  public int Length(){
    return nodeList.Count;
  }

}
