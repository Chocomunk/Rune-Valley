﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int maxHealth = 100;
    public int health = 100;
    public int maxEnergy = 100;
    public int energy = 100;
    public int armor;

    public float walkingSpeed = 5;
    public float runningSpeed = 10;
    public float miningSpeed = 5;
    public float miningDistance = 10;
    public float miningDamage = 25;

    public void RecoverHealth(int amount)
    {
        health = (int)Mathf.Min(health + amount, maxHealth);
    }

    public void RecoverEnergy(int amount)
    {
        energy = (int)Mathf.Min(energy + amount, maxEnergy);
    }

}
