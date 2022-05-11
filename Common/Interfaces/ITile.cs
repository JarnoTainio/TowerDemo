using UnityEngine;

public interface ITile 
{
  Vector2Int GetPosition();

  bool Equals(ITile path);

  bool IsTargeted();

  void Targeting();

  Transform GetTransform();
}
