using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Button removeButton;
    public Text itemCountText;

    public int index = 0;

    InventoryEntry item;

    void UpdateText()
    {
        if(item != null && item.itemCount > 0)
        {
            itemCountText.text = item.itemCount+"";
        } else
        {
            itemCountText.text = "";
        }
    }
    
    public void AddItem (InventoryEntry newItem)
    {
        item = newItem;

        icon.sprite = item.entryItem.icon;
        icon.enabled = true;
        removeButton.interactable = true;

        UpdateText();
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;

        UpdateText();
    }

    public void OnRemoveButton()
    {
        PlayerManager.playerInventory.RemoveAt(index, 1);

        UpdateText();
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }

        UpdateText();
    }

}