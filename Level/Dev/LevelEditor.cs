using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditor : MonoBehaviour
{
  public LevelData levelData;
  public Camera mainCamera;
  public EditorTile tilePrefab;
  public int size = 100;
  public Color emptyColor;
  public Color roadColor;
  public Color nodeColor;
  public Color blockingNodeColor;
  public Color startColor;
  public Color exitColor;
  private TileData[,] tiles;
  private TileType selectedTileType;
  private EditorTile startTile;
  private EditorTile endTile;

  public void Awake(){
    InitGrid();
    LoadTiles();
    selectedTileType = TileType.none;
    mainCamera.transform.position = new Vector3(size /2, size / 2, size /3);
  }

  private void InitGrid(){
    size = levelData.size;
    tiles = new TileData[size, size];

    for(int x = 0; x < size; x++){
      for(int y = 0; y < size; y++){
          tiles[x,y] = new TileData(x,y);
      }
    }
  }

  private void LoadTiles(){
    foreach(TileData tile in levelData.tiles){
      tiles[tile.x, tile.y] = tile;
    }
    tiles[levelData.start.x, levelData.start.y] = levelData.start;
    tiles[levelData.exit.x, levelData.exit.y] = levelData.exit;

    for(int x = 0; x < size; x++){
      for(int y = 0; y < size; y++){
        EditorTile tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
        tile.Init(this, x, y);
        SetTile(tile, tiles[x,y].type);
      }
    }
  }

  void Update(){
    if (Input.GetKeyDown(KeyCode.Alpha1)){
      selectedTileType = TileType.none;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2)){
      selectedTileType = TileType.path;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3)){
      selectedTileType = TileType.pathNode;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha4)){
      selectedTileType = TileType.start;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha5)){
      selectedTileType = TileType.exit;
    }
  }

  public void TileClicked(EditorTile tile){
    TileType newType = selectedTileType;
    TileData tileData = tiles[tile.x, tile.y];
    TileType currentType = tileData.type;

    if (selectedTileType == TileType.pathNode){
      if (currentType == TileType.pathNode ){
        newType = TileType.blockingNode;
      } else {
        newType = TileType.pathNode;
      }
    }
    else if (currentType == selectedTileType){
      return;
    }

    SetTile(tile, newType);
  }

  private void SetTile(EditorTile tile, TileType type){
    TileData data = tiles[tile.x, tile.y];
    data.SetType(type);

    if (data.IsDoor()){
      SetDoor(tile, data);
    } else {
      UpdateList(tile, data);
    }

    Color color = GetTileColor(type);
    tile.SetColor(color);
  }

  private void SetDoor(EditorTile tile, TileData data){
    if (selectedTileType == TileType.exit){
      endTile = tile;
      levelData.exit = data;

    } else if (selectedTileType == TileType.start){
      startTile = tile;
      levelData.start = data;
    }
  }

  private void UpdateList(EditorTile tile, TileData data){
    if (data.type == TileType.none){
      levelData.tiles.Remove(data);
    } else if (!levelData.tiles.Contains(data)) {
      levelData.tiles.Add(data);
    }
  }

  private Color GetTileColor(TileType type){
    if (type == 0){
      return emptyColor;
    } else if (type == TileType.path){
      return roadColor;
    } else if (type == TileType.pathNode){
      return nodeColor;
    } else if (type == TileType.blockingNode){
      return blockingNodeColor;
    } else if (type == TileType.exit){
      return exitColor;
    } else if (type == TileType.start){
      return startColor;
    }
    return Color.magenta;
  }

  private List<TileData> GetTiles(){
    List<TileData> tileList = new List<TileData>();
    for(int x = 0; x < size; x++){
      for(int y = 0; y < size; y++){
        TileData tileData = tiles[x,y];
        if (!tileData.IsEmpty()){
          tileList.Add(tileData);
        }
      }
    }
    return tileList;
  }

}
