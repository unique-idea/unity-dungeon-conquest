
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
{
   public static Inventory instance;

    public List<ItemData> startingItem;

    public List<InventoryItems> equipment;
    public Dictionary<ItemDataEquipment, InventoryItems> equipmentDictionary;

    public List<InventoryItems> inventory;
    public Dictionary<ItemData, InventoryItems> inventoryDictionary;

    public List <InventoryItems> stash;
    public Dictionary<ItemData, InventoryItems> stashDictionary;

    [Header("Iventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    [Header("Items cooldown")]
    private float lastTimeUsed;
    private float lastTimeUserArmor;
    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Database")]
    public List<ItemData> itemDatabase;
    public List<InventoryItems> loadedItems;
    public List<ItemDataEquipment> loadedEquipment;

    private UIItemSlot[] inventoryItemSlot;
    private UIItemSlot[] stashItemSlot;
    private UIEquipmentSlot[] equipmentSlot;
    private UIStatSlot[] statSlot;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            //Debug.Log("Destroy Instance inventory");
        }
    }
    private void Start()
    {
        inventory = new List<InventoryItems>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItems>();

        stash = new List<InventoryItems>();
        stashDictionary = new Dictionary<ItemData, InventoryItems>();

        equipment = new List<InventoryItems>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItems>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UIItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UIEquipmentSlot>();

        statSlot = statSlotParent.GetComponentsInChildren<UIStatSlot>();

        AddStartingItem();
    }

    private void AddStartingItem()
    {
        foreach (ItemDataEquipment item in loadedEquipment)
        {
            EquipItem(item);
        }
        if (loadedItems.Count > 0)
        {
           foreach(InventoryItems item in loadedItems)
            {
                for(int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }

        for (int i = 0; i < startingItem.Count; i++)
        {
            if (startingItem[i] != null)
            {
                AddItem(startingItem[i]);
            }

        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;

        InventoryItems newItem = new InventoryItems(newEquipment);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItems> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if(oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();


        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnEquipItem(ItemDataEquipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItems value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);

            itemToRemove.RemoveModifiers();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);
        }else if(_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItems value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItems newItem = new InventoryItems(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    //Modify back just delete if
    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItems value))
        {
            if(_item.itemType == ItemType.Equipment)
            {
                InventoryItems newItem = new InventoryItems(_item);
                inventory.Add(newItem);
            }
            else
            {
                value.AddStack();
            }
        }
        else
        {
            InventoryItems newItem = new InventoryItems(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item,out InventoryItems value))
        {
            if(value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItems stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if(inventoryItemSlot == null)
        {
            Debug.Log("Null");
        }

       if(inventory.Count >= inventoryItemSlot.Length)
        {
            return false;
        } 
        return true;
    }
    private void UpdateSlotUI()
    {

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItems> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }


        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatUI();
    }

    public void UpdateStatUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValue();
        }
    }

    public bool CanCraft(ItemDataEquipment _itemToCraft, List<InventoryItems> _reqiredMaterial)
    {
        List<InventoryItems> materialsToRemove = new List<InventoryItems>();

        for(int i = 0; i < _reqiredMaterial.Count; i++) 
        {
            if (stashDictionary.TryGetValue(_reqiredMaterial[i].data, out InventoryItems stashValue))
            {
                if(stashValue.stackSize < _reqiredMaterial[i].stackSize)
                {
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue); 
                }
            }
            else
            {
                return false;
            }
        }
        
        for(int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        return true;
       
    }

    public List<InventoryItems> GetEquipmentList() 
    {
       return equipment;
    }

    public List<InventoryItems> GetStashList() { return stash; }

    public ItemDataEquipment GetEquipment(EquipmentType _type) 
    {
        ItemDataEquipment equipedItem = null;
        foreach (KeyValuePair<ItemDataEquipment, InventoryItems> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }
        return equipedItem;
    }

    public void UserFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if(currentFlask == null)
        {
            return;
        }

        bool canUseFlask = Time.time > lastTimeUsed + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsed = Time.time;
        }
        else
        {

        }
    }

    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor);

        if(Time.time > lastTimeUserArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUserArmor = Time.time;
            return true;
        }
        return false;
    }

    public void LoadData(GameData _data)
    {
       foreach(KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in itemDatabase)
            {
                if (item != null && item.itemId == pair.Key){
                    InventoryItems itemToLoad = new InventoryItems(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach(string loadedItemId in _data.equipmentId)
        {
            foreach(var item in itemDatabase)
            {
                if(item != null && loadedItemId == item.itemId)
                {
                    loadedEquipment.Add(item as ItemDataEquipment);
                }
            }
        }
        //AddStartingItem();
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach(KeyValuePair<ItemData, InventoryItems> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData, InventoryItems> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        } 

        foreach(KeyValuePair<ItemDataEquipment, InventoryItems> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item database")]
    private void FillUpItemDatabase() => itemDatabase = new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetName = AssetDatabase.FindAssets("", new[] { "Assets/Script/Items" });

        foreach(string SOname in assetName)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOname);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDatabase.Add(itemData);
        }
        return itemDatabase;
    }
#endif
}
