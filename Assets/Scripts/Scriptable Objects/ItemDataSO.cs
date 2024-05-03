using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    protected StringBuilder sb = new();

    public virtual string GetDescription()
    {
        return "";
    }
}
