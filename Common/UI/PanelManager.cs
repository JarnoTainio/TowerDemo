using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
  public LevelInputManager inputManager;
  public Image panel;
  public IPanelObject[] panelObjects;
  public IPanelObject activePanelObject;
  private float timeSpeed = 1f;

  public void Open(IPanelObject panelObject){
    if (activePanelObject != null){
      panel.gameObject.SetActive(false);
      activePanelObject = null;
    }

    panel.gameObject.SetActive(true);
    activePanelObject = panelObject;
    inputManager.SetControlsEnabled(false);
    StopGameSpeed();
  }

  public void Close(bool adjustTime = false){
    panel.gameObject.SetActive(false);
    activePanelObject = null;
    inputManager.SetControlsEnabled(true);
    RestoreGameSpeed();
  }

  private void StopGameSpeed(){
    timeSpeed = Time.timeScale;
    GameManager.instance.SetGameSpeed(0f);
  }

  private void RestoreGameSpeed(){
    GameManager.instance.SetGameSpeed(timeSpeed);
  }
}
