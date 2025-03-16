using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLoseItems;
    [SerializeField] private float chanceToLoseMaterial;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItems> itemsToUnequip = new List<InventoryItems>();
        List<InventoryItems> materialToLose = new List<InventoryItems>();

        foreach (InventoryItems item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnEquipItem(itemsToUnequip[i].data as ItemDataEquipment);
        }

        foreach (InventoryItems item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLoseMaterial)
            {
                DropItem(item.data);
                materialToLose.Add(item);
            }
        }

        for (int i = 0; i < materialToLose.Count; i++)
        {
            inventory.RemoveItem(materialToLose[i].data);
        }
    }
}
