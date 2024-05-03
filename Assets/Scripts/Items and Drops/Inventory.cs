using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonMonoBehavior<Inventory>
{
    public List<InventoryItem> stash;
    public Dictionary<ItemDataSO, InventoryItem> stashDict;

    [Header("Inventory UI")] 
    [SerializeField]
    private Transform stashSlotParent;
    [SerializeField]
    private Transform statSlotParent;

    private ItemSlot[] stashItemSlots;
    private StatSlot[] statSlots;
    
    private void Start()
    {
        stash = new List<InventoryItem>();
        stashDict = new Dictionary<ItemDataSO, InventoryItem>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlot>();
        statSlots = stashSlotParent.GetComponentsInChildren<StatSlot>();
    }

    public void AddItem(ItemDataSO item)
    {
        if (stashDict.TryGetValue(item, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashDict.Add(item, newItem);
        }
        UpdateUISlot();
    }

    public void RemoveItem(ItemDataSO item)
    {
        if (!stashDict.TryGetValue(item, out InventoryItem value))
        {
            return;
        }
        
        if (value.stackSize <= 1)
        {
            stash.Remove(value);
            stashDict.Remove(item);
        }
        
        else
        {
            value.RemoveToStack();
        }
        UpdateUISlot(); 
    }

    private void UpdateUISlot()
    {
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }

        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValue();
        }
    }
}
