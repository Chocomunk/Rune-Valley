using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlayerManager : MonoBehaviour {

    public static Player playerInstance;
    public static Inventory playerInventory;
    public static bool viewingInventory = false;

    public void Awake()
    {
        playerInventory = this.GetComponent<Inventory>();
    }

    public static bool PlayerExists()
    {
        return playerInstance != null;
    }

}
