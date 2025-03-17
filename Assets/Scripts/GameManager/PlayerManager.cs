using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance {get; private set;}  // singleton - globally accesible class with only 1 instance



    // create instance on Awake
    private void Awake(){

        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

}
