using Unity.Mathematics;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] 
    private GameObject dropPrefab;
    [SerializeField] 
    private ItemDataSO[] possibleDrop;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            DropItem(possibleDrop[i]);
        }
    }
    
    public void DropItem(ItemDataSO itemDataSo)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, quaternion.identity);
        newDrop.GetComponent<ItemObject>().SetupItem(itemDataSo);
    }
}
