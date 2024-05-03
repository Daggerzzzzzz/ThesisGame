using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    private ItemObject itemObject;

    private void Start()
    {
        itemObject = GetComponentInParent<ItemObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            itemObject.ItemPickup();
        }
    }
}
