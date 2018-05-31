using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    public Transform itemsParent;
    public GameObject inventoryUI;
    public InventorySlot inventorySlotPrefab;
    public Inventory inventory;

    private InventorySlot[] slots;
    private GameObject inventoryGrid;

	// Use this for initialization
	void Start () {
        inventoryGrid = inventoryUI.GetComponentInChildren<GridLayoutGroup>().gameObject;
        slots = inventoryGrid.GetComponentsInChildren<InventorySlot>();

        inventory.onItemChangedCallback += UpdateUI;
        inventory.onSizeChangedCallback += RefreshGUI;

        RefreshGUI();
        inventoryUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SetViewingInventory(bool viewing)
    {
        inventoryUI.SetActive(viewing);
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
            slot.AssignInventoryInstance(inventory);
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
}
