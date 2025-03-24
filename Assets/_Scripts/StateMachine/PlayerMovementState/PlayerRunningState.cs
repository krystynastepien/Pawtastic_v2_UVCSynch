using UnityEngine;

public class PlayerRunningState : PlayerMovementBaseState
{
    public override void EnterState(PlayerMovementStateManager mgr){
        //Debug.Log("player entered Running state");
        mgr.Animator.SetBool(AnimationManager.Instance.IsMovingParam, true);
    }

    public override void  UpdateState_Frame(PlayerMovementStateManager mgr){
        if( mgr.PlayerActions.MoveInput != Vector2.zero){
            mgr.Animator.SetFloat(AnimationManager.Instance.HorizontalParam, mgr.PlayerActions.MoveInput.x);
            mgr.Animator.SetFloat(AnimationManager.Instance.VerticalParam, mgr.PlayerActions.MoveInput.y);
        }
        else{
            mgr.ChangeState(mgr.IdleState);
        }
    }

    public override void  UpdateState_Physics(PlayerMovementStateManager mgr){
    }

    public override void ExitState(PlayerMovementStateManager mgr){
        mgr.Animator.SetBool(AnimationManager.Instance.IsMovingParam, false);
    }

}
