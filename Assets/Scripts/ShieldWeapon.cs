using UnityEngine;
using Fusion;

public class ShieldWeapon : NetworkBehaviour, IAttack, IItemData
{
    //Animator _anim;
    NetworkMecanimAnimator _anim;
    public WeaponsData data;
    [Networked] public bool IsBlocking { get; set; }
    private void Awake()
    {
        _anim = GetComponent<NetworkMecanimAnimator>();
    }

    public void Attack() 
    {
        _anim.Animator.SetTrigger("OnAttack");
    }
    public void ActionHold() 
    {
        IsBlocking = true;
        _anim.Animator.SetBool("IsHolding", true);
    }
    public void ActionUp() 
    {
        IsBlocking = false;
        _anim.Animator.SetBool("IsHolding", false);
    }
    public float GetDamage()=> data.damage;   
    public WeaponsData GetData()=> data;
    public void SetData(WeaponsData newData) => data = newData;

}
