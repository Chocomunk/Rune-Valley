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

    private InventoryEntry[] _items;
    public InventoryEntry[] items {
        get { return _items; }
    }

    private int _maxSize = 20;
    public int maxSize {
        get { return _maxSize; }
        set { SetSize(value);  }
    }

    public void Awake()
    {
        _items = new InventoryEntry[_maxSize];
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

        if(items == null)
        {
            _maxSize = size;
            return;
        }

        InventoryEntry[] newItems = new InventoryEntry[size];
        for (int i = 0; i < (_maxSize < size ? _maxSize : size) ; i++)
        {
            newItems[i] = _items[i];
        }
        _items = newItems;
        _maxSize = size;
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
            for (int i = 0; i < _maxSize && nullIndex < 0 && existingIndex < 0; i++)
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

        for (int i = 0; i < _maxSize; i++)
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

        if (_items[index] != null)
        {
            InventoryEntry oldItem = _items[index];
            int newCount = count < 0 || count > oldItem.itemCount? oldItem.itemCount : count;
            InventoryEntry newItem = oldItem.PopItem(newCount);
            if (oldItem.IsEmpty())
                _items[index] = null;

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