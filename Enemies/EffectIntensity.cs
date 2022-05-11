using UnityEngine;

public class EffectIntensity : MonoBehaviour
{
  public ParticleSystem effect;
  public float defaulIntensity = 1;
  public float intensityMultiplyer = 1f;

  public void SetIntensity(float value){
    ParticleSystem.EmissionModule emission = effect.emission;
    emission.rateOverTime = intensityMultiplyer * (value / defaulIntensity);
  }
  

}
