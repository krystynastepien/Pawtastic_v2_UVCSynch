using UnityEngine;

public class PlayerStats : CreatureStats
{
    public void OnStart()
    {
        Init(maxHealth: 10, attack: 5, defense: 2, speed: 5);
    }

    /// <summary>
    /// execute on Fixedupdate in GameFlow
    /// </summary>
    public void OnUpdate()
    {
    }

    public override void TakeDamage(int damageVal){
        CurrentHealth = CurrentHealth - damageVal;
        if(CurrentHealth <= 0){ // character dies
            gameObject.SetActive(false);
        }
        Debug.Log("PLAYER DIED");
    }
}
