using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterializeEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public IEnumerator MaterializeRoutine(float materializeTime, Material normalMaterial)
    {
        float dissolveAmount = 0f;
        
        while (dissolveAmount < 1.2f)
        {
            dissolveAmount += Time.deltaTime / materializeTime;
            
            spriteRenderer.material.SetFloat(DissolveAmount, dissolveAmount);

            yield return null;
        }
        
        spriteRenderer.material = normalMaterial;
    }
}
