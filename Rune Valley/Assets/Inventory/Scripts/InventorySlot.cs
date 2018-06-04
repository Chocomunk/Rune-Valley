using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Text itemCountText;

    public int index = 0;

    private Inventory inventory;

    private InventoryEntry _item;
    public InventoryEntry item {
        get { return _item; }
    }

    public void UseItem()
    {
        if (_item != null)
        {
            _item.Use();
        }

        UpdateText();
    }

    public void UpdateText()
    {
        if(_item != null && _item.itemCount > 0)
        {
            itemCountText.text = _item.itemCount+"";
        } else
        {
            itemCountText.text = "";
        }
    }
    
    public void AddItem (InventoryEntry newItem)
    {
        _item = newItem;

        icon.sprite = _item.entryItem.icon;
        icon.enabled = true;

        UpdateText();
    }

    public void ClearSlot()
    {
        _item = null;

        icon.sprite = null;
        icon.enabled = false;

        UpdateText();
    }

    public void AssignInventoryInstance(Inventory newInstance)
    {
        if (newInstance != null)
        {
            inventory = newInstance;
        } else
        {
            Debug.LogError("Trying to assign a null invenotry instance!");
        }
    }

    public void HandleLeftClick()
    {
        PlayerManager.inventoryManager.StackItemInteract(inventory, index);
    }

    public void HandleRightClick()
    {
        PlayerManager.inventoryManager.SingleItemInteract(inventory, index);
    }
}