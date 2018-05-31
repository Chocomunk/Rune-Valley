using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name = "New Item";
    public ItemPickup resourcePrefab;
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public int defaultCount = 1;

    public virtual void Use()
    {
        // Implement

        Debug.Log("Using " + name);
    }

}
