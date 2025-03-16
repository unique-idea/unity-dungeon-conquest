using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetUpCraftSlot(ItemDataEquipment _data)
    {
        if(_data == null)
        {
            return;
        }

        items.data = _data;

        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetUpCraftWindow(items.data as ItemDataEquipment);
    }

}
