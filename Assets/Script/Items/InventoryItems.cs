using System;

[Serializable]
public class InventoryItems
{
    public ItemData data;
    public int stackSize;

    public InventoryItems(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;

}
