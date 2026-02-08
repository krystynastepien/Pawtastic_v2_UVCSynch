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
    }

    public void OnFixedUpdate()
    {
    }

    public void OnFrameUpdate()
    {
    }
    #endregion

    #region Public Methods
    // Your public methods here
    #endregion

    #region Private Methods
    // Your private helper methods here
    #endregion
}