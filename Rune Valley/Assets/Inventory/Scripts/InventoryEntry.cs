using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InventoryEntry
{
    private Item _entryItem;
    public Item entryItem {
        get { return _entryItem;  }
    }

    private int _itemCount;
    public int itemCount {
        get { return _itemCount; }
    }

    public InventoryEntry(Item entry, int count)
    {
        this._entryItem = entry;
        this._itemCount = count;
    }

    public bool IsEmpty()
    {
        return this._itemCount <= 0;
    }

    public InventoryEntry SplitEntry()
    {
        if (!this.IsEmpty())
        {
            int otherItemCount = _itemCount / 2;
            _itemCount = _itemCount - otherItemCount;
            return new InventoryEntry(this._entryItem, otherItemCount);
        }
        Debug.LogError("No items left to split! This entry should have been deleted");
        return null;
    }

    public bool MergeInventory(InventoryEntry other)
    {
        if(this.equals(other))
        {
            this._itemCount += other.itemCount;
            return true;
        }
        Debug.LogError("Error Merging ivnentory entries: Mismatched Item types!");
        return false;
    }

    public InventoryEntry PopItem()
    {
        return this.PopItem(1);
    }

    public InventoryEntry PopItem(int count)
    {
        if (this.IsEmpty())
        {
            Debug.LogError("No items left to be popped! This entry should have been deleted");
            return null;
        }

        if(count == 0)
        {
            Debug.LogWarning("Trying to pop 0 items");
            return null;
        }

        if(count > this._itemCount)
        {
            Debug.LogWarning("Trying to pop more items than this entry contains!");
        }

        int popCount = count > this._itemCount || count == -1 ? this._itemCount : count;

        this._itemCount -= popCount;
        return new InventoryEntry(this._entryItem, popCount);
    }

    public void Use()
    {
        this._itemCount -= 1;
        this.entryItem.Use();
    }

    public bool equals(InventoryEntry other)
    {
        return this._entryItem == other.entryItem;
    }
}
