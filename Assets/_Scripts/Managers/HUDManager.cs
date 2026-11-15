using UnityEngine;

public class HUDManager : Singleton<HUDManager>, IManager
{
    #region Fields
        private bool _isReady;
        private HUD_HealthBarController _healthBarController;
        private InventoryController _inventoryController;
        private PlayerHealthController _subscribedHealth;
    #endregion

    #region Properties
    #endregion

    #region IManager Lifecycle Methods (GameFlow)
    public void OnAwake()
    {
        base.Initialize(); // Call the singleton logic
    }

    public void OnStart()
    {
        _isReady = InitHUDElements();
        if (!_isReady)
        {
            string msg = "Initializing HUDManager failed.";
            GameLogger.LogCritical(msg, LoggerConfig.LogCategoryFlags.UI, this);
            return;
        }    

        _subscribedHealth = PlayerManager.Instance.HealthCtrl;
        if (_subscribedHealth == null)
        {
            // PlayerManager.OnStart musi być wcześniej w tablicy GameFlow, inaczej Health == null
            GameLogger.LogCritical("HUDManager: player health not available.", LoggerConfig.LogCategoryFlags.UI | LoggerConfig.LogCategoryFlags.Player, this);
            return;
        }

        _healthBarController.OnStart();
        _inventoryController.OnStart();

        SubscribeToEvents();
        UpdateHUD_OnHealthChange(); // pierwsze odświeżenie, żeby pasek HealthBar pokazywał aktualny stan od załadowania
    }

    public void OnFixedUpdate()
    {
    }

    public void OnFrameUpdate()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            GameLogger.Log("User pressed I for inventory", LoggerConfig.LogCategoryFlags.UI, _inventoryController);
            ShowOrHide(UIElement.InventoryUI, !_inventoryController.IsVisible);
        }
    }
    #endregion


    #region Initialization

    /// <summary>
    /// initialize all HUD elements
    /// </summary>
    /// <returns></returns>
    private bool InitHUDElements()
    {   
        return InitHealthBarController() && InitInventoryController();
    }


    /// <summary>
    /// Find the HealthBar Object
    /// </summary>
    private bool InitHealthBarController()
    {
        // _healthBarController = GameObject.FindObjectsByType(
        //     typeof(HealthBarController), 
        //     FindObjectsInactive.Include, 
        //     FindObjectsSortMode.None)[0] as HealthBarController;
        _healthBarController = Object.FindFirstObjectByType<HUD_HealthBarController>(FindObjectsInactive.Include);
        if(_healthBarController == null)
        {
            LogMissingObject(nameof(HUD_HealthBarController));
            return false;
        }
        return true;
    }

    /// <summary>
    /// Find the Inventory object
    /// </summary>
    /// <returns></returns>
    private bool InitInventoryController()
    {
        // _inventoryController = GameObject.FindObjectsByType(
        //     typeof(InventoryController), 
        //     FindObjectsInactive.Include, 
        //     FindObjectsSortMode.None)[0] as InventoryController;
        _inventoryController = Object.FindFirstObjectByType<InventoryController>(FindObjectsInactive.Include);
        if(_inventoryController == null)
        {
            LogMissingObject(nameof(InventoryController));
            return false;
        }
        return true;
    }

    #endregion




    #region Public Methods
    /// <summary>
    /// rerender HUD healthbar on health change
    /// </summary>
    public void UpdateHUD_OnHealthChange()
    {
        //_healthBarController.OnUpdateHeartPanel();
        _healthBarController.OnUpdateHeartPanel(_subscribedHealth.CurrentHealth);
    }

    public InventoryController GetInventory()
    {
        return _inventoryController;
    }


    #endregion

    #region Private Methods
    /// <summary>
    /// assign delegates
    /// </summary>
    private void SubscribeToEvents()
    {
        // subscribe to OnHealthChangedEvent to update HUD on health change
        _subscribedHealth.OnHealthChangedEvent += UpdateHUD_OnHealthChange;
    }
    
    private void OnDestroy()
    {
        //Unsubscribe to prevent memory leaks
        if (_subscribedHealth != null){
            _subscribedHealth.OnHealthChangedEvent -= UpdateHUD_OnHealthChange;
        }
    }

    /// <summary>
    /// show or hide given HUD element
    /// </summary>
    /// <param name="element"></param>
    private void ShowOrHide(UIElement element, bool enable)
    {
        switch (element)
        {
            case UIElement.InventoryUI:
                if (enable)
                    _inventoryController.OnShow();
                else
                    _inventoryController.OnHide();              
                break;
            default:
                break;
        }
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Loguje błąd o brakującym elemencie HUD
    /// </summary>
    private void LogMissingObject(string elementName)
    {
        string msg = "HUD is missing object: " + elementName;
        GameLogger.LogWarning(msg, LoggerConfig.LogCategoryFlags.UI, this);
    }

    #endregion

}

/// <summary>
/// ui elements to control
/// </summary>
enum UIElement
{
    LifePanelUI,
    InventoryUI,
    PauseUI,
    MainMenuUI,
}