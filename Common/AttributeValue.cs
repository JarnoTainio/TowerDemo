[System.Serializable]
public class AttributeValue
{
  public Attribute attribute;
  public float value;

  public override string ToString()
  {
    switch (attribute){
      case Attribute.reloadSpeed:
        return Translation.GetTranslation("attribute_reloadspeed", ToString(value));
      case Attribute.range:
        return Translation.GetTranslation("attribute_range", ToString(value));
      case Attribute.damage:
        return Translation.GetTranslation("attribute_damage", ToString(value));
      case Attribute.cost:
        return Translation.GetTranslation("attribute_cost", ToString(value));
      case Attribute.costIncrease:
        return Translation.GetTranslation("attribute_costincrease", ToString(value));
      case Attribute.sellValue:
        return Translation.GetTranslation("attribute_sellvalue");
      case Attribute.buildingCooldown:
        return Translation.GetTranslation("attribute_buildingcooldown");
    }
    return $"{attribute} {value}";
  }

  private string ToString(float value){
    string str = value > 0 ? "+" : "";
    str += $"{(int)(value * 100)}%";
    return str;
  }
}
