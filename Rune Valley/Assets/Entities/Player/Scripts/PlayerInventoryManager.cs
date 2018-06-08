﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour {

    public InventorySlot heldItemSlot;

    public InventoryUI packInventoryUI;
    public InventoryUI hotbarInventoryUI;
    public InventoryUI externalInventoryUI;

    public Inventory packInventory;
    public Inventory hotbarInventory;

    public float heldItemDropDistance = 1.5f;
    public float heldItemDropSpeed = 1.5f;

    private RectTransform heldItemRect;

    private bool _viewingInventory = false;
    public bool viewingInventory {
        get { return _viewingInventory; }
    }

    private bool _viewingExternalInventory = false;
    public bool viewingExternalInventory {
        get { return _viewingExternalInventory; }
    }

    private InventoryEntry _heldItem;
    public InventoryEntry heldItem {
        get { return _heldItem; }
    }

    public void Start()
    {
        heldItemRect = heldItemSlot.GetComponent<RectTransform>();
        packInventoryUI.SetInventory(packInventory);
        hotbarInventoryUI.SetInventory(hotbarInventory);
    }

    public void Update()
    {
        // Release held item if it is empty
        if(_heldItem != null && _heldItem.IsEmpty())
        {
            ReleaseHeldItem();
        }

        // Toggle inventory view (GUI and UI consqeuences)
		if(Input.GetButtonDown("Inventory") || (_viewingInventory && Input.GetKeyUp(KeyCode.Escape)))
        {
            SetViewingInventory(!_viewingInventory);
            if (!_viewingInventory)
            {
                SetViewingExternalInventory(false);
                PlayerManager.instance.viewingMenu = false;
            }
        }

        // Check for clicks outside of inventory menus
        if (!InventoryGUIMenu.PointerInAnyMenu())
        {
            if(Input.GetMouseButtonDown(0))
            {
                PlayerManager.instance.inventoryManager.DropHeldItem();
            } else if (Input.GetMouseButtonDown(1))
            {
                PlayerManager.instance.inventoryManager.DropPoppedHeldItem();
            }
        }
    }

    public void LateUpdate()
    {
        // If viewing inventory, have the held item follow the cursor
        if (_viewingInventory)
        {
            heldItemRect.anchoredPosition = Input.mousePosition;
        }
    }

    public void SetViewingInventory(bool viewing)
    {
        this._viewingInventory = viewing;
        this.heldItemSlot.gameObject.SetActive(viewing);
        packInventoryUI.SetViewingInventory(viewing);
        hotbarInventoryUI.SetViewingInventory(viewing);
        if (!viewing && _heldItem != null)
        {
            DropHeldItem();
        }
    }

    public void SetExternalInventory(Inventory externalInventory, string title)
    {
        if(externalInventory == null)
        {
            Debug.LogError("Trying to assign a null external inventory");
            return;
        }
        externalInventoryUI.SetInventory(externalInventory, title);
    }

    public void SetViewingExternalInventory(bool viewing)
    {
        this._viewingExternalInventory = viewing;
        externalInventoryUI.SetViewingInventory(viewing);
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
            Transform playerTransform = PlayerManager.instance.playerInstance.transform;
            Transform cameraTransform = PlayerManager.instance.playerCamera.transform;

            ItemPickup newInstance = Instantiate(item.entryItem.resourcePrefab, 
                playerTransform.position + cameraTransform.forward * heldItemDropDistance, 
                playerTransform.rotation) as ItemPickup;
            newInstance.GetComponent<Rigidbody>().velocity = cameraTransform.forward * heldItemDropSpeed;
            newInstance.SetInventoryEntry(item);
        }
    }

    public bool AddToPlayerInventory(InventoryEntry newItem)
    {
        bool hotbarAttempt = hotbarInventory.Add(newItem);
        bool packInventoryAttempt = hotbarAttempt ? true : packInventory.Add(newItem);
        return packInventoryAttempt;
        
    }

    public void StackItemInteract(Inventory sourceInventory, int index)
    {
        if(sourceInventory != null)
        {
            InventoryEntry slotItem = sourceInventory.items[index];
            if (_heldItem != null && slotItem != null && _heldItem.equals(slotItem))
            {
                sourceInventory.MergeStack(index, _heldItem);
                ReleaseHeldItem();
            } else
            {
                if (Input.GetKey(KeyCode.LeftShift) && slotItem != null)
                {
                    if (_viewingExternalInventory)
                    {
                        if(sourceInventory == externalInventoryUI.inventory)
                        {
                            if (AddToPlayerInventory(slotItem))
                                sourceInventory.RemoveAt(index);
                        } else
                        {
                            if (externalInventoryUI.inventory.Add(slotItem))
                                sourceInventory.RemoveAt(index);
                        }
                    } else
                    {
                        if(sourceInventory == hotbarInventory)
                        {
                            if (packInventory.Add(slotItem))
                                hotbarInventory.RemoveAt(index);
                        }
                        if(sourceInventory == packInventory)
                        {
                            if (hotbarInventory.Add(slotItem))
                                packInventory.RemoveAt(index);
                        }
                    }
                } else
                {
                    InventoryEntry oldPlayerEntry = SetHeldItem(sourceInventory.items[index]);
                    sourceInventory.SetItem(index, oldPlayerEntry);
                }
            }
        }
    }

    public void SingleItemInteract(Inventory sourceInventory, int index)
    {
        if(sourceInventory != null)
        {
            InventoryEntry slotItem = sourceInventory.items[index];
            if(_heldItem == null && slotItem != null)
            {
                SetHeldItem(sourceInventory.SplitStack(index));
            } else if(_heldItem != null)
            {
                if(slotItem == null)
                {
                    sourceInventory.SetItem(index, PopHeldItem());
                } else if (slotItem.equals(heldItem))
                {
                    sourceInventory.MergeStack(index, PopHeldItem());
                }
            }
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
