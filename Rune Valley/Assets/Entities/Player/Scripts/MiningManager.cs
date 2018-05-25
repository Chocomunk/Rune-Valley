using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInfo))]
public class MiningManager : MonoBehaviour {

    public LayerMask GatherableLayer;

    private PlayerInfo info;
    private Vector3 rs, re;

	// Use this for initialization
	void Start () {
        info = this.gameObject.GetComponentInParent<PlayerInfo>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Gather();
        }
        Debug.DrawLine(rs, rs + info.miningDistance*re, new Color(255,0,0));
	}

    void Gather()
    {
        RaycastHit hit;
        Transform camT = info.getCamera().transform;
        Ray shotRay = new Ray(this.transform.position, this.transform.forward);
        if(Physics.Raycast(shotRay, out hit, info.miningDistance, GatherableLayer))
        {
            Gatherable target = hit.collider.gameObject.GetComponent<Gatherable>();
            if (!target && hit.collider.gameObject!=null) {
                Debug.LogError("Tried mining object with no 'Gatherable' script");
            } else {
                target.Damage(info.miningDamage);
                Debug.Log("Hit " + target.gameObject.name);
            }

        }
        rs = shotRay.origin;
        re = shotRay.direction;
    }
}
