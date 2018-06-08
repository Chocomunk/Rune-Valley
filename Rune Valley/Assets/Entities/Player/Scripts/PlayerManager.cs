﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInventoryManager))]
public class PlayerManager : MonoBehaviour {

    private static PlayerManager _instance = null;
    public static PlayerManager instance {
        get { return _instance; }
    }

    public Player playerInstance;
    public PlayerStats playerStats;
    public Camera playerCamera;
    [HideInInspector] public PlayerInventoryManager inventoryManager;

    [HideInInspector] public bool viewingMenu;

    public void Awake()
    {
        inventoryManager = this.GetComponent<PlayerInventoryManager>();
        if(_instance != null)
        {
            Debug.LogError("Multiple PlayerManager instances found! Overriding existing instance.");
        }
        _instance = this;
    }

    public bool PlayerExists()
    {
        return playerInstance != null;
    }

}
