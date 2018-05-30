using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(PlayerInventoryManager))]
public class PlayerManager : MonoBehaviour {

    public static Player playerInstance;
    public static Inventory playerInventory;
    public static PlayerInventoryManager inventoryManager;

    public static bool _viewingInventory = false;
    public static bool viewingInventory {
        get { return _viewingInventory; }
        set {
            _viewingInventory = value;
            inventoryManager.SetShowHeldItem(value);
        }
    }

    public void Awake()
    {
        playerInventory = this.GetComponent<Inventory>();
        inventoryManager = this.GetComponent<PlayerInventoryManager>();
    }

    public static bool PlayerExists()
    {
        return playerInstance != null;
    }

}
