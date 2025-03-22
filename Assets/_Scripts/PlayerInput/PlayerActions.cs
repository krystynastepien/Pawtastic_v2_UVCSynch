using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerActions : MonoBehaviour
{
    // game const settings
    #region const 
    private float playerSpeed;
    #endregion

    #region Player State

    private bool _isPlayerMoving;
        
    #endregion

    private Rigidbody2D _rigidBody; 
    private Vector2 _moveInput;
    private Animator _animator;


    public void OnStart()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        playerSpeed = GameManager.Instance.Player_BaseSpeed;
        _isPlayerMoving = false;
    }

    /// <summary>
    /// execute on Fixedupdate in GameFlow
    /// </summary>
    public void OnUpdate()
    {
        _rigidBody.linearVelocity = _moveInput * playerSpeed; 
        _isPlayerMoving = IsPlayerMoving();
        

        // animation walk <-> idle update
        _animator.SetBool(AnimationManager.Instance.IsMovingParam, _isPlayerMoving);
        AnimateMovement(_moveInput);

    }

    #region Player Actions Input collection
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        //movement animation
    }

    #endregion

    #region Player Animations

    private void AnimateMovement(Vector2 moveInput){
        if(moveInput != Vector2.zero){
            _animator.SetFloat(AnimationManager.Instance.HorizontalParam, moveInput.x);
            _animator.SetFloat(AnimationManager.Instance.VerticalParam, moveInput.y);
        }
    }
   
    #endregion

    #region Helpers

    /// <summary>
    /// Returns true if the movement engine detects move input different from 0
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerMoving(){
        if(_moveInput != Vector2.zero)
            return true;
        else
            return false;
    }
        
    #endregion
}
