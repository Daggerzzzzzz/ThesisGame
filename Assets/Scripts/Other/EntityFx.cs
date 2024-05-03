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
    [SerializeField]
    private Material spriteLitDefault;

    [Header("Status Effects Colors")] 
    [SerializeField]
    private Color[] freezeColor;
    [SerializeField] 
    private Color[] burnColor;
    [SerializeField] 
    private Color[] shockColor;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private IEnumerator FlashEffects()
    {
        sr.material = hitEffect;
        Color currentColor = sr.color;
        
        sr.color = Color.white;
        
        yield return new WaitForSeconds(.2f);
        sr.color = currentColor;
        sr.material = spriteLitDefault;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void BurnFx(float seconds)
    {
        InvokeRepeating(nameof(BurnColorFx), 0, .2f);
        Invoke(nameof(CancelColorChange), seconds);
    }
    
    public void FreezeFx(float seconds)
    {
        InvokeRepeating(nameof(FreezeColorFx), 0, .2f);
        Invoke(nameof(CancelColorChange), seconds);
    }
    
    public void ShockFx(float seconds)
    {
        InvokeRepeating(nameof(ShockColorFx), 0, .2f);
        Invoke(nameof(CancelColorChange), seconds);
    }

    private void BurnColorFx()
    {
        sr.color = sr.color != burnColor[0] ? burnColor[0] : burnColor[1];
    }

    private void FreezeColorFx()
    {
        sr.color = sr.color != freezeColor[0] ? freezeColor[0] : freezeColor[1];
    }
    
    private void ShockColorFx()
    {
        sr.color = sr.color != shockColor[0] ? shockColor[0] : shockColor[1];
    }
}
