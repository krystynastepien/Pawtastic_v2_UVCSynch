using UnityEngine;

[CreateAssetMenu(fileName = "AnimationParameters", menuName ="ScriptableObjects/AnimationParameters", order = 0)]
public class AnimationParameters : ScriptableObject
{
     #region Fields
        [SerializeField] private string _horizontalParam = "xVelocity_AnimatorParam";
        [SerializeField] private string _verticalParam = "yVelocity_AnimatorParam";
        [SerializeField] private string _isMovingParam = "isMoving_AnimatorParam";
        #endregion


        #region Properties
        public string HorizontalParam{
            get { return _horizontalParam; }
        }
        public string VerticalParam{
            get { return _verticalParam; }
        }
        public string IsMovingParam{
            get { return _isMovingParam; }
        }
        #endregion 
}
