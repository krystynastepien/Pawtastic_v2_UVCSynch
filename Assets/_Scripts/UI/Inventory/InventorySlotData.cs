using System;
using UnityEngine;
 
/// <summary>
/// Runtime state of one inventory slot
/// Serializable so it can also be used as a mockup entry in the Inspector.
/// </summary>
[Serializable]
public class InventorySlotData
{
    public ItemData Item;
    [Min(1)] public int Quantity = 1;

    public bool IsEmpty
    {
        get { return Item == null || Quantity <= 0; }
    }
 
    public InventorySlotData() { }
 
    public InventorySlotData(ItemData item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
 
    public void Set(ItemData item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
 
    public void Clear()
    {
        Item = null;
        Quantity = 0;
    }
}