using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
  // Unity
  public Path pathPrefab;
  public Node nodePrefab;
  public Node blockingNodePrefab;
  public Vector3 pathOffSet;
  int tileSize = 1;

  // Grid
  private PathType[,] pathGrid;
  private Vector2Int gridStart;
  private Vector2Int gridEnd;

  // Vectors
  private Vector3 spawningPoint;
  private Vector3 endPoint;
  public Route defaultRoute;
  private List<ITile> paths;

  /*========================
   * Unity functions
   *========================*/
  void Awake(){
    LevelData levelData = GameManager.instance.levelData;
    int size = levelData.size;
    pathGrid = new PathType[size, size];
    List<Node> nodes = SpawnTiles();
    SetAdjacencies(nodes);
    gridStart = levelData.start.GetPosition();
    gridEnd = levelData.exit.GetPosition();

    defaultRoute = new Route(CreatePath(gridStart), true);
    spawningPoint = defaultRoute.GetWorlPoint(0);
  }

  /*========================
   * Private functions
   *========================*/

  private List<Node> SpawnTiles(){
    paths = new List<ITile>();
    List<Node> nodes = new List<Node>();
    SpawnPath(GameManager.instance.levelData.start);
    SpawnPath(GameManager.instance.levelData.exit);
    LevelData data = GameManager.instance.levelData;
    foreach(TileData tileData in data.tiles){
      if (tileData.type == TileType.path){
        Path path = SpawnPath(tileData);
        paths.Add(path);
      }
      else if (tileData.IsNode()){
        Node node = CreateNode(tileData);
        if (tileData.type == TileType.pathNode){
          paths.Add(node);
        }
        nodes.Add(node);
      }
    }
    return nodes;
  }

  private Path SpawnPath(TileData tileData){
    Vector2Int pos = tileData.GetPosition();
    Path path = Instantiate(pathPrefab, TileVector(pos.x, pos.y), Quaternion.identity, transform);
    path.position = new Vector2Int(pos.x, pos.y);
    pathGrid[pos.x, pos.y] = PathType.path;
    return path;
  }

  private Node CreateNode(TileData tileData){
    Node prefab = tileData.type == TileType.blockingNode ? blockingNodePrefab : nodePrefab;
    Vector2Int pos = tileData.GetPosition();
    Node node = Instantiate(prefab, TileVector(pos.x, pos.y), Quaternion.identity, transform);
    node.position = pos;
    pathGrid[pos.x, pos.y] = tileData.isPath ? PathType.path : PathType.none;
    node.isBlocking = !tileData.isPath;
    return node;
    }

  private List<PathNode> CreatePath(Vector2Int start){
    List<PathNode> path = Pathfinder.FindPath(pathGrid, start, gridEnd);
    foreach(PathNode pathNode in path){
      pathNode.worldPoint = TileVector(pathNode.x, pathNode.y);
    }
    return path;
  }

  private void SetAdjacencies(List<Node> nodes){
    
    foreach(Node node in nodes){
      for(int x = -1; x <= 1; x++) {
        for(int y = -1; y <= 1; y++) {
          if (y == 0 && x == 0){
            continue;
          }
          if (x != 0 && y != 0){
            continue;
          }
          int xx = node.position.x + x;
          if (xx < 0 && xx >= pathGrid.GetLength(0)){
            continue;
          }
          int yy = node.position.y + y;
          if (yy < 0 && yy >= pathGrid.GetLength(1)){
            continue;
          }
          if (pathGrid[xx, yy] == PathType.path){
            node.isAdjacentToPath = true;
          }
        }
      }
    }

  }

  private Vector3 TileVector(int x, int y){
    return new Vector3(x, 0, y) * tileSize + pathOffSet;
  }

  /*========================
   * Public functions
   *========================*/

  public Route GetRoute(Vector2Int point){
    return new Route(CreatePath(point), false);
  }

  public Route GetDefaultRoute(){
    return defaultRoute;
  }

  public void TowerBuilt(Vector2Int pos){
    pathGrid[pos.x, pos.y] = PathType.tower;
    bool oldDefaultIsValid = true;
    if (true || defaultRoute.Contains(pos)){
      defaultRoute = new Route(CreatePath(gridStart), true);
      oldDefaultIsValid = false;
    }
    GameManager.instance.RouteChanged(defaultRoute, oldDefaultIsValid);
  }

  public void TowerSold(Vector2Int pos, bool pathNode){
    pathGrid[pos.x, pos.y] = pathNode ? PathType.path : PathType.none;
    defaultRoute = new Route(CreatePath(gridStart), true);
    GameManager.instance.RouteChanged(defaultRoute, false);
  }


  public Vector3 GetSpawningPoint(){
    return spawningPoint;
  }

  public ITile GetSelectedPath(){
    foreach(ITile path in paths){
      if (path.IsTargeted()){
        return path;
      }
    }
    return null;
  }

  public List<ITile> GetAdjacentPaths(Vector2Int pos){
    List<ITile> list = new List<ITile>();
    foreach(ITile path in paths){
      Vector2Int p = path.GetPosition();
      int xx = pos.x - p.x;
      int yy = pos.y - p.y;
      if ((xx == 0 || yy == 0) && Mathf.Abs(xx + yy) == 1) {
        list.Add(path);
      }
    }
    return list;
  }

}
