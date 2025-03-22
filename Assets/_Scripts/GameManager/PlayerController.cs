using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance {get; private set;}  // singleton - globally accesible class with only 1 instance
    private GameObject _player;
    private PlayerActions _playerActions;

    private PlayerMovementStateManager _mngrMovementState;

    /// <summary>
    /// create instance on Awake
    /// </summary>
    public void OnAwake(){

        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    /// <summary>
    /// execute on Start
    /// </summary>
    public void OnStart()
    {
        CreatePlayer();
        _playerActions.OnStart();
    }

    /// <summary>
    /// execute on Updatepdate in GameFlow
    /// </summary>
    public void OnFrameUpdate()
    {
        //_playerActions.OnFrameUpdate();
        _mngrMovementState.OnFrameUpdate();
    }

    /// <summary>
    /// execute on FixedUpdate in GameFlow
    /// </summary>
    public void OnFixedUpdate()
    {
        _playerActions.OnFixedUpdate();
    }


    #region Init

    /// <summary>
    /// Create the Player
    /// </summary>
    private void CreatePlayer(){
        InitPlayer();
        InitPlayerComponents();
        _mngrMovementState.OnStart();
    }

    /// <summary>
    /// Find Player Object
    /// </summary>
    private void InitPlayer(){
        _player = GameObject.FindWithTag(GameManager.Instance.Tag_player); 
        if(_player == null){
            string logMessage = "Object with player tag: '" + GameManager.Instance.Tag_player + "' not found";
            Debug.LogError(logMessage, _player);
        }
    }

    /// <summary>
    /// Initialize all player components
    /// </summary>
    private void InitPlayerComponents(){       
        if(_player == null){
            Debug.LogError("Player Not Found");
            return;
        }

        // get components
        _playerActions = _player.GetComponent<PlayerActions>();
        _mngrMovementState = _player.GetComponent<PlayerMovementStateManager>();
        _mngrMovementState.OnAwake();
        // player health, statts and so on
    }     
    #endregion
    

}
