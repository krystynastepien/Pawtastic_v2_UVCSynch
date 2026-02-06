using UnityEngine;

public static class PlayerStats
{
    #region Health & Lives
    public const int MAX_HEALTH = 5;
    public const int MAX_LIVES = 3; //9;
    #endregion

    #region Combat Stats
    public const int BASE_ATTACK = 5;
    public const int BASE_DEFENSE = 2;
    #endregion

    #region Movement Stats
    public const float BASE_SPEED = 5f;
    //public const float RUN_SPEED = 8f;
    //public const float JUMP_FORCE = 10f;
    #endregion

    // #region Animation Parameters
    // public const string ANIM_HORIZONTAL_VELOCITY = "xVelocity_AnimatorParam";
    // public const string ANIM_VERTICAL_VELOCITY = "yVelocity_AnimatorParam";
    // public const string ANIM_IS_MOVING = "isMoving_AnimatorParam";
    // #endregion
}