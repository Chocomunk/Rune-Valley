using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInventoryManager))]
public class PlayerManager : MonoBehaviour {

    public static Player playerInstance;
    public static Camera playerCameraInstance;
    public static PlayerInventoryManager inventoryManager;

    public static bool viewingMenu;

    public void Awake()
    {
        inventoryManager = this.GetComponent<PlayerInventoryManager>();
    }

    public static void SetPlayer(Player player)
    {
        playerInstance = player;
        playerCameraInstance = playerInstance.GetComponentInChildren<Camera>();
    }

    public static bool PlayerExists()
    {
        return playerInstance != null;
    }

}
