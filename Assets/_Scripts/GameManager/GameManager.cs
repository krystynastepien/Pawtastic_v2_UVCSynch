using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // singleton - globally accesible class with only 1 instance

    #region PlayerStats
    #region Fields
    [SerializeField] private float player_BaseSpeed = 5;
    [SerializeField] private string tag_player = "Player";
    #endregion

    #region Properties
    public float Player_BaseSpeed
    {
        get { return player_BaseSpeed; }
    }
    public string Tag_player
    {
        get { return tag_player; }
    }
    #endregion
    #endregion


    /// <summary>
    /// create instance on Awake
    /// </summary>
    public void OnAwake()
    {

        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}
