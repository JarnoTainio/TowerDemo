using UnityEngine;

public class DialogManager : MonoBehaviour, IPanelObject
{
  public PanelManager panelManager;
  public GameObject dialogPanel;
  public bool stopTime;
  
  public void SetOpen(bool isOpen){
    if (!stopTime){
      return;
    }
    gameObject.SetActive(isOpen);
  }

  public void Open(){
    SetOpen(true);
    panelManager.Open(this);
  }

  public void Close(){
    SetOpen(false);
    panelManager.Close();
  }
}
