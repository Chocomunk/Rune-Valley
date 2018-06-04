using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemPickup : MonoBehaviour {

    public float attractRange = 4;
    public float pickupRange = .75f;
    public float pickupCooldown = 2;    // Time in seconds after instantiation before item can be picked up
    public float flightSpeed = 3;
    public float lifetime = -1;

    public Item item;

    private float currCooldown = -1;
    private float currLifetime = -1;

    private Rigidbody rigidBody;
    private InventoryEntry entry;

	// Use this for initialization
	void Start () {
        rigidBody = this.gameObject.GetComponent<Rigidbody>();
        currCooldown = pickupCooldown;
        currLifetime = lifetime;
        if(entry == null)
            entry = new InventoryEntry(item, item.defaultCount);
	}
	
	void FixedUpdate () {
        if (PlayerManager.PlayerExists())
        {
            // Cooldown condition always true in "no cooldown" case (cooldown = -1)
            if (currCooldown <= 0 && PlayerInRange(attractRange)) 
            {
                MoveToPlayer();
                if (PlayerInRange(pickupRange))
                {
                    if (PlayerManager.inventoryManager.AddToPlayerInventory(entry))
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
        }

        // Cooldown = -1 means no cooldown
        if(pickupCooldown > 0 && currCooldown > 0)
        {
            currCooldown -= Time.deltaTime;
        }

        // Lifetime = -1 means infinite lifetime
        if(lifetime > 0)
        {
            if(currLifetime <= 0)
            {
                Destroy(this.gameObject);
            } else
            {
                currLifetime -= Time.deltaTime;
            }
        }
	}

    void OnDrawGizmosSelected()
    {
        if (PlayerInRange(pickupRange))
        {
            Gizmos.color = Color.blue;
        } else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(this.transform.position, pickupRange);
    }

    public bool SetInventoryEntry(InventoryEntry newEntry)
    {
        if (entry == null || entry.equals(newEntry))
        {
            this.entry = newEntry;
            return true;
        }
        return false;
    }

    bool PlayerInRange(float range)
    {
        if (PlayerManager.PlayerExists())
        {
            float distance = Vector3.Distance(PlayerManager.playerInstance.transform.position, this.transform.position);
            return distance < range;
        }
        return false;
    }

    void MoveToPlayer()
    {
        Vector3 dir = PlayerManager.playerInstance.transform.position + new Vector3(0,1,0) - this.transform.position;
        rigidBody.MovePosition(this.transform.position + dir * Time.deltaTime * flightSpeed);
    }

}
