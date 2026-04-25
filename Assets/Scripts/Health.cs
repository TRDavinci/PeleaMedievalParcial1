using Fusion;
using System;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public float maxHealth = 100f;
    [Networked] public float CurrentHealth { get; set; }

    public Func<float,Vector2, float> OnProcessDamage;

    public override void Spawned()
    {
        if (!HasStateAuthority) return;
        CurrentHealth = maxHealth;
    }
   

    public void Local_TakeDamage(float dmg, Vector2 attackerPos)
    {
        if (CurrentHealth <= 0) return;

        if (OnProcessDamage != null)
        {
            dmg = OnProcessDamage(dmg, attackerPos);
        }
        CurrentHealth -= dmg;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(float dmg, Vector2 attackerPos)=>Local_TakeDamage(dmg, attackerPos);


    private void Die()
    {            
        Debug.Log(gameObject.name + " ha muerto.");
        Runner.Despawn(Object);
    }
}
