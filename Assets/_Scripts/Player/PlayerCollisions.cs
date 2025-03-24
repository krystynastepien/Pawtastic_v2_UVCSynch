using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
      ProcessCollision(collision.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision.gameObject);

    }

    void ProcessCollision(GameObject collider){
        
        //switch for different tags
        if (collider.CompareTag(GameManager.Instance.Tag_water))
        {
            PlayerController.Instance.PlayerStats.TakeDamage(damageVal: 2);
        }
    }
}
