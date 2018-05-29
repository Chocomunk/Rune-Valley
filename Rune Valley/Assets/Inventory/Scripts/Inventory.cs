using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{

    public delegate void OnItemChanged();
    public delegate void OnSizeChanged();
    public OnItemChanged onItemChangedCallback;
    public OnSizeChanged onSizeChangedCallback;

    public InventoryEntry[] items;

    private int _maxSize = 20;
    public int maxSize {
        get { return _maxSize; }
        set { SetSize(value); }
    }

    public void Awake()
    {
        items = new InventoryEntry[_maxSize];
    }

    public void SetSize(int size)
    {
        if (size < 0)
        {
            Debug.LogError("Cannot set inventory size to be negative!");
            return;
        }

        if (size < _maxSize)
        {
            Debug.LogWarning("Warning! Setting new inventory size to be smaller! Items may be lost");
        }

        InventoryEntry[] newItems = new InventoryEntry[size];
        for (int i = 0; i < _maxSize; i++)
        {
            newItems[i] = items[i];
        }
        items = newItems;
        _maxSize = size;
        InvokeOnSizeChangedCallback();
    }

    public bool Add(InventoryEntry item)
    {
        if (item == null)
        {
            Debug.LogError("Trying to add null pointer to inventory!");
            return false;
        }

        if (item.IsEmpty())
        {
            Debug.LogError("Trying to add empty inventory entry to inventory!");
            return false;
        }

        if (!(item.entryItem.isDefaultItem))
        {
            int nullIndex = -1;
            int existingIndex = -1;
            for (int i = 0; i < _maxSize && nullIndex < 0 && existingIndex < 0; i++)
            {
                if (nullIndex < 0 && items[i] == null)
                {
                    nullIndex = i;
                    continue;
                }
                if (existingIndex < 0 && item.equals(items[i]))
                    existingIndex = i;
            }

            if(existingIndex >= 0)
            {
                items[existingIndex].MergeInventory(item);
                InvokeOnItemChangedCallback();
                return true;
            } else if(nullIndex >= 0)
            {
                items[nullIndex] = item;
                InvokeOnItemChangedCallback();
                return true;
            }
        }
        return false;
    }

    public InventoryEntry SetItem(int index, InventoryEntry newItem)
    {
        if (index >= maxSize || index < 0)
        {
            throw new IndexOutOfRangeException("Inventory index out of bounds");
        }

        InventoryEntry oldItem = items[index];
        if (oldItem.equals(newItem))
        {
            oldItem.MergeInventory(newItem);
            return null;
        } else
        {
            items[index] = newItem;
            return oldItem;
        }
    }

    public InventoryEntry Remove(InventoryEntry item)
    {
        if (item == null)
        {
            Debug.LogError("Trying to remove null pointer form inventory!");
            return null;
        }

        for (int i = 0; i < _maxSize; i++)
        {
            if (items[i] == item)
            {
                InventoryEntry oldItem = items[i];
                items[i] = null;

                InvokeOnItemChangedCallback();
                return oldItem;
            }
        }
        return null;
    }

    public InventoryEntry RemoveAt(int index, int count)
    {
        if (index >= maxSize || index < 0)
        {
            throw new IndexOutOfRangeException("Inventory index out of bounds");
        }

        if (count == 0)
        {
            Debug.LogWarning("Remove count is 0, not removing any items!");
            return null;
        }

        if (items[index] != null)
        {
            InventoryEntry oldItem = items[index];
            int newCount = count < 0 || count > oldItem.itemCount? oldItem.itemCount : count;
            InventoryEntry newItem = oldItem.PopItem(newCount);
            if (oldItem.IsEmpty())
                items[index] = null;

            InvokeOnItemChangedCallback();
            return newItem;
        }
        return null;
    }

    void InvokeOnItemChangedCallback()
    {
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    void InvokeOnSizeChangedCallback()
    {
        if (onSizeChangedCallback != null)
        {
            onSizeChangedCallback.Invoke();
        }
    }

}