using System.Collections;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    [SerializeField] 
    private float shockWaveTime = 0.75f;
    
    private Material material;
    private static int _waveDistance = Shader.PropertyToID("_WaveDistance");
    
    
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key was pressed");
            CallShockWave();
        }
    }

    public void CallShockWave()
    {
        StartCoroutine(ShockWaveAction(-0.1f, 1f));
    }

    private IEnumerator ShockWaveAction(float startPos, float endPos)
    {
        material.SetFloat(_waveDistance, startPos);
        float lerpedAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < shockWaveTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPos, endPos, (elapsedTime / shockWaveTime));
            material.SetFloat(_waveDistance, lerpedAmount);
            yield return null;
        }
    }
}
