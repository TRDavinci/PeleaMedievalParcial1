using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;
using UnityEngine.UIElements;

public class Arrow : NetworkBehaviour
{
    [Networked] public float Damage { get; set; }
    [Networked] public Vector2 Direction { get; set; }
    [Networked] public float Force { get; set; }
    [Networked] public NetworkObject Owner { get; set; }
    
    public float lifeTime = 3f;    
    private NetworkRigidbody2D _rb;
    private TickTimer _lifeTimer;

    public override void Spawned()
    {
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeTime);
        _rb = GetComponent<NetworkRigidbody2D>();


        if (_rb != null)
        {
            _rb.Rigidbody.linearVelocity = Direction * Force;
        }
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

        if (Owner != null && (collision.gameObject == Owner.gameObject || collision.transform.IsChildOf(Owner.transform))) return;
            Debug.Log("Flecha impactó en: " + collision.name);
        
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out Health target))
            {               
                target.RPC_TakeDamage(Damage, transform.position);
            }            
            Runner.Despawn(Object);
        }        
        else if (collision.CompareTag("Border") || collision.CompareTag("Obstacle"))
        {
            Runner.Despawn(Object);
        }
    }
}



