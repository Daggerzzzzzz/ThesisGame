using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : SingletonMonoBehavior<Inventory>, ISaveManager
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

    [Header("Database")] 
    private List<ItemDataSO> itemDatabase;
    public List<InventoryItem> loadedItems;
    private List<EquipmentDataSO> equipmentDatabase;
    public List<InventoryItem> loadedEquipments;

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

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.itemDataSo);
                }
            }
        }
        
        if (loadedEquipments.Count > 0)
        {
            foreach (InventoryItem equipment in loadedEquipments)
            {
                AddItem(equipment.itemDataSo);
            }
        }
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
            }
            else
            {
                oldArmor = armor.itemDataSo as ArmorDataSO;
            }
            
            if (oldArmor != null)
            {
                oldArmor.RemoveModifiers();
                inventoryDict.Remove(oldArmor);
                PlayerManager.Instance.player.GetComponent<PlayerDropItem>()?.DropEquipment(oldArmor);
            }
        
            armor = newItem;
            inventoryDict.Add(newArmor, newItem);
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
                inventoryDict.Remove(oldWeapon);
                PlayerManager.Instance.player.GetComponent<PlayerDropItem>()?.DropEquipment(oldWeapon);
            }
        
            weapon = newItem;
            inventoryDict.Add(newWeapon, newItem);
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

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, int> pair in data.stash)
        {
            foreach (var item in GetItemDatabase())
            {
                if (item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item)
                    {
                        stackSize = pair.Value
                    };

                    loadedItems.Add(itemToLoad);
                }
            }
        }
        
        foreach (KeyValuePair<string, int> pair in data.equipment)
        {
            foreach (var equipment in GetEquipmentDatabase())
            {
                if (equipment != null && equipment.itemID == pair.Key)
                {
                    InventoryItem equipmentToLoad = new InventoryItem(equipment)
                    {
                        stackSize = pair.Value
                    };

                    loadedEquipments.Add(equipmentToLoad);
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.stash.Clear();
        data.equipment.Clear();

        foreach (KeyValuePair<ItemDataSO, InventoryItem> pair in stashDict)
        {
            data.stash.Add(pair.Key.itemID, pair.Value.stackSize);
        }
        
        foreach (KeyValuePair<EquipmentDataSO, InventoryItem> pair in inventoryDict)
        {
            data.equipment.Add(pair.Key.itemID, pair.Value.stackSize);
        }
    }

    private List<EquipmentDataSO> GetEquipmentDatabase()
    {
        equipmentDatabase = new List<EquipmentDataSO>();
        string[] equipmentAssetNames = AssetDatabase.FindAssets("", new[] {"Assets/SOData/Equipments"});

        foreach (string SOName in equipmentAssetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var equipmentData = AssetDatabase.LoadAssetAtPath<EquipmentDataSO>(SOpath);
            
            equipmentDatabase.Add(equipmentData);
        }

        return equipmentDatabase;
    }
    
    private List<ItemDataSO> GetItemDatabase()
    {
        itemDatabase = new List<ItemDataSO>();
        string[] itemAssetNames = AssetDatabase.FindAssets("", new[] {"Assets/SOData/Items"});

        foreach (string SOName in itemAssetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemDataSO>(SOpath);
            
            itemDatabase.Add(itemData);
        }

        return itemDatabase;
    }
}
