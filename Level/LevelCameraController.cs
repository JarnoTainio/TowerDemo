using UnityEngine;

public class LevelCameraController : MonoBehaviour
{
  public float speed = 5;
  public float scrollSpeed = 2;
  public Vector2 horizontalBoundaries;
  public Vector2 verticalBoundaries;
  public Vector2 zoomBoundaries;
  public static bool HasDeltaMoved;
  private Vector3 lastMousePosition;
  public static bool movementEnabled;

  void Update()
  {
    if (!movementEnabled){
      return;
    }
    Vector3 dir = new Vector3();
    dir += MouseMovement();
    dir += KeyboardMovement();
    dir += MouseScroll();

    MoveCamera(dir);
  }

  private void MoveCamera(Vector3 dir){
    if (dir != Vector3.zero){
      Vector3 pos = transform.position + dir * Time.unscaledDeltaTime;
      float x = Mathf.Clamp(pos.x, horizontalBoundaries.x, horizontalBoundaries.y);
      float y = Mathf.Clamp(pos.y, zoomBoundaries.x, zoomBoundaries.y);
      float z = Mathf.Clamp(pos.z, verticalBoundaries.x, verticalBoundaries.y);
      transform.position = new Vector3(x, y, z);
      HasDeltaMoved = true;
    } else {
      HasDeltaMoved = false;
    }
  }

  private Vector3 KeyboardMovement(){
    Vector3 dir = new Vector3();
    if (Input.GetKey(KeyCode.W)){
      dir += new Vector3(0, 0, speed);
    } else if (Input.GetKey(KeyCode.S)){
      dir += new Vector3(0, 0, -speed);
    }

    if (Input.GetKey(KeyCode.A)){
      dir += new Vector3(-speed, 0, 0);
    } else if (Input.GetKey(KeyCode.D)){
      dir += new Vector3(speed, 0, 0);
    }
    return dir;
  }

  private Vector3 MouseMovement(){
    Vector3 dir = new Vector3();
    if (Input.GetMouseButtonUp(1)){
      lastMousePosition = Vector3.zero;
    }
    else if (Input.GetMouseButtonDown(1)){
      lastMousePosition = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
    }
    else if (Input.GetMouseButton(1)){
      Vector3 newMousePosition = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
      dir = lastMousePosition - newMousePosition;
      lastMousePosition = newMousePosition;
    }
    return dir;
  }

  private Vector3 MouseScroll(){
    Vector3 dir = new Vector3();
    Vector2 scroll = Input.mouseScrollDelta;
    if (scroll != Vector2.zero){
      dir += new Vector3(0, -scroll.y * scrollSpeed, 0);
    }
    return dir;
  }
}
