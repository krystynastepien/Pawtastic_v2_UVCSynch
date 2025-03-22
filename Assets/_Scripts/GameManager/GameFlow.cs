using UnityEngine;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(AnimationManager))]
public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }  // singleton - globally accesible class with only 1 instance   
    [SerializeField] private GameManager _gameMngr;
    [SerializeField] private PlayerController _playerMngr;
    [SerializeField] private AnimationManager _animationMngr;

    private void Awake()
    {
        // initialize this singleton
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        // get all manager objects
        InitManagerObjects();

        // initialize all other singletons    
        _gameMngr.OnAwake();
        _playerMngr.OnAwake();
        _animationMngr.OnAwake();
    }

    void Start()
    {
        _playerMngr.OnStart();
    }

    void FixedUpdate()    // Update is called 50 times per second (not once per frame like Update) - for physics
    {
        _playerMngr.OnFixedUpdate();
    }

    void Update()    // Update is called once per frame - for animation
    {
        _playerMngr.OnFrameUpdate();
    }


    /// <summary>
    /// initialize all manager components in the GameFlow object
    /// </summary>
    private void InitManagerObjects(){
        _gameMngr = GetComponent<GameManager>();
        _playerMngr = GetComponent<PlayerController>();
        _animationMngr = GetComponent<AnimationManager>();
    }
}
