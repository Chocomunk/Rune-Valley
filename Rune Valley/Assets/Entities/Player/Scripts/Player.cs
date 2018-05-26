using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInfo))]
public class Player : MonoBehaviour {

    private PlayerInfo info;

	// Use this for initialization
	void Start () {
        info = this.gameObject.GetComponent<PlayerInfo>();
        PlayerManager.playerInstance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(info.health <= 0)
        {
            Die();
        }
	}

    public void Damage(float damage)
    {
        info.health -= (int)(damage + Random.Range(0,1));
    }

    void Die()
    {
        Debug.Log("Player has died");
    }

}
