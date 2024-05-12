using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Material material;
    private EntityStats entityStats;
    private SpriteRenderer sr;
    
    private static readonly int Health = Shader.PropertyToID("_Health");

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        entityStats = GetComponentInParent<EntityStats>();
        material = sr.material;
        
        if (material != null)
        {
            UpdateHealthUI();
        }
        else
        {
            Debug.LogWarning("Material is null. Health UI update skipped.");
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
}