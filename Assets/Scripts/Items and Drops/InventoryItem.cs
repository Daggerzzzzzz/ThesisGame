using System;

[Serializable]
public class InventoryItem
{
    public ItemDataSO itemDataSo;
    public int stackSize;

    public InventoryItem(ItemDataSO itemDataSo)
    {
        this.itemDataSo = itemDataSo;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }
    
    public void RemoveToStack()
    {
        stackSize--;
    }
}
