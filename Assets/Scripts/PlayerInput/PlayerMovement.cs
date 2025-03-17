using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    // game const settings
    #region const 
    private float playerSpeed;
    #endregion

    #region Player State

    private bool isMoving;
        
    #endregion

    private Rigidbody2D rigidBody; 
    private Vector2 moveInput;
    private Animator animator;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSpeed = GameManager.Instance.Player_BaseSpeed;
        isMoving = false;
    }

    // Update is called 50 times per second (not once per frame like Update)
    void FixedUpdate()
    {
        /* //legacy code - now using Player Input component
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            rigidBody.linearVelocity = new Vector2(horizontal, vertical) * gameParameters.PlayerSpeed; 
        */
        rigidBody.linearVelocity = moveInput * playerSpeed; 
        if(moveInput != Vector2.zero)
            isMoving = true;
        else
            isMoving = false;
        

        // animation walk <-> idle update
        animator.SetBool(AnimationManager.Instance.IsMovingParam, isMoving);
    }

    #region Player Actions


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //movement animation
        AnimateMovement(moveInput);
    }

    #endregion

    #region Player Animations

    private void AnimateMovement(Vector2 moveInput){
        if(moveInput != Vector2.zero){
            animator.SetFloat(AnimationManager.Instance.HorizontalParam, moveInput.x);
            animator.SetFloat(AnimationManager.Instance.VerticalParam, moveInput.y);
        }
    }
        
    #endregion
}
