[System.Serializable]
public class WaveEntry{
  public EnemyTag enemyTag = EnemyTag.none;
  public Tier tier = 0;
  public float delay = 1f;

  public override string ToString()
  {
    return $"{enemyTag} {tier} {delay}s";
  }
}
