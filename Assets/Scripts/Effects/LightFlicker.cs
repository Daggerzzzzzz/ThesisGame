using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class LightFlicker : MonoBehaviour
{
   [SerializeField] 
   private float lightIntensityMin;
   [SerializeField] 
   private float lightIntensityMax;
   [SerializeField] 
   private float lightFlickerTimeMin;
   [SerializeField] 
   private float lightFlickerTimeMax;
   
   private Light2D light2D;
   private float lightFlickerTimer;

   private void Awake()
   {
      light2D = GetComponentInChildren<Light2D>();
   }

   private void Start()
   {
      lightFlickerTimer = Random.Range(lightFlickerTimeMin, lightFlickerTimeMax);
   }

   private void Update()
   {
      if (light2D == null)
      {
         return;
      }

      lightFlickerTimer -= Time.deltaTime;

      if (lightFlickerTimer < 0f)
      {
         lightFlickerTimer = Random.Range(lightFlickerTimeMin, lightFlickerTimeMax);

         RandomizeLightIntensity();
      }
   }

   private void RandomizeLightIntensity()
   {
      light2D.intensity = Random.Range(lightIntensityMin, lightIntensityMax);
   }
}
