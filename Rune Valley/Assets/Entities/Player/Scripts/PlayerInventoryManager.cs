using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour {

    public InventorySlot heldItemSlot;

    private RectTransform heldItemRect;

    private bool showHeldItem = false;

    private InventoryEntry _heldItem;
    public InventoryEntry heldItem {
        get { return _heldItem; }
    }

    public void Start()
    {
        heldItemRect = heldItemSlot.GetComponent<RectTransform>();
    }

    public void Update()
    {
        if(_heldItem != null && _heldItem.IsEmpty())
        {
            _heldItem = null;
        }
        if (showHeldItem)
        {
            heldItemRect.anchoredPosition = Input.mousePosition;
        }
    }

    public void SetShowHeldItem(bool shouldShow)
    {
        this.showHeldItem = shouldShow;
        this.heldItemSlot.gameObject.SetActive(shouldShow);
    }

    public InventoryEntry SetHeldItem(InventoryEntry itemEntry)
    {
        if(itemEntry != null && itemEntry.itemCount <= 0)
        {
            Debug.LogError("Trying to set to an empty item entry, empty item entries should not exist!");
            return null;
        }

        InventoryEntry oldEntry = _heldItem;
        _heldItem = itemEntry;
        if(_heldItem == null)
        {
            heldItemSlot.ClearSlot();
        } else
        {
            heldItemSlot.AddItem(_heldItem);
        }
        return oldEntry;
    }

    public InventoryEntry ReleaseHeldItem()
    {
        return SetHeldItem(null);
    }
}
