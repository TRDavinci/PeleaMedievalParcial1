using UnityEngine;
using Fusion;

public class MeleeWeapon : MonoBehaviour, IAttack, IItemData
{
    NetworkMecanimAnimator _anim;
    //Animator _anim;
    public WeaponsData data;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    private void Awake()
    {
     _anim=GetComponent<NetworkMecanimAnimator>();       
    }
    public void Attack()
    {
        _anim.Animator.SetTrigger("OnAttack");
    }
    public void HitTarget()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, data.hitRadius, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject == transform.root.gameObject) continue;

            if (enemy.TryGetComponent(out Health h))
            {
                h.RPC_TakeDamage(data.damage, transform.position);
            }
        }
    }
    public float GetDamage() => data.damage;

    public void SetData(WeaponsData newData) => data = newData;
    public WeaponsData GetData() => data;
    public void ActionHold() { _anim.Animator.SetBool("IsHolding", true); }      
    public void ActionUp() { _anim.Animator.SetBool("IsHolding", false); }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(attackPoint.position, data.hitRadius);
    }

}
