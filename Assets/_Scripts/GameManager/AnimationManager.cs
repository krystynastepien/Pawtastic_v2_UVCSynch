using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance {get; private set;} // singleton - globally accesible class with only 1 instance

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


    // create instance on Awake
    public void OnAwake(){

        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}
