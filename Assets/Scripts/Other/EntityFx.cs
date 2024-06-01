using System;
using System.Collections;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Color color;
    
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
    
    [Header("Ailments Particles")] 
    [SerializeField]
    private ParticleSystem burnEffect;
    [SerializeField]
    private ParticleSystem freezeEffect;
    [SerializeField]
    private ParticleSystem shockEffect;
    
    private const float tolerance = 0.0001f;
    private HealthBar healthBar;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private IEnumerator FlashEffects()
    {
        sr.material = hitEffect;
        Color currentColor = sr.color;
        
        sr.color = Color.white;
        
        yield return new WaitForSeconds(.2f);
        color = currentColor;
        sr.material = spriteLitDefault;
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        color = Color.white;
        
        burnEffect.Stop();
        freezeEffect.Stop();
        shockEffect.Stop();
    }

    private void CancelAlphaChange()
    {
        CancelColorChange();
        color.a = 1f;
        sr.color = color;
    }

    private void InvincibleState()
    {
        color.a = Math.Abs(color.a - 0.5f) > tolerance ?  color.a = 0.5f :  color.a = 1f;
        sr.color = color;
    }

    public void InvincibleEffect(float seconds)
    {
        InvokeRepeating(nameof(InvincibleState), 0, .1f);
        Invoke(nameof(CancelAlphaChange), seconds);
    }

    public void BurnFx(float seconds)
    {
        burnEffect.Play();
        
        InvokeRepeating(nameof(BurnColorFx), 0, .2f);
        Invoke(nameof(CancelColorChange), seconds);
    }
    
    public void FreezeFx(float seconds)
    {
        freezeEffect.Play();
        
        InvokeRepeating(nameof(FreezeColorFx), 0, .2f);
        Invoke(nameof(CancelColorChange), seconds);
    }
    
    public void ShockFx(float seconds)
    {
        shockEffect.Play();
        
        InvokeRepeating(nameof(ShockColorFx), 0, .2f);
        Invoke(nameof(CancelColorChange), seconds);
    }

    private void BurnColorFx()
    {
        color = color != burnColor[0] ? burnColor[0] : burnColor[1];
    }

    private void FreezeColorFx()
    {
        color = color != freezeColor[0] ? freezeColor[0] : freezeColor[1];
    }
    
    private void ShockColorFx()
    {
        color = color != shockColor[0] ? shockColor[0] : shockColor[1];
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
        {
            healthBar.DisableSpriteRenderer();
            sr.color = Color.clear;
        }
        else
        {
            healthBar.EnableSpriteRenderer();
            sr.color = Color.white;
        }
    }
}
