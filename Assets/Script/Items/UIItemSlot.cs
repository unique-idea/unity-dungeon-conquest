
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour , IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;

    public InventoryItems items;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
       // Debug.Log("Run UIItemSlot");
    }
    public void UpdateSlot(InventoryItems _newItems)
    {
        items = _newItems;

        itemImage.color = Color.white;

        if (_newItems != null)
        {
            itemImage.sprite = _newItems.data.icon;

            if (_newItems.stackSize > 1)
            {
                itemText.text = _newItems.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        items = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(items == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Inventory.instance.RemoveItem(items.data);
        }

        if(items.data.itemType == ItemType.Equipment && !Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(items != null)
            {
                Inventory.instance.EquipItem(items.data);
            }
        }
        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (items == null)
        {
            return;
        }
        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(items == null)
        {
            return;
        }

        ui.itemToolTip.ShowToolTip(items.data as ItemDataEquipment);
    }
}
