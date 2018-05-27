using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemPickup : MonoBehaviour {

    public float attractRange = 7;
    public float pickupRange = .6f;
    public float flightSpeed = 3;
    public float lifetime = -1;

    public Item item;

    private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = this.gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (PlayerManager.PlayerExists())
        {
            if (InRange(attractRange))
            {
                MoveToPlayer();
                if (InRange(pickupRange))
                {
                    Debug.Log("Picked up "+this.gameObject.name);
                    Inventory.instance.Add(item);
                    Destroy(this.gameObject);
                }
            }
        }
	}

    void OnDrawGizmosSelected()
    {
        if (InRange(pickupRange))
        {
            Gizmos.color = Color.blue;
        } else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(this.transform.position, pickupRange);
    }

    bool InRange(float range)
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
