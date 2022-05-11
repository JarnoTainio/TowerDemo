using UnityEngine;


[CreateAssetMenu(fileName = "RuneAttribute", menuName = "TowerDefence/Rune/Attribute", order = 1)]
public class RuneAttribute : Rune
{
  public AttributeValue[] attributeValues;

  public override string GetDescription(){
    string str = "";
    foreach(AttributeValue av in attributeValues){
      if (str.Length > 0){
        str += "\n";
      }
      str += av.ToString();
    }
    return str;
  }

  public override float GetAttributeModifier(Attribute attribute){ 
    float value = 0f;
    foreach(AttributeValue av in attributeValues){
      if (av.attribute == attribute){
        value += av.value;
      }
    }
    return value;
  }

}
