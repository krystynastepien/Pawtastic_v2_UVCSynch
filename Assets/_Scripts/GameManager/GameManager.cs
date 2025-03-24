using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // singleton - globally accesible class with only 1 instance

    #region PlayerStats
    #region Fields
    [SerializeField] private string tag_player = "Player";
    [SerializeField] private string tag_water = "Water"; // water 
    #endregion

    #region Properties
    public string Tag_player
    {
        get { return tag_player; }
    }
    public string Tag_water
    {
        get { return tag_water; }
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
