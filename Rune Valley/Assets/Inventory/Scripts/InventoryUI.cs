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
        inventory.onItemChangedCallback += UpdateUI;

        InitializeGUI();
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Inventory"))
        {
            PlayerManager.viewingInventory = !inventoryUI.activeSelf;
            inventoryUI.SetActive(PlayerManager.viewingInventory);
        }
	}

    void InitializeGUI()
    {
        slots = new InventorySlot[inventory.maxSize];
        for(int i=0; i<inventory.maxSize; i++)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab) as InventorySlot;
            slot.index = i;
            slot.transform.SetParent(inventoryGrid.transform);
            slots[i] = slot;
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
}
