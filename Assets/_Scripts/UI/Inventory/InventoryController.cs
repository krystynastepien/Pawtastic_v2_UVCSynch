using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Owns the PlayerInventory (data) and keeps the slot views (UI) in sync with it
/// </summary>
public class InventoryController : MonoBehaviour
{
    #region Consts
    #endregion consts


    #region Serialized Fields
    [Header("UI")]
    [SerializeField] private InventoryItemSlot _itemPrefab;
    [SerializeField] private RectTransform _contentPanel;
    [SerializeField] private InventoryItemDescription _descriptionPanel;
 
    [Header("Data")]
    [Tooltip("All items that exist in the game (used by the TEST helpers, later by save/load)")]
    //[SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField, Min(1)] private int _startingSlotCount = 8;
 
    [Header("MOCKUP")]
    [Tooltip("Items the player starts with (play mode only). Edit the list to test stacking / overflow.")]
    [SerializeField] private List<InventorySlotData> _mockupStartingItems = new List<InventorySlotData>();
 
    #endregion SerializedFields


    #region Fields
    private GameObject _inventoryCanvas;
    private PlayerInventory _inventory;
    private readonly List<InventoryItemSlot> _slotViews = new List<InventoryItemSlot>();
    private InventoryItemSlot _selectedSlot;
    private InventoryItemSlot _draggedSlot;
    private bool _isVisible;
    #endregion Fields
 

    #region Properties
    public bool IsVisible {get => _isVisible; private set => _isVisible = value; }
    public PlayerInventory Inventory => _inventory;
    #endregion


    #region Lifecycle Methods (GameFlow)
    public void OnStart()
    {
        // get inventory parent - Inventory Canvas HUD
        _inventoryCanvas = this.transform.parent.gameObject;
        if (!_inventoryCanvas.CompareTag(EditorObjectTag.Canvas))
        {
            GameLogger.LogWarning("InventoryController's parent is not Canvas, it won's show or hide the inventory by pressing I", LoggerConfig.LogCategoryFlags.UI, this);
        }
 
        // data first, then the views that display it
        _inventory = new PlayerInventory(_startingSlotCount);
        foreach (InventorySlotData entry in _mockupStartingItems)
        {
            int left = _inventory.TryAdd(entry.Item, entry.Quantity);
            if (left > 0)
                GameLogger.LogWarning($"MOCKUP: {left}x {(entry.Item != null ? entry.Item.Name : "<none>")} did not fit in the inventory.", LoggerConfig.LogCategoryFlags.UI, this);
        }
 
        InitializeInventoryUI(_inventory.Capacity);
        _inventory.OnChanged += Refresh;
        Refresh();
 
        GameLogger.Log("MOCKUP: RENDERING INVENTORY WITH " + _inventory.Capacity + " SLOTS.", LoggerConfig.LogCategoryFlags.UI | LoggerConfig.LogCategoryFlags.Player, this);
 
        OnHide(); // hide inventory onStart
    }
 
    private void OnDestroy()
    {
        if (_inventory != null) _inventory.OnChanged -= Refresh;
    }
    #endregion Lifecycle Methods (GameFlow)



     #region public methods
 
    /// <summary>
    /// hide the inventory window
    /// </summary>
    public void OnHide()
    {
        _inventoryCanvas.SetActive(false);
        ShowDescription(null);
        _isVisible = false;
    }
 
    /// <summary>
    /// show the inventory window
    /// </summary>
    public void OnShow()
    {
        _isVisible = true;
        
        _inventoryCanvas.SetActive(true);
    }
 
    /// <summary>
    /// Entry point for pickups, rewards, vendors... Returns how many items did NOT fit.
    /// </summary>
    public int TryAddItem(ItemData item, int amount = 1)
    {
        if (_inventory == null)
        {
            Debug.LogWarning("InventoryController.TryAddItem called before OnStart.", this);
            return amount;
        }
        return _inventory.TryAdd(item, amount);
    }
 
    /// <summary>
    /// Slot upgrade (NPC / item): adds data slots AND their views.
    /// </summary>
    public void ExpandSlots(int extraSlots)
    {
        _inventory.ExpandSlots(extraSlots, notify: false);
        for (int i = 0; i < extraSlots; i++)
            _slotViews.Add(CreateItemSlot());
        Refresh();
    }
 
