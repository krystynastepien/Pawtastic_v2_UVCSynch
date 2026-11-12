using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>, IManager
{
    #region Fields
    private bool _isReady;
    private GameObject _player;
    private PlayerActions _playerActions;
    private PlayerMovementStateManager _mngrMovementState;
    public PlayerHealthController _playerHealth;

    
    #endregion

    #region IManager Lifecycle Methods (GameFlow)
    public void OnAwake()
    {
        base.Initialize(); // Call the singleton logic
        //_isReady = true;
    }

    public void OnStart()
    {
         _isReady = CreatePlayer();
        if (!_isReady) return;    
        
        _mngrMovementState.OnStart();
        _playerHealth.OnStart();
        _playerActions.OnStart();
    }

    public void OnFrameUpdate()
    {
        if (!_isReady) return;
        //_playerActions.OnFrameUpdate();
        _mngrMovementState.OnFrameUpdate();
    }

    public void OnFixedUpdate()
    {
        if (!_isReady) return;
        _playerActions.OnFixedUpdate();
    }
    #endregion

    #region Initialization
    private bool CreatePlayer()
    {   
        return InitPlayer() && InitPlayerComponents();
    }

    /// <summary>
    /// Find Player Object
    /// </summary>
    private bool InitPlayer()
    {
        _player = GameObject.FindWithTag(Tags.Player); 
        if(_player == null)
        {
            string msg = "Object with player tag: '" + Tags.Player + "' not found";
            GameLogger.LogCritical(msg, LoggerConfig.LogCategoryFlags.Player, _player);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Initialize all player components
    /// </summary>
    private bool InitPlayerComponents()
    {       
        //Pobierz komponenty
        if (!_player.TryGetComponent(out _playerActions))
        {
            LogMissingComponent(nameof(PlayerActions));
            return false;
        }

        if (!_player.TryGetComponent(out _mngrMovementState))
        {
            LogMissingComponent(nameof(PlayerMovementStateManager));
            return false;
        }

        if (!_player.TryGetComponent(out _playerHealth))
        {
            LogMissingComponent(nameof(PlayerHealthController));
            return false;
        }

        return true;
        //_playerStats = new PlayerStats(maxHealth: 10, attack: 5, defense: 2);        
    }
    #endregion

    #region Helpers

    /// <summary>
    /// Loguje błąd o brakującym komponencie na obiekcie Player
    /// </summary>
    private void LogMissingComponent(string componentName)
    {
        string msg = "Player object is missing component: " + componentName;
        GameLogger.LogWarning(msg, LoggerConfig.LogCategoryFlags.Player, _player);
    }

    #endregion

}