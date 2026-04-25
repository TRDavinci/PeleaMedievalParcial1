using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class Arrow : NetworkBehaviour
{
    public float damage;
    public float lifeTime = 3f;
    public GameObject owner;


    private TickTimer _lifeTimer;

    public override void Spawned()
    {        
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeTime);
    }

    public override void FixedUpdateNetwork()
    {
      if(_lifeTimer.Expired(Runner))
      {
            Runner.Despawn(Object);
      }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!HasStateAuthority) return;
        
        if (owner != null && (collision.gameObject == owner || collision.transform.IsChildOf(owner.transform))) return;
        Debug.Log("Flecha impactó en: " + collision.name);
        
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out Health target))
            {               
                target.RPC_TakeDamage(damage, transform.position);
            }            
            Runner.Despawn(Object);
        }        
        else if (collision.CompareTag("Border") || collision.CompareTag("Obstacle"))
        {
            Runner.Despawn(Object);
        }
    }
}



