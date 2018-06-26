using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningManager : MonoBehaviour {

    public LayerMask GatherableLayer;
    public LayerMask InteractableLayer;
    public AudioSource managerAudioSource;

    private PlayerStats stats;
    private ToolItem selectedTool = null;

	// Use this for initialization
	void Start () {
        stats = PlayerManager.instance.playerStats;
	}

    void Update()
    {
        if (PlayerManager.instance.viewingMenu)
        {
            return;
        }

        // Mining or interaction
        if (Input.GetButtonDown("Fire1"))
        {
            InventoryEntry selectedItem = PlayerManager.instance.inventoryManager.getSelectedItem();
            if(selectedItem != null)
            {
                if(selectedItem.entryItem is ToolItem)
                {
                    //PlayerManager.instance.miningManager.Gather(selectedItem.entryItem as ToolItem);
                    selectedTool = selectedItem.entryItem as ToolItem;
                    PlayerManager.instance.playerAnimator.SetBool("SwingTool", true);
                } else
                {
                    PlayerManager.instance.playerAnimator.SetBool("SwingTool", false);
                    if(selectedItem.entryItem is UsableItem)
                    {
                        (selectedItem.entryItem as UsableItem).Use();
                        selectedItem.PopItem();
                    }
                }
            }
        } else if (Input.GetButtonUp("Fire1"))
        {
            PlayerManager.instance.playerAnimator.SetBool("SwingTool", false);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            PlayerManager.instance.miningManager.Interact();
        }
    }

    public void Gather()
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
                managerAudioSource.clip = target.gatherSound;
                managerAudioSource.Play();
                if (target.gatherableStats.gatherableType == selectedTool.toolType || target.gatherableStats.gatherableType < 0)
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
