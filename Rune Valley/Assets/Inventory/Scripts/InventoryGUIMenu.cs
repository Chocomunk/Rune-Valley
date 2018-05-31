using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryGUIMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    private bool pointerInMenu = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!pointerInMenu)
        {
            if(Input.GetMouseButtonDown(0))
            {
                PlayerManager.inventoryManager.DropHeldItem();
            } else if (Input.GetMouseButtonDown(1))
            {
                PlayerManager.inventoryManager.DropPoppedHeldItem();
            }
        }
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerInMenu = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerInMenu = false;
    }
}
