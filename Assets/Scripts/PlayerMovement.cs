using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] 
    private Rigidbody2D rigidBody; 

    [SerializeField] 
    private GameParameters gameParameters;
 
    // Update is called 50 times per second (not once per frame like Update)
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rigidBody.linearVelocity = new Vector2(horizontal, vertical) * gameParameters.PlayerSpeed;
    }
}
