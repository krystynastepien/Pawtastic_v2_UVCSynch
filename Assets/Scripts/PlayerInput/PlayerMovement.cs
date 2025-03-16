using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    #region const
    [SerializeField] // game const settings
    private GameParameters gameParameters;
    private float playerSpeed;
    #endregion

    private Rigidbody2D rigidBody; 
    private Vector2 moveInput;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerSpeed = gameParameters.Player_BaseSpeed;
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
    }

    #region Player Actions
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    #endregion
}
