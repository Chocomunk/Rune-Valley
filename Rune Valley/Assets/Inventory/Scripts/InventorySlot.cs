using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Image BG;
    public Text itemCountText;
    public Sprite defaultBG;
    public Sprite selectedBG;

    [HideInInspector] public int index = 0;

    private Inventory inventory;

    private InventoryEntry _item;
    public InventoryEntry item {
        get { return _item; }
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
        _item.OnCountChangedCallback += UpdateText;

        icon.sprite = _item.entryItem.icon;
        icon.enabled = true;

        UpdateText();
    }

    public void ClearSlot()
    {
        if (_item != null)
            _item.OnCountChangedCallback -= UpdateText;
        _item = null;

        icon.sprite = null;
        icon.enabled = false;

        UpdateText();
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            BG.sprite = selectedBG;
        } else
        {
            BG.sprite = defaultBG;
        }
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
        PlayerManager.instance.inventoryManager.StackItemInteract(inventory, index);
    }

    public void HandleRightClick()
    {
        PlayerManager.instance.inventoryManager.SingleItemInteract(inventory, index);
    }
}