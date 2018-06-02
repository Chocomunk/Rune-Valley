using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public Inventory chestInventory;

    private bool _viewingInventory = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_viewingInventory)
        {
            if(Input.GetButton("Inventory") || Input.GetKeyUp(KeyCode.Escape))
            {
                Close();
            }
        }
	}

    public void Open()
    {
        _viewingInventory = true;
        PlayerManager.inventoryManager.SetExternalInventory(this.chestInventory, "Chest");
        PlayerManager.inventoryManager.SetViewingInventory(true);
        PlayerManager.inventoryManager.SetViewingExternalInventory(true);
    }

    public void Close()
    {
        _viewingInventory = false;
    }
}
