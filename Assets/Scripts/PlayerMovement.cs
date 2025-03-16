using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D rigidBody; 
 
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rigidBody.linearVelocity = new Vector2(horizontal, vertical);
    }
}
