using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    #region Consts
    #endregion consts


    #region Serialized Fields
    [SerializeField] private InventoryItem _itemPrefab;
    [SerializeField] private RectTransform _contentPanel;
    #endregion SerializedFields

    #region Fields

    private GameObject _inventoryCanvas;
    private List<InventoryItem> _inventoryItems = new List<InventoryItem>();

    #endregion Fields

    #region Lifecycle Methods (GameFlow)
    public void OnStart()
    {
        // get inventory parent - Inventory Canvas HUD
        _inventoryCanvas = this.transform.parent.gameObject;

        ///create empty inventory with 7 slots
        InitializeInventoryUI(7);
    }
    #endregion Lifecycle Methods (GameFlow)



    #region public methods

    /// <summary>
    /// show or hide the inventory window
    /// </summary>
    public void OnShowOrHide()
    {
        _inventoryCanvas.SetActive(!_inventoryCanvas.activeSelf);
    }
    
    #endregion public methods

    #region private methods

    /// <summary>
    ///  Initialize item slot objects in panel with set amount of inventory size
    /// </summary>
    /// <param name="heartCount">amount of hearts to create</param>
    private void InitializeInventoryUI(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
           CreateItemSlot();
        }
    }

    #endregion private methods
    /// <summary>
    /// Create copy of item slot object 
    /// </summary>
    /// <param name="fullHeartSprite"></param>
    /// <returns></returns>
    private InventoryItem CreateItemSlot()
    {
        // create prefab object
        InventoryItem newSlot = Instantiate(_itemPrefab, _contentPanel.transform);
        if (newSlot == null)
        {
            GameLogger.LogError("Cannot create new item slot object for HUD Inventory.", LoggerConfig.LogCategoryFlags.UI, newSlot);
            return null;
        }

        // Get existing Image component ?

        _inventoryItems.Add(newSlot);
        return newSlot;
    }



    
    #region helpers
    #endregion helpers

}
