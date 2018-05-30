using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Inventory))]
[CanEditMultipleObjects]
public class NewBehaviourScript : Editor {

    Inventory inventory;

    private int _inventorySize;

    private void OnEnable()
    {
        inventory = (Inventory)target;
        _inventorySize = inventory.maxSize;

        //inventory.onItemChangedCallback += DrawInventoryList;
        //inventory.onSizeChangedCallback += DrawInventoryList;
    }

    public override void OnInspectorGUI()
    {
        int newSize = EditorGUILayout.IntField("Inventory Size", _inventorySize);
        if (_inventorySize != newSize)
        {
            _inventorySize = newSize;
            inventory.SetSize(_inventorySize);
        }

        DrawInventoryList();
    }

    void DrawInventoryList()
    {
        for(int i=0; i<inventory.maxSize; i++)
        {
            InventoryEntry entry = inventory.items != null ? inventory.items[i] : null;
            string entryLabel = "Entry " + i + ": ";
            entryLabel += entry == null ? "EMPTY" :  entry.entryItem+", "+entry.itemCount;

            EditorGUILayout.LabelField(entryLabel);
        }
    }

}
