using UnityEngine;

[CreateAssetMenu(fileName = "GameParameters", menuName ="ScriptableObjects/GameParameters", order = 0)]
public class GameParameters : ScriptableObject
{
    #region PlayerStats
        #region Field
        [SerializeField] private float player_BaseSpeed = 5;
        [SerializeField] private string tag_player = "Player";
        #endregion

        #region Properties
        public float Player_BaseSpeed{
            get { return player_BaseSpeed; }
        }
        public string Tag_player{
            get { return tag_player; }
        }

        #endregion 
    #endregion 

}
