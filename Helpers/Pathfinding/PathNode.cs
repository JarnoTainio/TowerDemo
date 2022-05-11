using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathNode
{
  public int x;
  public int y;
  public int cost;
  public int distance;
  public PathNode parent;
  public Vector3 worldPoint;

  public PathNode(int x, int y, Vector2Int target, int cost = 0, PathNode parent = null){
    this.x = x;
    this.y = y;
    this.distance = (int)Vector2Int.Distance(new Vector2Int(x,y), target);
    this.cost = cost;
    this.parent = parent;
  }

  public Vector2Int Vector(){
    return new Vector2Int(x,y);
  }

  public override string ToString()
  {
    return $"{x}, {y} ({cost})";
  }

  public List<PathNode> ToList(){
    return BuildList(this);
  }

  public static List<PathNode> BuildList(PathNode node){
    PathNode current = node;
    List<PathNode> list = new List<PathNode>();
    while(current != null){
      list.Add(current);
      current = current.parent;
    }
    return list;
  }
}
