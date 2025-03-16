using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipmentSlot : UIItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(items == null || items.data == null)
        {
            return;
        }

        Inventory.instance.UnEquipItem(items.data as ItemDataEquipment);
        Inventory.instance.AddItem(items.data as ItemDataEquipment);

        ui.itemToolTip.HideToolTip();

        CleanUpSlot();
    }
}
