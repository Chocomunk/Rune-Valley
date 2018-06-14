using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningManager : MonoBehaviour {

    public LayerMask GatherableLayer;
    public LayerMask InteractableLayer;

    private PlayerStats stats;

	// Use this for initialization
	void Start () {
        stats = PlayerManager.instance.playerStats;
	}

    public void Gather(ToolItem tool)
    {
        RaycastHit hit;
        Transform camT = PlayerManager.instance.playerCamera.transform;
        Ray shotRay = new Ray(camT.position, camT.forward);
        if (Physics.Raycast(shotRay, out hit, stats.miningDistance, GatherableLayer))
        {
            Gatherable target = hit.collider.gameObject.GetComponent<Gatherable>();
            if (!target && hit.collider.gameObject != null) {
                Debug.LogError("Tried mining object with no 'Gatherable' script");
            } else {
                if (target.gatherableStats.gatherableType == tool.toolType || target.gatherableStats.gatherableType < 0)
                {
                    target.Damage(stats.miningDamage);
                    //Debug.Log("Hit " + target.gameObject.name);
                }
            }

        }
    }

    public void Interact()
    {
        RaycastHit hit;
        Transform camT = PlayerManager.instance.playerCamera.transform;
        Ray shotRay = new Ray(camT.position, camT.forward);
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
