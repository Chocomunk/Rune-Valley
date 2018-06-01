using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class MiningManager : MonoBehaviour {

    public LayerMask GatherableLayer;
    public LayerMask InteractableLayer;

    private PlayerStats stats;

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

        if (Input.GetButtonDown("Fire2"))
        {
            Interact();
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
                //Debug.Log("Hit " + target.gameObject.name);
            }

        }
    }

    void Interact()
    {
        RaycastHit hit;
        Transform camT = stats.getCamera().transform;
        Ray shotRay = new Ray(this.transform.position, this.transform.forward);
        if(Physics.Raycast(shotRay, out hit, stats.miningDistance, InteractableLayer))
        {
            Interactable target = hit.collider.gameObject.GetComponent<Interactable>();
            if (!target && hit.collider.gameObject!=null) {
                Debug.LogError("Tried interacting with object with no 'Interactable' script");
            } else {
                target.Interact();
                //Debug.Log("Hit " + target.gameObject.name);
            }

        }
    }
}
