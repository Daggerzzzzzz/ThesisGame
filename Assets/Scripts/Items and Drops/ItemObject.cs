using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] 
    private ItemDataSO itemDataSo;

    private void OnValidate()
    {
        if (itemDataSo == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemDataSo.icon;
        gameObject.name = itemDataSo.name;
    }

    public void SetupItem(ItemDataSO itemDataSo)
    {
        this.itemDataSo = itemDataSo;
    }

    public void ItemPickup()
    {
        Inventory.Instance.AddItem(itemDataSo);
        Destroy(gameObject);
    }
}
