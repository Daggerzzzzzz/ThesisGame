using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] 
    private Transform parentTransform;
    
    private ItemObject itemObject;
    
    private bool canEquip;

    private void Start()
    {
        itemObject = GetComponentInParent<ItemObject>();
        parentTransform = itemObject.GetComponentInParent<Transform>();
    }

    private void Update()
    {
        if (canEquip)
        {
            switch (itemObject.ItemDataSo.itemType)
            {
                case ItemType.EQUIPMENT:
                {
                    if (PlayerManager.Instance.player.OnPlayerInputs.Player.Equip.IsPressed())
                    {
                        itemObject.ItemPickup();
                    }
                    break;
                }
                case ItemType.MATERIAL:
                    itemObject.ItemPickup();
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canEquip = true;
            Player player = other.GetComponent<Player>();
            player.eKey.SetActive(true);
            player.equipmentInfo.SetActive(true);

            if (parentTransform.gameObject.name == "Sword")
            {
                WeaponDataSO weaponDataSo = itemObject.ItemDataSo as WeaponDataSO;
                player.itemTooltip.ShowWeaponTooltip(weaponDataSo);
            }

            else if (parentTransform.gameObject.name == "Armor")
            { 
                ArmorDataSO armorDataSo = itemObject.ItemDataSo as ArmorDataSO;
                player.itemTooltip.ShowArmorTooltip(armorDataSo);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canEquip = false;
        PlayerManager.Instance.player.eKey.SetActive(false);
        PlayerManager.Instance.player.equipmentInfo.SetActive(false);
    }
}
