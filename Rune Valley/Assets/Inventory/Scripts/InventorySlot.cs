using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour {

    public delegate void OnSlotLeftClick(int slotIndex);
    public delegate void OnSlotRightClick(int slotIndex);
    public OnSlotLeftClick OnSlotLeftCLickCallback;
    public OnSlotRightClick OnSlotRightCLickCallback;

    public Image icon;
    public Text itemCountText;

    public int index = 0;

    private InventoryEntry _item;
    public InventoryEntry item {
        get { return _item; }
    }

    void UpdateText()
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

    public void HandleLeftClick()
    {
        OnSlotLeftCLickCallback.Invoke(index);
    }

    public void HandleRightClick()
    {
        OnSlotRightCLickCallback.Invoke(index);
    }

    public void UseItem()
    {
        if (_item != null)
        {
            _item.Use();
        }

        UpdateText();
    }
}