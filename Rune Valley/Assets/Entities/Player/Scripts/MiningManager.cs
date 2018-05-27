using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class MiningManager : MonoBehaviour {

    public LayerMask GatherableLayer;

    private PlayerStats stats;
    private Vector3 rs, re;

	// Use this for initialization
	void Start () {
        stats = this.gameObject.GetComponentInParent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Gather();
        }
	}

    void OnDrawGizmos()
    {
        if (stats != null)
        {
            Debug.DrawLine(rs, rs + stats.miningDistance*re, new Color(255,0,0));
        }
    }

    void Gather()
    {
        RaycastHit hit;
        Transform camT = stats.getCamera().transform;
        Ray shotRay = new Ray(this.transform.position, this.transform.forward);
        if(Physics.Raycast(shotRay, out hit, stats.miningDistance, GatherableLayer))
        {
            Gatherable target = hit.collider.gameObject.GetComponent<Gatherable>();
            if (!target && hit.collider.gameObject!=null) {
                Debug.LogError("Tried mining object with no 'Gatherable' script");
            } else {
                target.Damage(stats.miningDamage);
                Debug.Log("Hit " + target.gameObject.name);
            }

        }
        rs = shotRay.origin;
        re = shotRay.direction;
    }
}
