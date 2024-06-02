using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

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
    public List<ItemDataSO> itemDatabase;
    public List<InventoryItem> loadedItems;
    private List<EquipmentDataSO> equipmentDatabase;
    public List<InventoryItem> loadedEquipments;

    [Header("Skill Effects")] 
    public bool firstTimeUsed = true;
    private float lastTimeUsedPotion;
    private float lastTimeUsedArmor;
    private float lastTimeUsedWeapon;

    private ItemSlot[] stashItemSlots;
    private ItemSlot[] inventoryItemSlots;
    private WeaponSlot weaponSlot;
    private ArmorSlot armorSlot;
    private StatSlot[] statSlots;
    
    public UnityEvent<int> onPotionUsed;
    public UnityEvent<int> numberOfPotionInStack;
    public UnityEvent<int> guardianAngelUsed;
    public UnityEvent keyCheck;
    private int potionCount;

    protected override void Awake()
    {
        base.Awake();
        
        stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlot>();
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlot>();
        weaponSlot = equipmentSlotParent.GetComponentInChildren<WeaponSlot>();
        armorSlot = equipmentSlotParent.GetComponentInChildren<ArmorSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<StatSlot>();
    }

    private void Start()
    {
        stash = new List<InventoryItem>();
        stashDict = new Dictionary<ItemDataSO, InventoryItem>();
        
        inventory = new List<InventoryItem>();
        inventoryDict = new Dictionary<EquipmentDataSO, InventoryItem>();
        
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
        
        potionCount = GetStackValue("Potion");
        numberOfPotionInStack?.Invoke(potionCount);
    }

    private void RemoveItem(ItemDataSO item)
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

        UpdateUISlot();
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

        UIStatSlotUpdate();
    }

    public void UIStatSlotUpdate()
    {
        foreach (var stat in statSlots)
        {
            stat.UpdateStatValue();
        }
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, int> pair in data.stash)
        {
            foreach (var item in itemDatabase)
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
            foreach (var equipment in itemDatabase)
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
    
#if UNITY_EDITOR
    [ContextMenu("Fill Up Equipment Database")]
    private void FillUpEquipmentDatabase() => equipmentDatabase = new List<EquipmentDataSO>(GetEquipmentDatabase());
    
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
    
    [ContextMenu("Fill Up Item Database")]
    private void FillUpItemDatabase() => itemDatabase = new List<ItemDataSO>(GetItemDatabase());
    
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
#endif

    public EquipmentDataSO GetEquipment(EquipmentType type)
    {
        EquipmentDataSO equippedEquipment = null;

        foreach (KeyValuePair<EquipmentDataSO, InventoryItem> equipment in  inventoryDict)
        {
            if (equipment.Key.equipmentType == type)
            {
                equippedEquipment = equipment.Key;
            }
        }

        return equippedEquipment;
    }

    private ItemDataSO GetItemInStash(string name)
    {
        ItemDataSO itemToGet = null;
        
        foreach (KeyValuePair<ItemDataSO, InventoryItem> item in stashDict)
        {
            if (item.Key.itemName == name)
            {
                itemToGet = item.Key;
            }
        }

        return itemToGet;
    }

    public int GetStackValue(string name)
    {
        int stackValue = 0;
        
        foreach (KeyValuePair<ItemDataSO, InventoryItem> item in stashDict)
        {
            if (item.Key.itemName == name)
            {
                stackValue = item.Value.stackSize;
            }
        }

        return stackValue;
    }
    

    public void UsePotion()
    {
        ItemDataSO currentPotion = GetItemInStash("Potion");
        
        if (currentPotion == null)
        {
            SoundManager.Instance.PlaySoundEffects(32, null, false);
            PlayerManager.Instance.player.OnEntityFx.CreateInformationText("No Potions");
            return;
        }
        
        bool canUsePotion = Time.time > lastTimeUsedPotion + currentPotion.itemCooldown;
        
        if (canUsePotion)
        {
            SoundManager.Instance.PlaySoundEffects(11, null, true);
            currentPotion.UseEffect(Vector2.zero, PlayerManager.Instance.player.GetComponent<PlayerStats>());
            RemoveItem(currentPotion);
            potionCount = GetStackValue("Potion");
            lastTimeUsedPotion = Time.time;
            onPotionUsed?.Invoke(currentPotion.itemCooldown);
            numberOfPotionInStack?.Invoke(potionCount);
        }
        else
        {
            PlayerManager.Instance.player.OnEntityFx.CreateInformationText("In Cooldown");
            SoundManager.Instance.PlaySoundEffects(32, null, false);
            Debug.Log("Potion is on Cooldown");
        }
    }

    public bool UseArmor()
    {
        EquipmentDataSO currentArmor = GetEquipment(EquipmentType.ARMOR);

        if (Time.time > lastTimeUsedArmor + currentArmor.itemCooldown || firstTimeUsed)
        {
            lastTimeUsedArmor = Time.time;
            guardianAngelUsed?.Invoke(currentArmor.itemCooldown);
            return true;
        }
        
        Debug.Log("Armor on Cooldown");
        return false;
    }
    
    public bool UseWeapon()
    {
        EquipmentDataSO currentWeapon = GetEquipment(EquipmentType.WEAPON);

        if (Time.time > lastTimeUsedWeapon + currentWeapon.itemCooldown)
        {
            lastTimeUsedWeapon = Time.time;
            return true;
        }
        
        Debug.Log("Weapon on Cooldown");
        return false;
    }

    public bool CheckForKey()
    {
        ItemDataSO currentKey = GetItemInStash("Key");
        
        if (currentKey == null)
        {
            return false;
        }

        return true;
    }
}