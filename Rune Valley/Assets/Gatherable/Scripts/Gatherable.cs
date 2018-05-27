using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GatherableStats))]
public class Gatherable : MonoBehaviour {

    private GatherableStats stats;

    public GameObject drop;

    public int dropCount = 3;
    public float dropSpread = 0.5f;

	// Use this for initialization
	void Start () {
        stats = this.gameObject.GetComponent<GatherableStats>();
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
        Debug.Log(this.gameObject.name + " destroyed");
        Drop();
        Destroy(this.gameObject);
    }

    void Drop()
    {
        if(drop != null)
        {
            for(int i=0; i<dropCount; i++)
            {
                float x = Random.Range(-dropSpread, dropSpread);
                float y = Random.Range(-.25f, .25f);
                float z = Random.Range(-dropSpread, dropSpread);

                Instantiate(drop, new Vector3(x, y, z) + this.transform.position, Quaternion.identity);
            }
        } else {
            Debug.LogError(this.gameObject.name+" has no resource prefab to drop");
        }
    }

}
