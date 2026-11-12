using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(AnimationManager))]
public class GameFlow : MonoBehaviour
{
    //public static GameFlow Instance { get; private set; }  // singleton - globally accesible class with only 1 instance   
    [SerializeField] private GameManager _gameMngr;
    [SerializeField] private PlayerManager _playerMngr;
    [SerializeField] private HUDManager _hudMngr;
    [SerializeField] private AnimationManager _animationMngr;

    private IManager[] _managersTable;

    private void Awake()
    {
        // get all manager objects
        GetManagerComponents();

        // kolejność w tablicy to kolejność wywołań // tablica  klas IManager - moje menadzery dziedzicza po tej klasie
        _managersTable = new IManager[]    
        {  
            _gameMngr, 
            _playerMngr, 
            _hudMngr, 
            _animationMngr 
        };

        foreach (IManager manager in _managersTable) {  
            manager.OnAwake();    // initialize  singletons in loop
        }

        // initialize all other singletons    
        // _gameMngr.OnAwake();
        // _playerMngr.OnAwake();
        // _hudMngr.OnAwake();
        // _animationMngr.OnAwake();
    }

    private void Start()
    { 
        foreach (IManager manager in _managersTable)
        {
            manager.OnStart();
        }  
    }

    private void FixedUpdate()  // Update is called 50 times per second (not once per frame like Update) - for physics
    { 
        foreach (IManager manager in _managersTable)
        {
            manager.OnFixedUpdate();
        }
    }
    
    private void Update() // Update is called once per frame - for animation
    { 
        foreach (IManager manager in _managersTable)
        {
            manager.OnFrameUpdate();
        }
    }


    // void Start()
    // {
    //     _playerMngr.OnStart();
    //     _hudMngr.OnStart();
    // }

    // void FixedUpdate()    // Update is called 50 times per second (not once per frame like Update) - for physics
    // {
    //     _playerMngr.OnFixedUpdate();
    // }

    // void Update()    // Update is called once per frame - for animation
    // {
    //     _playerMngr.OnFrameUpdate();
    // }


    /// <summary>
    /// initialize all manager components in the GameFlow object
    /// </summary>
    private void GetManagerComponents(){
        _gameMngr = GetComponent<GameManager>();
        _playerMngr = GetComponent<PlayerManager>();
        _hudMngr = GetComponent<HUDManager>();

        _animationMngr = GetComponent<AnimationManager>();

        LogManagerComponentInitStatus();        
    }


    #region Helper

    /// <summary>
    /// method logs once the state of managers init - whether every component was found and if not, which failed
    /// </summary>
    private void LogManagerComponentInitStatus()
    {
        List<string> missingManagers = new List<string>();

        if (_gameMngr == null) missingManagers.Add(nameof(GameManager));
        if (_playerMngr == null) missingManagers.Add(nameof(PlayerManager));
        if (_hudMngr == null) missingManagers.Add(nameof(HUDManager));
        if (_animationMngr == null) missingManagers.Add(nameof(AnimationManager));

        if (missingManagers.Count > 0)
        {
            string msg = $"Missing managers: {string.Join(", ", missingManagers)}";
            GameLogger.LogWarning(msg, LoggerConfig.LogCategoryFlags.System, this);
            return;
        }

        GameLogger.Log(
            "All managers' component initialization successful.",
            LoggerConfig.LogCategoryFlags.System,
            this
        );
    }

    #endregion
}