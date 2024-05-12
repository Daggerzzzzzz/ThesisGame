using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;

    private ItemObject itemObject;
    private Player player;

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
            player = other.GetComponent<Player>();
            player.eKey.SetActive(true);
            player.equipmentInfo.SetActive(true);

            if (parentTransform.gameObject.name == "weapon")
            {
                WeaponDataSO weaponDataSo = itemObject.ItemDataSo as WeaponDataSO;
                player.itemTooltip.ShowWeaponTooltip(weaponDataSo);
            }

            else if (parentTransform.gameObject.name == "armor")
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
            canEquip = false;
            player = other.GetComponent<Player>();

            player.eKey.SetActive(false);
            player.equipmentInfo.SetActive(false);

        }
    }
}
