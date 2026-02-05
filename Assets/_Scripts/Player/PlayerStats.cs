using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : CreatureStats
{
    [SerializeField] private const int MAX_LIVES = 9;

    /// <summary>
    /// player has 9 lives total - within each x health bars (current life)
    /// </summary>
    [SerializeField] private int _playerMaxLives;
    [SerializeField] private int _playerCurrentLives;


    /// <summary>
    /// character dies when lives go down to 0
    /// </summary>
    public int MaxLives { get => _playerMaxLives; set => _playerMaxLives = value; }
    public int CurrentLives { get => _playerCurrentLives; set => _playerCurrentLives = value; }

    /// <summary>
    /// only one coroutine damage
    /// </summary>
    private Coroutine _damageCoroutine;


    public void OnStart()
    {
        BaseInit(maxHealth: 10, attack: 5, defense: 2, speed: 5);
        MaxLives = MAX_LIVES;
        CurrentLives = MAX_LIVES;
    }

    /// <summary>
    /// execute on Fixedupdate in GameFlow
    /// </summary>
    public void OnUpdate()
    {
    }

    public override void TakeDamage(int damageVal)
    {
        CurrentHealth = CurrentHealth - damageVal;

        if(CurrentHealth <= 0){ 
            if(CurrentLives == 1)
            {
                gameObject.SetActive(false);
                Debug.Log("PLAYER DIED");
            }
            else
            {
                CurrentLives = CurrentLives - 1;
                CurrentHealth = MaxHealth;
            }
        }
    }

    public override void TakeDamageOverGivenTime(float time, int damageVal){
        if(_damageCoroutine == null)
            _damageCoroutine = StartCoroutine(DamageOverGivenTimeCoroutine(time, damageVal));
        else{
            StopActiveDamageCoroutine();
            _damageCoroutine = StartCoroutine(DamageOverGivenTimeCoroutine(time, damageVal));
        }    
    }


    /// <summary>
    /// Substracts given damage from object's current health over time
    /// </summary>
    /// <param name="damageVal"></param>
    public override void TakeDamageForSeconds(float seconds, int damageVal){
        Debug.Log("_damageCoroutine: " + _damageCoroutine);
        if(_damageCoroutine == null)
            _damageCoroutine = StartCoroutine(DamageForSecondsCoroutine(seconds, damageVal));
        else{
            StopActiveDamageCoroutine();
            _damageCoroutine = StartCoroutine(DamageForSecondsCoroutine(seconds, damageVal));
        }
    }


    /// <summary>
    /// Substracts given damage from object's current health over time
    /// </summary>
    /// <param name="damageVal"></param>
    public override IEnumerator DamageOverGivenTimeCoroutine(float duration, int damageValMax) // damageValMax over duration 10sec = damage per loop should be 2
    {
        float amountDamaged = 0;
        int damagePerLoop = (int)Math.Round(damageValMax / duration);
        if(damagePerLoop == 0)
            damagePerLoop = 1;

        while (amountDamaged < damageValMax)
        {
            TakeDamage(damagePerLoop);
            amountDamaged = amountDamaged + damagePerLoop;
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Substracts given damage from object's current health every x seconds
    /// </summary>
    /// <param name="damageVal"></param>
    public override IEnumerator DamageForSecondsCoroutine(float seconds, int damageVal) // damageValMax over duration 10sec = damage per loop should be 2
    {
        while(CurrentHealth > 0){
            Debug.Log("inside DamageForSecondsCoroutine, seconds: " + seconds + ", damage: " + damageVal);
            //TODO force int instead of float?
            seconds = (int)Math.Round(seconds);
            TakeDamage(damageVal);
            yield return new WaitForSeconds(seconds);
        }
    }

    public override void StopActiveDamageCoroutine(){
        if(_damageCoroutine != null)
            StopCoroutine(_damageCoroutine);
    }

}
