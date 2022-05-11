using UnityEngine;

[System.Serializable]
public class TileData
{
  public int x;
  public int y;
  public TileType type;
  public bool isNode;
  public bool isPath;

  public TileData(int x, int y, TileType type = TileType.none){
    this.x = x;
    this.y = y;
    this.type = type;
    this.isPath = false;
    this.isNode = false;
    SetType(type);
  }

  public void SetType(TileType type){
    this.type = type;
    isNode = IsNode();
    isPath = IsPath();
  }

  public bool IsDoor(){
    return type == TileType.start || type == TileType.exit;
  }

  public bool IsNode(){
    if (type == TileType.pathNode){
      return true;
    }
    if (type == TileType.blockingNode){
      return true;
    }
    return false;
  }

  public bool IsPath(){
    if (type == TileType.path){
      return true;
    }
    if (type == TileType.pathNode){
      return true;
    }
    if (type == TileType.start){
      return true;
    }
    if (type == TileType.exit){
      return true;
    }
    return false;
  }

  public bool IsEmpty(){
    return type == TileType.none;
  }

  public Vector2Int GetPosition(){
    return new Vector2Int(x,y);
  }
}