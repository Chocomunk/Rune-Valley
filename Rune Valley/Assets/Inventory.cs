using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Singleton

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one inventory instance found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int maxSize = 20;
    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        if (!(item.isDefaultItem || items.Count >= maxSize))
        {
            items.Add(item);
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
            return true;
        }
        return false;
    }

    public bool Remove(Item item)
    {
        return items.Remove(item);
    }

}
