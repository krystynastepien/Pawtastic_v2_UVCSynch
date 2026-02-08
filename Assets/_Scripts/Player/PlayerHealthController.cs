using System;
using System.Collections;
using UnityEngine;

public class PlayerHealthController : CreatureStats
{
    #region Serialized Fields
    [SerializeField] private int _playerMaxLives;
    #endregion

    #region  Fields
    private int _playerCurrentLives;
    #endregion

    #region Properties
    /// <summary>
    /// Maximum number of lives the player can have
    /// </summary>
    public int MaxLives { get => _playerMaxLives; set => _playerMaxLives = value; }

    /// <summary>
    /// Current number of lives remaining. Character dies when it reaches 0.
    /// </summary>
    public int CurrentLives { get => _playerCurrentLives; set => _playerCurrentLives = value; }
    #endregion

    #region Fields
    /// <summary>
    /// Tracks active damage coroutine to prevent multiple simultaneous damage effects
    /// </summary>
    private Coroutine _damageCoroutine;
    #endregion

    #region Initialization
    public void OnStart()
    {
        BaseInit(
            PlayerStats.INIT_MAX_HEALTH,
            PlayerStats.BASE_ATTACK,
            PlayerStats.BASE_DEFENSE,
            PlayerStats.BASE_SPEED
        );
        MaxLives = PlayerStats.INIT_MAX_LIVES;
        CurrentLives = PlayerStats.INIT_MAX_LIVES;
    }
    #endregion

    #region Damage Methods
    public override void TakeDamage(int damageVal)
    {
        CurrentHealth -= damageVal;

        if (CurrentHealth <= 0)
        {
            if (CurrentLives == 1)
            {
                gameObject.SetActive(false);
                GameLogger.Log("PLAYER died", LoggerConfig.LogCategoryFlags.Player, this);
            }
            else
            {
                CurrentLives--; // CurrentLives = CurrentLives - 1
                CurrentHealth = MaxHealth;
            }
        }
    }

    public override void TakeDamageOverGivenTime(float time, int damageVal)
    {
        if (_damageCoroutine == null)
            _damageCoroutine = StartCoroutine(DamageOverGivenTimeCoroutine(time, damageVal));
        else
        {
            StopActiveDamageCoroutine();
            _damageCoroutine = StartCoroutine(DamageOverGivenTimeCoroutine(time, damageVal));
        }
    }

    /// <summary>
    /// Subtracts given damage from object's current health over time
    /// </summary>
    public override void TakeDamageForSeconds(float seconds, int damageVal)
    {
        Debug.Log("_damageCoroutine: " + _damageCoroutine);
        if (_damageCoroutine == null)
            _damageCoroutine = StartCoroutine(DamageForSecondsCoroutine(seconds, damageVal));
        else
        {
            StopActiveDamageCoroutine();
            _damageCoroutine = StartCoroutine(DamageForSecondsCoroutine(seconds, damageVal));
        }
    }

    public override void StopActiveDamageCoroutine()
    {
        if (_damageCoroutine != null)
            StopCoroutine(_damageCoroutine);
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Applies damage over a given duration (total damageValMax spread across duration)
    /// </summary>
    public override IEnumerator DamageOverGivenTimeCoroutine(float duration, int damageValMax)
    {
        float amountDamaged = 0;
        int damagePerLoop = (int)Math.Round(damageValMax / duration);
        if (damagePerLoop == 0)
            damagePerLoop = 1;

        while (amountDamaged < damageValMax)
        {
            TakeDamage(damagePerLoop);
            amountDamaged += damagePerLoop;
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Applies damage repeatedly every X seconds until health reaches 0
    /// </summary>
    public override IEnumerator DamageForSecondsCoroutine(float seconds, int damageVal)
    {
        while (CurrentHealth > 0)
        {
            Debug.Log("inside DamageForSecondsCoroutine, seconds: " + seconds + ", damage: " + damageVal);
            seconds = (int)Math.Round(seconds);
            TakeDamage(damageVal);
            yield return new WaitForSeconds(seconds);
        }
    }
    #endregion
}