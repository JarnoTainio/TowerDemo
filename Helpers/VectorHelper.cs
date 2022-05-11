using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorHelper
{
  public static float GetFacingLevel(Vector3 looker, Vector3 facing, Vector3 target){
    return Vector3.Dot((target - looker).normalized, facing);
  }
}
