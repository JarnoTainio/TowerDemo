using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraController : MonoBehaviour
{
  public Vector3[] locations;
  public int currentIndex;

  private Vector3 startingPosition;

  void Start(){
    startingPosition = transform.position;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))  {
      currentIndex++;
      if (currentIndex >= locations.Length){
        currentIndex = 0;
      }
      transform.position = startingPosition + locations[currentIndex];
    }
  }
}
