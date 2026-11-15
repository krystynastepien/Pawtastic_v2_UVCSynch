using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerHealthController _healthCtrl;
    private void Awake()
    {
        if (!TryGetComponent(out _healthCtrl))
        {
            GameLogger.LogError("PlayerCollisions: missing PlayerHealthController.", LoggerConfig.LogCategoryFlags.Player, this);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
      Debug.Log("OnTriggerEnter2D");
      ProcessCollisionOverTime(collision.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerExit2D");
        StopCollisionOverTime(collision.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      //Debug.Log("OnCollisionEnter2D");
      ProcessCollision(collision.gameObject);
    }

    #region Helpers
    void ProcessCollision(GameObject collider){
        
        //switch for different tags
        if (collider.CompareTag(EditorObjectTag.Water))
        {
            //PlayerCollisions: TakeDamage() daje info do PlayerHealthController (komenda: "zrób coś")
            _healthCtrl.TakeDamage(damageVal: 2);
        }
    }

    void ProcessCollisionOverTime(GameObject collider){ 
        //switch for different tags
        if (collider.CompareTag(EditorObjectTag.Water))
        {
            _healthCtrl.TakeDamageForSeconds(seconds: 1, damageVal: 1);
        }
    }

    void StopCollisionOverTime(GameObject collider){
        
        //switch for different tags
        if (collider.CompareTag(EditorObjectTag.Water))
        {
            _healthCtrl.StopActiveDamageCoroutine();
        }
    }
    #endregion
}
