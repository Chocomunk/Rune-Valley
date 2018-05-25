using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GatherableInfo))]
public class Gatherable : MonoBehaviour {

    private GatherableInfo info;

	// Use this for initialization
	void Start () {
        info = this.gameObject.GetComponent<GatherableInfo>();
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
        Debug.Log(this.gameObject.name + " destroyed");
        Destroy(this.gameObject);
    }

}
