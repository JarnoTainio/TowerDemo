public struct UpgradeStatus{
  public string strKey;
  public string oldValue;
  public string newValue;
  public bool isUpgrade;
  public bool hasValue;

  public UpgradeStatus(string strKey, float oldValue = 0, float newValue = 0, bool growingValue = true){
    this.strKey = strKey;
    this.oldValue = FormatHelper.Float(oldValue);
    this.newValue = FormatHelper.Float(newValue);
    isUpgrade = growingValue ? oldValue < newValue : oldValue > newValue;
    hasValue = true;
  }

  public UpgradeStatus(string strKey){
    this.strKey = strKey;
    this.oldValue = "";
    this.newValue = "";
    isUpgrade = false;
    hasValue = false;
  }
}