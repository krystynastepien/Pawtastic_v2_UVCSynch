using UnityEngine;

public interface IDamageable
{
     /// <summary>
    /// Substracts given damage from object's current health
    /// </summary>
    /// <param name="damageVal"></param>
    public void TakeDamage(int damageVal){} 
}
