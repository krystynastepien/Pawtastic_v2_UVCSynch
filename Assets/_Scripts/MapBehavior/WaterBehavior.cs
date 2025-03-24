using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
    void OnTriggerStay(Collider characterCollider)
    {
        if(characterCollider.gameObject.tag == GameManager.Instance.Tag_player){
            
        }

    }

}
