using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonMonoBehavior<Inventory>
{
    public List<InventoryItem> inventory;
    public List<InventoryItem> stash;
    public InventoryItem weapon;
    public InventoryItem armor;

    private Dictionary<EquipmentDataSO, InventoryItem> inventoryDict;
    private Dictionary<ItemDataSO, InventoryItem> stashDict;
    private (WeaponDataSO, InventoryItem) weaponInfo;
    private (ArmorDataSO, InventoryItem) armorInfo;

    [Header("Inventory UI")] 
    [SerializeField]
    private Transform stashSlotParent;
    [SerializeField]
    private Transform inventorySlotParent;
    [SerializeField]
    private Transform equipmentSlotParent;
    [SerializeField]
    private Transform statSlotParent;

    private ItemSlot[] stashItemSlots;
    private ItemSlot[] inventoryItemSlots;
    private WeaponSlot weaponSlot;
    private ArmorSlot armorSlot;
    private StatSlot[] statSlots;
    
    private void Start()
    {
        stash = new List<InventoryItem>();
        stashDict = new Dictionary<ItemDataSO, InventoryItem>();
        
        inventory = new List<InventoryItem>();
        inventoryDict = new Dictionary<EquipmentDataSO, InventoryItem>();
        
        stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlot>();
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlot>();
        weaponSlot = equipmentSlotParent.GetComponentInChildren<WeaponSlot>();
        armorSlot = equipmentSlotParent.GetComponentInChildren<ArmorSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<StatSlot>();
    }

    public void AddItem(ItemDataSO item)
    {
        if (item.itemType == ItemType.EQUIPMENT)
        {
            AddToInventory(item);
        }
        else if (item.itemType == ItemType.MATERIAL)
        {
            AddToStash(item);
        }
        UpdateUISlot();
    }

    private void AddToInventory(ItemDataSO item)
    {
        EquipmentDataSO newEquipment = item as EquipmentDataSO;
        InventoryItem newItem = new InventoryItem(item);
        
        if (newEquipment != null && newEquipment.equipmentType == EquipmentType.ARMOR)
        {
            ArmorDataSO newArmor = newEquipment as ArmorDataSO;
            ArmorDataSO oldArmor = null;
            
            if (armor.itemDataSo == null )
            {
                armor = newItem;
                newItem.stackSize++;
            }
            else
            {
                oldArmor = armor.itemDataSo as ArmorDataSO;
            }
            
            if (oldArmor != null)
            {
                oldArmor.RemoveModifiers();
                PlayerManager.Instance.player.GetComponent<PlayerDropItem>()?.DropEquipment(oldArmor);
            }
        
            armor = newItem;
            newArmor.AddModifiers();
        }
        else if (newEquipment != null && newEquipment.equipmentType == EquipmentType.WEAPON)
        {
            WeaponDataSO newWeapon = newEquipment as WeaponDataSO;
            WeaponDataSO oldWeapon = null;
            
            if (weapon.itemDataSo == null )
            {
                weapon = newItem;
            }
            else
            {
                oldWeapon = weapon.itemDataSo as WeaponDataSO;
            }
            
            if (oldWeapon != null)
            {
                oldWeapon.RemoveModifiers();
                PlayerManager.Instance.player.GetComponent<PlayerDropItem>()?.DropEquipment(oldWeapon);
            }
        
            weapon = newItem;
            newWeapon.AddModifiers();
        }
    }
    
    private void AddToStash(ItemDataSO item)
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
    }

    public void RemoveItem(ItemDataSO item)
    {
        if (stashDict.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                stash.Remove(value);
                stashDict.Remove(item);
            }

            else
            {
                value.RemoveToStack();
            }
        }
    }
    
    private void UpdateUISlot()
    {
        if (weapon.itemDataSo != null)
        {
            weaponSlot.UpdateSlot(weapon);
        }
        
        if (armor.itemDataSo != null)
        {
            armorSlot.UpdateSlot(armor);
        }
        
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }
        
        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }
        
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }
        
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValue();
        }
    }
}
