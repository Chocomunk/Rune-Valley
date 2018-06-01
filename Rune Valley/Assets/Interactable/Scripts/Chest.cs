using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public InventoryUI inventoryUI;

    private bool _viewingInventory = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_viewingInventory)
        {
            if(Input.GetButton("Inventory"))
            {
                Close();
            } else if (Input.GetKeyUp(KeyCode.Escape))
            {
                Close();
                PlayerManager.inventoryManager.SetViewingInventory(!PlayerManager.inventoryManager.viewingInventory);
                PlayerManager.viewingMenu = false;
            }
        }
	}

    public void Open()
    {
        _viewingInventory = true;
        inventoryUI.SetViewingInventory(true);
        PlayerManager.inventoryManager.SetViewingInventory(true);
    }

    public void Close()
    {
        _viewingInventory = false;
        inventoryUI.SetViewingInventory(false);
    }
}
