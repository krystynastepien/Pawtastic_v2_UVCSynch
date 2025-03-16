using UnityEngine;

[CreateAssetMenu(fileName = "GameParameters", menuName ="ScriptableObjects/GameParameters", order = 0)]
public class GameParameters : ScriptableObject
{
    #region Field
    [SerializeField] private float player_BaseSpeed = 5;
    #endregion



    #region Properties
    public float Player_BaseSpeed{
        get { return player_BaseSpeed; }
    }

    #endregion 



}
