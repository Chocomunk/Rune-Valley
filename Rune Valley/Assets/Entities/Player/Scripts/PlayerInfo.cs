﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerInfo : MonoBehaviour {

    public int health = 100;
    public int energy = 100;
    public int armor;

    public float walkingSpeed = 5;
    public float runningSpeed = 10;
    public float miningSpeed = 5;
    public float miningDistance = 10;
    public float miningDamage = 25;

    private Camera FirstPersonCamera;

    public void Start()
    {
        FirstPersonCamera = this.gameObject.GetComponent<Camera>();
    }

    public Camera getCamera()
    {
        return this.FirstPersonCamera;
    }

}
