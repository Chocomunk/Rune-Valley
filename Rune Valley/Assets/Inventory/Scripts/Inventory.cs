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

    public int maxSize = 20;

    private bool filled = false;

    private InventoryEntry[] _items;
    public InventoryEntry[] items {
        get { return _items; }
    }

    public void Awake()
    {
        _items = new InventoryEntry[maxSize];
        onItemChangedCallback += checkFull;
        onSizeChangedCallback += checkFull;
        checkFull();
    }

    public bool isFull()
    {
        return filled;
    }

    public void SetSize(int size)
    {
        if (size < 0)
        {
            Debug.LogError("Cannot set inventory size to be negative!");
            return;
        }

        if (size < maxSize)
        {
            Debug.LogWarning("Warning! Setting new inventory size to be smaller! Items may be lost");
        }

        if(items == null)
        {
            maxSize = size;
            return;
        }

        InventoryEntry[] newItems = new InventoryEntry[size];
        for (int i = 0; i < (maxSize < size ? maxSize : size) ; i++)
        {
            newItems[i] = _items[i];
        }
        _items = newItems;
        maxSize = size;
        InvokeOnSizeChangedCallback();
    }

    public InventoryEntry SplitStack(int index)
    {
        if (index >= maxSize || index < 0)
        {
            throw new IndexOutOfRangeException("Inventory index out of bounds");
        }

        if(items[index] == null)
        {
            return null;
        }

        InventoryEntry otherEntry = items[index].SplitEntry();
        if (items[index].IsEmpty())
            items[index] = null;
        InvokeOnItemChangedCallback();
        return otherEntry;
    }

    public bool MergeStack(int index, InventoryEntry entry)
    {
        if (index >= maxSize || index < 0)
        {
            throw new IndexOutOfRangeException("Inventory index out of bounds");
        }

        if(items[index] == null)
        {
            SetItem(index, entry);
        }

        if (items[index].equals(entry))
        {
            items[index].MergeEntry(entry);
            InvokeOnItemChangedCallback();
            return true;
        }
        return false;
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
            /// Prioritize stacking into a matching item slot, then try to fill empty slots
            int nullIndex = -1;
            int existingIndex = -1;
            for (int i = 0; i < maxSize && nullIndex < 0 && existingIndex < 0; i++)
            {
                if (nullIndex < 0 && _items[i] == null)
                {
                    nullIndex = i;
                    continue;
                }
                if (existingIndex < 0 && item.equals(_items[i]))
                    existingIndex = i;
            }

            if (existingIndex >= 0)
            {
                _items[existingIndex].MergeEntry(item);
                InvokeOnItemChangedCallback();
                return true;
            } else if(nullIndex >= 0)
            {
                _items[nullIndex] = item;
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

        InventoryEntry oldItem = _items[index];
        if (newItem != null && oldItem != null && oldItem.equals(newItem))
        {
            oldItem.MergeEntry(newItem);
            InvokeOnItemChangedCallback();
            return null;
        } else
        {
            _items[index] = newItem;
            InvokeOnItemChangedCallback();
            return oldItem;
        }
    }

    public InventoryEntry Remove(InventoryEntry item)
    {
        if (item == null)
        {
            Debug.LogError("Trying to remove null pointer from inventory!");
            return null;
        }

        for (int i = 0; i < maxSize; i++)
        {
            if (_items[i] == item)
            {
                InventoryEntry oldItem = _items[i];
                _items[i] = null;

                InvokeOnItemChangedCallback();
                return oldItem;
            }
        }
        return null;
    }

    public InventoryEntry RemoveAt(int index)
    {
        if (index >= maxSize || index < 0)
        {
            throw new IndexOutOfRangeException("Inventory index out of bounds");
        }

        InventoryEntry oldItem = _items[index];
        _items[index] = null;
        InvokeOnItemChangedCallback();
        return oldItem;
    }

    private void checkFull()
    {
        bool foundEmpty = false;
        for(int i=0; i<maxSize && !foundEmpty; i++)
        {
            foundEmpty = foundEmpty || _items[i] == null;
        }
        filled = !foundEmpty;
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