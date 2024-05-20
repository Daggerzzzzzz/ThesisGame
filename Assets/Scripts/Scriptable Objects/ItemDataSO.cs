using System.Text;
using UnityEditor;
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
    public string itemID;

    protected StringBuilder sb = new();

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return "";
    }
}
