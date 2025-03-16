using UnityEngine;

[CreateAssetMenu(fileName = "GameParameters", menuName ="ScriptableObjects/GameParameters", order = 0)]
public class GameParameters : ScriptableObject
{
    #region Field
    [SerializeField] private float playerSpeed = 5;
    #endregion



    #region Properties
    public float PlayerSpeed{
        get { return playerSpeed; }
    }

    #endregion 



}
