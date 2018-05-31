using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    public Transform itemsParent;
    public GameObject inventoryUI;
    public InventorySlot inventorySlotPrefab;

    private Inventory inventory;
    private InventorySlot[] slots;
    private GameObject inventoryGrid;

	// Use this for initialization
	void Start () {
        inventory = PlayerManager.playerInventory;
        inventoryGrid = inventoryUI.GetComponentInChildren<GridLayoutGroup>().gameObject;
        slots = inventoryGrid.GetComponentsInChildren<InventorySlot>();

        inventory.onItemChangedCallback += UpdateUI;
        inventory.onSizeChangedCallback += RefreshGUI;

        RefreshGUI();
        inventoryUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Inventory"))
        {
            PlayerManager.viewingInventory = !inventoryUI.activeSelf;
            inventoryUI.SetActive(PlayerManager.viewingInventory);
        }
	}

    void RefreshGUI()
    {
        ClearSlots();
        InitializeGUI();
        UpdateUI();
    }

    void InitializeGUI()
    {
        slots = new InventorySlot[inventory.maxSize];
        for(int i=0; i<inventory.maxSize; i++)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab, inventoryGrid.transform) as InventorySlot;
            slot.index = i;
            slot.OnSlotLeftCLickCallback += HandleLeftClick;
            slot.OnSlotRightCLickCallback += HandleRightClick;
            slots[i] = slot;
        }
    }

    void ClearSlots()
    {
        for(int i=0; i<slots.Length; i++)
        {
            GameObject go = slots[i].gameObject;
            slots[i] = null;
            Destroy(go);
        }
    }

    void UpdateUI()
    {
        for (int i=0; i<slots.Length; i++)
        {
            if (inventory.items[i] != null)
            {
                slots[i].AddItem(inventory.items[i]);
            } else
            {
                slots[i].ClearSlot();
            }
        }
    }

    void HandleLeftClick(int index)
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

    void HandleRightClick(int index)
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
                inventory.SetItem(index, inventoryManager.heldItem.PopItem());
            } else if (slotItem.equals(inventoryManager.heldItem))
            {
                inventory.MergeStack(index, inventoryManager.heldItem.PopItem());
            }
        }
    }
}
