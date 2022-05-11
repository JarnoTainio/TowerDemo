using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "TowerDefence/Level")]
public class LevelData : ScriptableObject
{
  public int size = 30;
  public int lives = 20;
  public int resources = 100;
  public List<Wave> waves;
  public TileData start;
  public TileData exit;
  public List<TileData> tiles;
}
