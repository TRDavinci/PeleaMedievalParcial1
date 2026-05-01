using UnityEngine;
using Fusion;

public class ShieldWeapon : NetworkBehaviour, IAttack, IItemData
{
    //Animator _anim;
    NetworkMecanimAnimator _anim;
    public WeaponsData data;
    [Networked] public bool IsBlocking { get; set; }
    [Networked] public NetworkObject ParentObject { get; set; }
    public override void Spawned()
    {
        _anim = GetComponent<NetworkMecanimAnimator>();
    }

    public void Attack() 
    {
        _anim.SetTrigger("OnAttack");
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
    public float GetDamage()=> data.damage;   
    public WeaponsData GetData()=> data;
    public void SetData(WeaponsData newData) => data = newData;

}
