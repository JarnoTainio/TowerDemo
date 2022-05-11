using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour, IPanelObject
{
  public PanelManager panelManager;
  public TowerUI towerUI;
  public TranslatedText gameOverTitle;

  public TranslatedText nextLevelText;
  public Button continueButton;

  public void Open(bool victory){
    Open();
    if (victory){
      gameOverTitle.SetKey("victory");
      nextLevelText.SetKey("next_level");
      continueButton.onClick.AddListener(GameManager.instance.NextLevel);
    } else {
      gameOverTitle.SetKey("defeated");
      nextLevelText.SetKey("new_game");
      continueButton.onClick.AddListener(GameManager.instance.NewGame);
    }
  }

  public void Open(){
    panelManager.Open(this);
    gameObject.SetActive(true);
    towerUI.Close();
  }

  public void Close(){}
}
