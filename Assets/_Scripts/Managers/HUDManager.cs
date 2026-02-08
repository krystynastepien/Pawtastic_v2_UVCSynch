using UnityEngine;

public class HUDManager : Singleton<HUDManager>, IManager
{
    #region Serialized Fields
        [SerializeField] private HealthBarController _healthBarController;

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
        _healthBarController.OnStart();
        SubscribeToEvents();
    }

    public void OnFixedUpdate()
    {
    }

    public void OnFrameUpdate()
    {
    }
    #endregion



    #region Public Methods
    /// <summary>
    /// rerender HUD healthbar on health change
    /// </summary>
    public void UpdateHUD_OnHealthChange()
    {
        _healthBarController.OnUpdateHeartPanel();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// assign delegates
    /// </summary>
    private void SubscribeToEvents()
    {
        // subscribe to OnHealthChangedEvent to update HUD on health change
        PlayerManager.Instance._playerHealth.OnHealthChangedEvent += UpdateHUD_OnHealthChange;
    }
    
    
    // Your private helper methods here
    private void OnDestroy()
    {
        //Unsubscribe to prevent memory leaks
        if (PlayerManager.Instance._playerHealth != null)
        {
            PlayerManager.Instance._playerHealth.OnHealthChangedEvent -= UpdateHUD_OnHealthChange;
        }
    }
    #endregion
}