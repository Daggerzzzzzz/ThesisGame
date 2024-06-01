using System.Collections;
using Cinemachine;
using UnityEngine;

public class HitStopEffect : MonoBehaviour
{
    private float speed;
    private bool restoreTime;
    
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }
    }

    public void Shake(Vector2 direction, float shakeForce)
    {
        impulseSource.GenerateImpulseWithVelocity(direction * shakeForce);
    }

    public void StopTime(float changeTime, int restoreSpeed, float delay)
    {
        speed = restoreSpeed;

        if (delay > 0)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            restoreTime = true;
        }

        Time.timeScale = changeTime;
    }

    private IEnumerator StartTimeAgain(float amount)
    {
        yield return new WaitForSecondsRealtime(amount);
        restoreTime = true;
    }
}