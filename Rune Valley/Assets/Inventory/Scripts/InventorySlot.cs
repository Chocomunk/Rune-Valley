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
        if(inventory != null)
        {
            PlayerInventoryManager inventoryManager = PlayerManager.inventoryManager;
            InventoryEntry slotItem = inventory.items[index];
            if (inventoryManager.heldItem != null && slotItem != null && inventoryManager.heldItem.equals(slotItem))
            {
                inventory.MergeStack(index, inventoryManager.heldItem);
                inventoryManager.ReleaseHeldItem();
            } else
            {
                InventoryEntry oldPlayerEntry = inventoryManager.SetHeldItem(inventory.items[index]);
                inventory.SetItem(index, oldPlayerEntry);
            }
        }
    }

    public void HandleRightClick()
    {
        if(inventory != null)
        {
            PlayerInventoryManager inventoryManager = PlayerManager.inventoryManager;
            InventoryEntry slotItem = inventory.items[index];
            if(inventoryManager.heldItem == null && slotItem != null)
            {
                inventoryManager.SetHeldItem(inventory.SplitStack(index));
            } else if(inventoryManager.heldItem != null)
            {
                if(slotItem == null)
                {
                    inventory.SetItem(index, inventoryManager.PopHeldItem());
                } else if (slotItem.equals(inventoryManager.heldItem))
                {
                    inventory.MergeStack(index, inventoryManager.PopHeldItem());
                }
            }
        }
    }
}