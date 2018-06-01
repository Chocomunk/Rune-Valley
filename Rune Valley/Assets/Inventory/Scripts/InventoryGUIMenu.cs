using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryGUIMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    private static List<InventoryGUIMenu> inventoryMenuList = new List<InventoryGUIMenu>();

    private bool pointerInMenu = false;

    // Use this for initialization
    void Start () {
        if (!inventoryMenuList.Contains(this))
        {
            inventoryMenuList.Add(this);
        } else
        {
            Debug.LogError("Inventory Menu already registered before game start!");
        }
	}

	// Update is called once per frame
	void Update () {

	}

    public static bool PointerInAnyMenu()
    {
        foreach(InventoryGUIMenu menu in inventoryMenuList)
        {
            if (menu.pointerInMenu)
                return true;
        }
        return false;
    }

    public bool PointerInMenu()
    {
        return pointerInMenu;
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
