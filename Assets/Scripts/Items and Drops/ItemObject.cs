using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemObject : MonoBehaviour
{
    [field:SerializeField]
    public ItemDataSO ItemDataSo { get; private set; }
    [SerializeField] 
    private Rigidbody2D rb;
    [SerializeField] 
    private Transform itemTransform;
    [SerializeField] 
    private float timeForMoving;
    
    private Vector3 offset;
    private bool magnetize;
    private float delay;
    private float pastTime;
    
    private void Update()
    {
        if (ItemDataSo.itemType == ItemType.MATERIAL)
        {
            if (timeForMoving >= delay)
            {
                pastTime = Time.deltaTime;
                itemTransform.position += offset * Time.deltaTime;
                delay += pastTime;
            }
        }

        if (magnetize)
        {
            Vector3 playerPosition = Vector3.MoveTowards(transform.position,
                PlayerManager.Instance.player.transform.position + new Vector3(0, -0.3f, 0), 20 * Time.deltaTime);
            rb.MovePosition(playerPosition);
        }
    }

    public void SetupItem(ItemDataSO itemDataSo)
    {
        ItemDataSo = itemDataSo;
        offset = new Vector3(Random.Range(-1, 2), offset.y, offset.z);
        offset = new Vector3(offset.x, Random.Range(-1, 2), offset.z);
        
        if (ItemDataSo == null)
        {
            return;
        }
        
        if (ItemDataSo.itemType == ItemType.EQUIPMENT)
        {
            EquipmentDataSO equipmentDataSo = ItemDataSo as EquipmentDataSO;
            if (equipmentDataSo != null) gameObject.name = equipmentDataSo.itemName;
        }
        else if (ItemDataSo.itemType == ItemType.MATERIAL)
        {
            StartCoroutine(MoveTowardsPlayerPosition());
            gameObject.name = ItemDataSo.name;
        }
        
        GetComponent<SpriteRenderer>().sprite = ItemDataSo.icon;
    }

    private IEnumerator MoveTowardsPlayerPosition()
    {
        yield return new WaitForSeconds(3f);
        magnetize = true;
    }

    public void ItemPickup()
    {
        Inventory.Instance.AddItem(ItemDataSo);
        Destroy(gameObject);
    }
}
