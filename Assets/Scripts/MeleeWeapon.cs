using UnityEngine;
using Fusion;

public class MeleeWeapon : NetworkBehaviour, IAttack, IItemData
{
    NetworkMecanimAnimator _anim;
    //Animator _anim;
    public WeaponsData data;
    public LayerMask enemyLayers;
    public Transform attackPoint;

    [Networked] public NetworkObject ParentObject { get; set; }
    public override void Spawned()
    {
     _anim=GetComponent<NetworkMecanimAnimator>();       
    }
    public void Attack()
    {
        _anim.SetTrigger("OnAttack");
        Debug.Log("Atacando");
    }
    public void HitTarget()
    {
        if (!HasStateAuthority) return;
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


    public override void Render()
    {
        if (transform.parent != null) return;
        
        PlayerRef owner = Object.InputAuthority != PlayerRef.None ? Object.InputAuthority : Object.StateAuthority;

        if (owner != PlayerRef.None)
        {
            NetworkObject playerNO = Runner.GetPlayerObject(owner);
            if (playerNO != null)
            {
                HandSlot[] slots = playerNO.GetComponentsInChildren<HandSlot>();
                foreach (var slot in slots)
                {
                    if (slot.CurrentWeaponNO == Object)
                    {
                        
                        transform.SetParent(slot.transform);
                        transform.localPosition = Vector3.zero;
                        transform.localRotation = Quaternion.identity;

                        
                        var wData = GetData();
                        var anim = GetComponent<Animator>();
                        if (anim != null && wData != null)
                        {
                            anim.runtimeAnimatorController = slot.rightHand ? wData.rightOverride : wData.leftOverride;
                        }
                        break;
                    }
                }
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
