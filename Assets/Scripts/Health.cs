using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float _currentHealth;

    public Func<float,Vector2, float> OnProcessDamage;
    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float dmg, Vector2 attackerPos)
    {
        if (_currentHealth <= 0) return;

        if (OnProcessDamage != null)
        {
            dmg = OnProcessDamage(dmg, attackerPos);
        }
        _currentHealth -= dmg;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        //if(HasStateAuthority) Runner.Despawn(Object);
        Debug.Log(gameObject.name + " ha muerto.");

        
        Destroy(gameObject);
    }
}
