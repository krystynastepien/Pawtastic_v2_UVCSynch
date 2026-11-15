using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// What the player currently owns
/// - Normal items: limited slots, stacking up to ItemData.MaxStackSize.
/// - Quest items: separate list, do NOT take slots for now 
/// TODO: seperate inventory tab for quest items
/// </summary>
public class PlayerInventory
{
   
    public event Action OnChanged; //something in inventory changed

    #region Fields
    private readonly List<InventorySlotData> _itemSlots = new List<InventorySlotData>();
    private readonly List<InventorySlotData> _questItems = new List<InventorySlotData>();
 
    public IReadOnlyList<InventorySlotData> ItemSlots { get { return _itemSlots; } }
    public IReadOnlyList<InventorySlotData> QuestItems { get { return _questItems; } }
    public int Capacity { get { return _itemSlots.Count; } }
 
    #endregion

    
    public PlayerInventory(int capacity)
    {
        ExpandSlots(capacity, notify: false);
    }

    #region Methods

     /// <summary>
    /// Upgrade: more slots (NPCs / items).
    /// </summary>
    public void ExpandSlots(int extraSlots, bool notify = true)
    {
        for (int i = 0; i < extraSlots; i++)
            _itemSlots.Add(new InventorySlotData());
 
        if (notify && extraSlots > 0) OnChanged?.Invoke();
    }
        
    #endregion


     /// <summary>
    /// Adds an item. Returns how many did NOT fit (0 = everything added).
    /// </summary>
    public int TryAdd(ItemData item, int amount)
    {
        if (item == null || amount <= 0) return amount;
 
        if (item.IsQuestItem)
        {
            AddQuestItem(item, amount);
            return 0;
        }
 
        int max = Mathf.Max(1, item.MaxStackSize);
        int remaining = amount;
 
        // 1. top up existing stacks
        foreach (InventorySlotData slot in _itemSlots)
        {
            if (remaining <= 0) break;
            if (slot.IsEmpty || slot.Item != item || slot.Quantity >= max) continue;
 
            int add = Mathf.Min(max - slot.Quantity, remaining);
            slot.Quantity += add;
            remaining -= add;
        }
 
        // 2. fill empty slots
        foreach (InventorySlotData slot in _itemSlots)
        {
            if (remaining <= 0) break;
            if (!slot.IsEmpty) continue;
 
            int add = Mathf.Min(max, remaining);
            slot.Set(item, add);
            remaining -= add;
        }
 
        if (remaining != amount) OnChanged?.Invoke();
        return remaining;
    }
 
    public bool TryRemoveAt(int index, int amount = 1)
    {
        if (!IsValidIndex(index) || amount <= 0) return false;
 
        InventorySlotData slot = _itemSlots[index];
        if (slot.IsEmpty || slot.Quantity < amount) return false;
 
        slot.Quantity -= amount;
        if (slot.Quantity <= 0) slot.Clear();
 
        OnChanged?.Invoke();
        return true;
    }
 
    /// <summary>
    /// Drag and drop: same item = merge stacks, otherwise swap places.
    /// </summary>
    public void Swap(int from, int to)
    {
        if (from == to || !IsValidIndex(from) || !IsValidIndex(to)) return;
 
        InventorySlotData a = _itemSlots[from];
        InventorySlotData b = _itemSlots[to];
        if (a.IsEmpty) return;
 
        if (!b.IsEmpty && a.Item == b.Item)
        {
            int max = Mathf.Max(1, a.Item.MaxStackSize);
            int moved = Mathf.Min(max - b.Quantity, a.Quantity);
            if (moved > 0)
            {
                b.Quantity += moved;
                a.Quantity -= moved;
                if (a.Quantity <= 0) a.Clear();
                OnChanged?.Invoke();
                return;
            }
        }
 
        ItemData tmpItem = a.Item;
        int tmpQty = a.Quantity;
        a.Set(b.Item, b.Quantity);
        b.Set(tmpItem, tmpQty);
        OnChanged?.Invoke();
    }
 
    private void AddQuestItem(ItemData item, int amount)
    {
        InventorySlotData existing = _questItems.Find(s => s.Item == item);
        if (existing != null)
            existing.Quantity += amount;
        else
            _questItems.Add(new InventorySlotData(item, amount));
 
        OnChanged?.Invoke();
    }
 
    private bool IsValidIndex(int index) => index >= 0 && index < _itemSlots.Count;
    
}
