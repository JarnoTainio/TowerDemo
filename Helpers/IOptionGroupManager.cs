using UnityEngine;

public interface IOptionGroupManager
{
  void OptionSelected(int index);

  Color GetSelectedColor();
}
