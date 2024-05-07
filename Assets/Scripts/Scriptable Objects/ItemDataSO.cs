using System.Text;
using UnityEngine;

public enum ItemType
{
    MATERIAL,
    EQUIPMENT
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/ItemData")]
public class ItemDataSO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    protected StringBuilder sb = new();

    public virtual string GetDescription()
    {
        return "";
    }
}
