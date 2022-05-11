using UnityEngine;

public class TimeManager : MonoBehaviour, IOptionGroupManager
{
  public int startingIndex;
  public float[] speedOptions;
  public OptionButton[] options;
  public Color selectedColor;
   
  void Start(){
    for(int i = 0; i < options.Length; i++){
      OptionButton option = options[i];
      option.Init(this, i);
    }
    UpdateState(startingIndex);
  }

  public void SetSpeed(int index){
    GameManager.instance.SetGameSpeed(speedOptions[index]);
  }

  public void OptionSelected(int index){
    SetSpeed(index);
    UpdateState(index);
  }

  public Color GetSelectedColor(){
    return selectedColor;
  }

  private void UpdateState(int index){
    for(int i = 0; i < options.Length; i++){
      OptionButton option = options[i];
      option.SetSelected(index);
    }
  }
}
