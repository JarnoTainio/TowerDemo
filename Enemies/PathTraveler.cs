using UnityEngine;

public class PathTraveler : MonoBehaviour
{
  [Header("Attributes")]
  public float speed;

  [Header("Attributes")]
  private int routeIndex;
  public Route route;
  private Vector3 target;
  private Vector3 targetOffset = new Vector3(0, 0.25f, 0);
  public bool endReached = false;

  // ==========================
  // Unity
  // ==========================

  public void OnDrawGizmosSelected(){
    
    for(int i = 0; i < route.nodeList.Count; i++)
    {
      PathNode node = route.nodeList[i];
      Vector3 v = node.worldPoint + targetOffset;
      Gizmos.color = i < routeIndex ? Color.yellow : Color.green;
      Gizmos.DrawCube(v, new Vector3(0.25f, 0.25f, 0.25f));
    }
  }

  // ==========================
  // Public
  // ==========================

  public void Travel(float deltaTime){
    if (route != null){
      transform.position = Vector3.MoveTowards(transform.position, target, speed * deltaTime);
      if (Vector3.Distance(transform.position, target) < 0.1f){
        routeIndex++;
        if (routeIndex == route.Length()){
          EndReached();
        }else{
          SetTarget();
        }
      }
    } 
  }

  public void DefaultRouteChanged(Route newRoute, bool oldDefaultIsValid){
    Vector2Int pos = route.nodeList[routeIndex].Vector();
    Route oldRoute = route;
    // TODO: Check if route is valid
    if (route.IsValid(newRoute, oldDefaultIsValid)){
      route = GameManager.instance.GetRoute(pos);
    } else {
      route = newRoute;
    }
    routeIndex = route.GetRelativeIndex(oldRoute, routeIndex);
    SetTarget();
  }

  public void SetRoute(Route route){
    this.route = route;
    routeIndex = 0;
    SetTarget();
  }

  public void FindRoute(){
    routeIndex = 0;
    SetTarget();
  }

  public void SetSpeed(float speed){
    this.speed = speed;
  }

   public float GetTraveledDistance(){
    return routeIndex * 100 + Vector3.Distance(transform.position, target);
  }

  // ==========================
  // Private
  // ==========================

  private void SetTarget(){
    target = route.GetWorlPoint(routeIndex) +targetOffset;
  }

  private void EndReached(){
    endReached = true;
  }
}
