using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType
{
    MATERIAL,
    EQUIPMENT
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [TextArea] 
    public string itemEffectDesc;
    
    public ItemType itemType;
    public Sprite icon;
    public string itemName;
    public string itemID;
    
    public int itemCooldown;
    [Range(0, 1)]
    public float dropChance;

    protected StringBuilder sb = new();
    protected int minDescLength;
    public ItemEffectSO[] itemEffects;

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        sb.Length = 0;
        minDescLength = 0;
        
        if (minDescLength < 1)
        {
            for (int i = 0; i < 1 - minDescLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        
        if (itemEffectDesc.Length > 0)
        {
            sb.Append(itemEffectDesc); 
        }
        
        return sb.ToString();
    }
    
    public void UseEffect(Vector2 spawnPosition, EntityStats entityStats)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(spawnPosition, entityStats);
        }
    }
}
