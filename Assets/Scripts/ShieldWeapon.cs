using UnityEngine;

public class ShieldWeapon : MonoBehaviour, IAttack, IItemData
{
    Animator _anim;
    public WeaponsData data;
    public bool isBlocking;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Attack() 
    {
        _anim.SetTrigger("OnAttack");
    }
    public void ActionHold() 
    {
        isBlocking = true;
        _anim.SetBool("IsHolding", true);
    }
    public void ActionUp() 
    {
        isBlocking = false;
        _anim.SetBool("IsHolding", false);
    }
    public float GetDamage()=> data.damage;   
    public WeaponsData GetData()=> data;
    public void SetData(WeaponsData newData) => data = newData;

}
