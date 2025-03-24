using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

public abstract class CreatureStats : MonoBehaviour, IDamageable
{
    #region Fields - stats
 
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private float speed;

    #endregion

    #region Fake Ctor
    public void Init(int maxHealth, int attack, int defense, int speed)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
    }

    #endregion

    #region Properties

    /// <summary>
    /// returns character's max health value
    /// </summary>
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }


    /// <summary>
    /// returns character's current health value
    /// </summary>
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    
    /// <summary>
    /// returns character's attack value
    /// </summary>
    public int Attack { get => attack; set => attack = value; }

    /// <summary>
    /// returns character's defense value
    /// </summary>
    public int Defense { get => defense; set => defense = value; }
    /// <summary>
    /// returns character's speed value
    /// </summary>
    public float Speed { get => speed; set => speed = value; }

    #endregion

    #region Modifiers

    public abstract void TakeDamage(int damageVal);
   
    /// <summary>
    /// returns characters health after taking damage
    /// </summary>
    /// <param name="damage"></param>
    public void Heal(int healVal){
        maxHealth = maxHealth + healVal;
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
    
    #endregion
}