    [ContextMenu("TEST: Add random item from database")]
    public void TEST_AddRandomItem()
    {
    //     if (_inventory == null || _itemDatabase == null)
    //     {
    //         Debug.LogWarning("TEST_AddRandomItem: needs play mode + assigned ItemDatabase.", this);
    //         return;
    //     }
    //     ItemData item = _itemDatabase.GetRandom();
    //     int left = TryAddItem(item, UnityEngine.Random.Range(1, 4));
    //     Debug.Log($"TEST: added {(item != null ? item.Name : "<none>")}, {left} did not fit.", this);
     }
    
    #endregion public methods
 
    #region private methods
 
    /// <summary>
    ///  Initialize item slot objects in panel with set amount of inventory size
    /// </summary>
    private void InitializeInventoryUI(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
           _slotViews.Add(CreateItemSlot());
        }
        if (_descriptionPanel != null) _descriptionPanel.OnInit();
    }
 
    /// <summary>
    /// null = clear. (Explicit null check on purpose: '?.' bypasses Unity's overloaded null.)
    /// </summary>
    private void ShowDescription(ItemData item)
    {
        if (_descriptionPanel == null) return;
 
        if (item == null)
            _descriptionPanel.ClearDescription();
        else
            _descriptionPanel.SetDescription(item);
    }
 
    /// <summary>
    /// Create copy of item slot object 
    /// </summary>
    private InventoryItemSlot CreateItemSlot()
    {
        // create prefab object
        InventoryItemSlot newSlot = Instantiate(_itemPrefab, _contentPanel.transform);
        if (newSlot == null)
        {
            GameLogger.LogWarning("Cannot create new item slot object for HUD Inventory.", LoggerConfig.LogCategoryFlags.UI, newSlot);
            return null;
        }
        newSlot.OnInit();
        SubscribeToEvents(newSlot);
 
        return newSlot;
    }
 
    /// <summary>
    /// Data -> UI. Called every time PlayerInventory changes.
    /// </summary>
    private void Refresh()
    {
        for (int i = 0; i < _slotViews.Count; i++)
        {
            _slotViews[i].Render(_inventory.ItemSlots[i]);
        }
 
        // keep the description page in sync with whatever is in the selected slot now
        if (_selectedSlot != null)
        {
            ItemData item = _selectedSlot.GetItemData();
            if (item == null)
            {
                _selectedSlot.Deselect();
                _selectedSlot = null;
            }
            ShowDescription(item);
        }
    }
 
    /// <summary>
    /// assign delegates
    /// </summary>
    private void SubscribeToEvents(InventoryItemSlot itemSlot)
    {
        itemSlot.OnLeftMouseBtnClick += HandleItemSelection;
        itemSlot.OnRightMouseBtnClick += HandleShowItemActions;
        itemSlot.OnItemBeginDrag += HandleBeginDrag;
        itemSlot.OnItemDrag += HandleDrag;
        itemSlot.OnItemEndDrag += HandleEndDrag;
        itemSlot.OnItemDroppedOn += HandleItemsSwap;
    }
 
        #region drag and drop handlers
    private void HandleItemSelection(InventoryItemSlot slot)
    {
        if (_selectedSlot != null) _selectedSlot.Deselect();
 
        if (slot.IsEmpty)
        {
            _selectedSlot = null;
            ShowDescription(null);
            return;
        }
 
        _selectedSlot = slot;
        slot.Select();
        ShowDescription(slot.GetItemData());
    }
 
    private void HandleBeginDrag(InventoryItemSlot slot)
    {
        _draggedSlot = slot;
    }
 
    private void HandleDrag(InventoryItemSlot slot)
    {
        if (slot != _draggedSlot) return;
        slot.SetItemImagePosition(Input.mousePosition);
    }
 
    private void HandleEndDrag(InventoryItemSlot slot)
    {
        _draggedSlot = null;
    }
 
    private void HandleShowItemActions(InventoryItemSlot slot)
    {
        // TODO: toggle Use / Drop button panel (slot.UseButton / slot.DropButton)
        GameLogger.Log("TODO: item actions for " + slot.name, LoggerConfig.LogCategoryFlags.UI, slot);
    }
 
    private void HandleItemsSwap(InventoryItemSlot targetSlot)
    {
        if (_draggedSlot == null || _draggedSlot == targetSlot) return;
 
        int from = _slotViews.IndexOf(_draggedSlot);
        int to = _slotViews.IndexOf(targetSlot);
        _inventory.Swap(from, to); // fires OnChanged -> Refresh
    }
 
        #endregion drag and drop handlers
    #endregion private methods


    #region helpers
    #endregion helpers

}
