using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    public Transform itemsParent;
    public GameObject inventoryUI;
    public Text inventoryTitle;
    public InventorySlot inventorySlotPrefab;

    private InventorySlot[] _slots;
    public InventorySlot[] slots {
        get { return _slots; }
    }
    private Inventory _inventory;
    public Inventory inventory {
        get { return _inventory; }
        set { SetInventory(value); }
    }

	// Use this for initialization
	void Start () {
        _slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        if(_inventory != null)
        {
            _inventory.onItemChangedCallback += UpdateUI;
            _inventory.onSizeChangedCallback += RefreshGUI;
        }

        RefreshGUI();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SetInventory(Inventory newInventory)
    {
        SetInventory(newInventory, "Inventory");
    }

    public void SetInventory(Inventory newInventory, string inventoryName)
    {
        inventoryTitle.text = inventoryName;
        if(_inventory != null)
        {
            _inventory.onItemChangedCallback -= UpdateUI;
            _inventory.onSizeChangedCallback -= RefreshGUI;
        }
        _inventory = newInventory;
        _inventory.onItemChangedCallback += UpdateUI;
        _inventory.onSizeChangedCallback += RefreshGUI;

        RefreshGUI();
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
        if(_inventory != null)
        {
            _slots = new InventorySlot[_inventory.maxSize];
            for(int i=0; i<_inventory.maxSize; i++)
            {
                InventorySlot slot = Instantiate(inventorySlotPrefab, itemsParent) as InventorySlot;
                slot.index = i;
                slot.AssignInventoryInstance(_inventory);
                _slots[i] = slot;
            }
        }
    }

    void ClearSlots()
    {
        if(_slots != null)
        {
            for(int i=0; i<_slots.Length; i++)
            {
                if(_slots[i] != null)
                {
                    GameObject go = _slots[i].gameObject;
                    _slots[i] = null;
                    Destroy(go);
                }
            }
        }
    }

    void UpdateUI()
    {
        if(_inventory != null)
        {
            for (int i=0; i<_slots.Length; i++)
            {
                if (_inventory.items[i] != null)
                {
                    _slots[i].AddItem(_inventory.items[i]);
                } else
                {
                    _slots[i].ClearSlot();
                }
            }
        }
    }
}
