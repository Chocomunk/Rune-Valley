using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class Player : MonoBehaviour {

    private PlayerStats stats;

	// Use this for initialization
	void Start () {
        stats = this.gameObject.GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () {
		if(stats.health <= 0)
        {
            Die();
        }
	}

    public void Damage(float damage)
    {
        stats.health -= (int)(damage + Random.Range(0,1));
    }

    void Die()
    {
        Debug.Log("Player has died");
    }

}
