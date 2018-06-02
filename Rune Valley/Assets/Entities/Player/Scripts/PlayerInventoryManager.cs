using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour {

    public InventorySlot heldItemSlot;
    public InventoryUI playerInventoryUI;
    public InventoryUI externalInventoryUI;

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
        playerInventoryUI.SetInventory(PlayerManager.playerInventory);
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
                PlayerManager.viewingMenu = false;
            }
        }

        // Check for clicks outside of inventory menus
        if (!InventoryGUIMenu.PointerInAnyMenu())
        {
            if(Input.GetMouseButtonDown(0))
            {
                PlayerManager.inventoryManager.DropHeldItem();
            } else if (Input.GetMouseButtonDown(1))
            {
                PlayerManager.inventoryManager.DropPoppedHeldItem();
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
        playerInventoryUI.SetViewingInventory(viewing);
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
            ItemPickup newInstance = Instantiate(item.entryItem.resourcePrefab, 
                PlayerManager.playerInstance.transform.position + PlayerManager.playerCameraInstance.transform.forward * heldItemDropDistance, 
                PlayerManager.playerInstance.transform.rotation) as ItemPickup;
            newInstance.GetComponent<Rigidbody>().velocity = PlayerManager.playerCameraInstance.transform.forward * heldItemDropSpeed;
            newInstance.SetInventoryEntry(item);
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
