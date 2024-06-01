using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    private ItemObject itemObject;
    private Player player;

    private bool canEquip;
    
    private void Start()
    {
        itemObject = GetComponentInParent<ItemObject>();
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
                        SoundManager.Instance.PlaySoundEffects(22, null, true);
                        itemObject.ItemPickup();
                    }
                    break;
                }
                case ItemType.MATERIAL:
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemObject.ItemDataSo.itemType == ItemType.MATERIAL)
            {
                SoundManager.Instance.PlaySoundEffects(12, null, true);
                itemObject.ItemPickup();
                return;
            }
            
            SoundManager.Instance.PlaySoundEffects(29, null, true);
            SoundManager.Instance.StopSoundEffects(28);
            canEquip = true;
            player = other.GetComponent<Player>();
            player.eKey.SetActive(true);
            player.equipmentInfo.SetActive(true);
            EquipmentDataSO equipmentDataSo = itemObject.ItemDataSo as EquipmentDataSO;

            if (equipmentDataSo.equipmentType == EquipmentType.WEAPON)
            {
                WeaponDataSO weaponDataSo = itemObject.ItemDataSo as WeaponDataSO;
                player.itemTooltip.ShowWeaponTooltip(weaponDataSo);
            }

            else if (equipmentDataSo.equipmentType == EquipmentType.ARMOR)
            {
                ArmorDataSO armorDataSo = itemObject.ItemDataSo as ArmorDataSO;
                player.itemTooltip.ShowArmorTooltip(armorDataSo);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemObject.ItemDataSo.itemType == ItemType.EQUIPMENT)
            {
                SoundManager.Instance.PlaySoundEffects(28, null, true);
            }

            canEquip = false;
            player = other.GetComponent<Player>();

            player.eKey.SetActive(false);
            player.equipmentInfo.SetActive(false);

        }
    }
}
