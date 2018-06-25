using UnityEngine;

[System.Serializable]
public class InventoryEntry
{
    public delegate void OnCountChanged();
    public OnCountChanged OnCountChangedCallback;

    [UnityEngine.SerializeField] private Item _entryItem;
    public Item entryItem {
        get { return _entryItem;  }
    }

    [UnityEngine.SerializeField] private int _itemCount;
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
            // Ensure other item has larger quantity in case of odd count
            int newItemCount = _itemCount / 2;
            int otherItemCount = _itemCount - newItemCount;
            _itemCount = newItemCount;
            OnCountChangedCallback.Invoke();
            return new InventoryEntry(this._entryItem, otherItemCount);
        }
        Debug.LogError("No items left to split! This entry should have been deleted");
        return null;
    }

    public bool MergeEntry(InventoryEntry other)
    {
        if(this.equals(other))
        {
            this._itemCount += other.itemCount;
            OnCountChangedCallback.Invoke();
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
        OnCountChangedCallback.Invoke();
        return new InventoryEntry(this._entryItem, popCount);
    }

    public bool equals(InventoryEntry other)
    {
        return other != null && this._entryItem == other.entryItem;
    }
}
