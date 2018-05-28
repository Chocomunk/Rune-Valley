using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

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
        bool result = items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        return result;
    }

}
