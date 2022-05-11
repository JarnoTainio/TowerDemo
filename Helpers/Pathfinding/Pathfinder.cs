using System.Collections.Generic;
using UnityEngine;
public class Pathfinder
{
  public static List<PathNode> FindPath(PathType[,] grid, Vector2Int start, Vector2Int end){
    PathNode currentNode = new PathNode(start.x, start.y, end);

    List<PathNode> openList = new List<PathNode>();
    openList.Add(currentNode);

    Dictionary<Vector2Int, PathNode> dictionary = new Dictionary<Vector2Int, PathNode>();
    dictionary.Add(currentNode.Vector(), currentNode);

    int width = grid.GetLength(0);
    int height = grid.GetLength(1);
    
    int i = 0;
    while (openList.Count > 0){
      i++;
      openList.Sort((a, b) => (a.distance + a.cost).CompareTo((b.distance + b.cost)) );
      currentNode = openList[0];
      openList.Remove(currentNode);

      if (currentNode.Vector() == end){
        break;
      }

      for(int x = -1; x <= 1; x++){
        for(int y = -1; y <= 1; y++){
          /// Diagonal
          if (x != 0 && y != 0){
            continue;
          }
          
          // Width
          int xx = currentNode.x + x;
          if (xx < 0 || xx >= width){
            continue;
          }
          
          // Height
          int yy = currentNode.y + y;
          if (yy < 0 || yy >= height){
            continue;
          }

          if (grid[xx, yy] == PathType.none){
            continue;
          }

          int stepCost = grid[xx, yy] == PathType.path ? 1 : 10000;
          int totalCost = currentNode.cost + stepCost;

          PathNode node = null;
          Vector2Int pos = new Vector2Int(xx, yy);

          // Visited before
          if (dictionary.ContainsKey(pos)){
            node = dictionary[pos];
            if (node.cost > totalCost){
              node.cost = totalCost;
              node.parent = currentNode;
              if (!openList.Contains(node)){
                openList.Add(node);
              }
            }

          // New node
          }else{
            node = new PathNode(xx, yy, end, totalCost, currentNode);
            dictionary.Add(node.Vector(), node);
            openList.Add(node);
          }
        }
      }

      if (i > 500){
        throw new System.Exception("Too long route!");
      }
    }
    return currentNode.ToList();
  }
}
