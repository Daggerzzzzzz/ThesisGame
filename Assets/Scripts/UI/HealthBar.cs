using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Material material;
    private SpriteRenderer sr;
    private EntityStats entityStats;
    
    private static readonly int Health = Shader.PropertyToID("_Health");
    
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        
        if (sr != null)
        {
            material = sr.material; 
        }
        
        entityStats = GetComponentInParent<EntityStats>();
        
        if (material != null)
        {
            UpdateHealthUI();
        }
    }
    
    public void UpdateHealthUI()
    {
        int maxHealth = entityStats.CalculateMaxHealthValue();
        int currentHealth = entityStats.currentHealth;
        
        float normalizedHealth = (float)currentHealth / maxHealth;
        
        material.SetFloat(Health, normalizedHealth);
    }

    public void DisableSpriteRenderer()
    {
        sr.enabled = false;
    }
    
    public void EnableSpriteRenderer()
    {
        sr.enabled = true;
    }
}