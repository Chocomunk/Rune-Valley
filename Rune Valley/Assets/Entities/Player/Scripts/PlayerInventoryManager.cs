﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour {

    public InventorySlot heldItemSlot;

    public float heldItemDropDistance = 1.5f;
    public float heldItemDropSpeed = 1.5f;

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
            ReleaseHeldItem();
        }
        if (showHeldItem)
        {
            heldItemRect.anchoredPosition = Input.mousePosition;
        }
    }

    public void DropHeldItem()
    {
        InstantiateItem(_heldItem);
        ReleaseHeldItem();
    }

    public void DropPoppedHeldItem()
    {
        InstantiateItem(this.PopHeldItem());
    }

    void InstantiateItem(InventoryEntry item)
    {
        if(_heldItem != null)
        {
            ItemPickup newInstance = Instantiate(item.entryItem.resourcePrefab, 
                PlayerManager.playerInstance.transform.position + PlayerManager.playerCameraInstance.transform.forward * heldItemDropDistance, 
                PlayerManager.playerInstance.transform.rotation) as ItemPickup;
            newInstance.GetComponent<Rigidbody>().velocity = PlayerManager.playerCameraInstance.transform.forward * heldItemDropSpeed;
            newInstance.SetInventoryEntry(item);
        }
    }

    public void SetShowHeldItem(bool shouldShow)
    {
        this.showHeldItem = shouldShow;
        this.heldItemSlot.gameObject.SetActive(shouldShow);
        if (!shouldShow && _heldItem != null)
        {
            DropHeldItem();
        }
    }

    public InventoryEntry PopHeldItem()
    {
        if(_heldItem != null)
        {
            InventoryEntry poppedEntry = _heldItem.PopItem();
            heldItemSlot.UpdateText();
            return poppedEntry;
        }
        return null;
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