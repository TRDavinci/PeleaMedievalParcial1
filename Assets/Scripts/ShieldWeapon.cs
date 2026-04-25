using UnityEngine;
using Fusion;

public class ShieldWeapon : MonoBehaviour, IAttack, IItemData
{
    //Animator _anim;
    NetworkMecanimAnimator _anim;
    public WeaponsData data;
    public bool isBlocking;
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
        isBlocking = true;
        _anim.Animator.SetBool("IsHolding", true);
    }
    public void ActionUp() 
    {
        isBlocking = false;
        _anim.Animator.SetBool("IsHolding", false);
    }
    public float GetDamage()=> data.damage;   
    public WeaponsData GetData()=> data;
    public void SetData(WeaponsData newData) => data = newData;

}
