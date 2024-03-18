using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    private SpriteRenderer sr;
    
    [Header("Flash Effects")] 
    [SerializeField]
    private Material hitEffect;
    
    private Material spriteLitDefault;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        spriteLitDefault = sr.material;
    }

    private IEnumerator FlashEffects()
    {
        sr.material = hitEffect;
        yield return new WaitForSeconds(.2f);
        sr.material = spriteLitDefault;
    }
}
