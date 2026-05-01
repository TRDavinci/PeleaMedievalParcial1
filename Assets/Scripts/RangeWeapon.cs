using UnityEngine;
using Fusion;


public class RangeWeapon : NetworkBehaviour, IAttack, IItemData
{
    public WeaponsData data;
    public GameObject prefabArrow;
    //Animator _anim;
    NetworkMecanimAnimator _anim;
    public Transform shootPoint;
    [Networked] public NetworkObject ParentObject { get; set; }

    [Header("Settings")]
    public float minForce = 10f;
    public float maxForce = 30f;
    public float maxChargeTime = 1.5f;

    private float chargeTimer;
    private bool isCharging;
    public override void Spawned()
    {
        _anim = GetComponent<NetworkMecanimAnimator>();
    }
    public void Attack()
    {
        isCharging = true;
        chargeTimer = 0;
        _anim.Animator.ResetTrigger("OnRelease");
        _anim.SetTrigger("OnDraw");
    }
    public void LaunchProjectile()
    {

        if (prefabArrow == null || shootPoint == null) return;

        float chargePercent = Mathf.Clamp01(chargeTimer / maxChargeTime);
        float finalForce = Mathf.Lerp(minForce, maxForce, chargePercent);
        float finalDamage = Mathf.Lerp(data.damage * 0.5f, data.damage, chargePercent);

        Vector2 fireDirection = transform.right;

        Runner.Spawn(prefabArrow, shootPoint.position, transform.rotation, Object.InputAuthority, (runner, obj) =>
        {
            var arrow = obj.GetComponent<Arrow>();
            arrow.Damage = finalDamage;
            arrow.Direction = fireDirection;
            arrow.Force = finalForce;
            arrow.Owner = transform.root.GetComponent<NetworkObject>();
        });



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
    public void ActionHold() 
    {
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            _anim.Animator.SetBool("IsHolding", true);
        }
    }
    public void ActionUp() 
    {
        isCharging = false;
        _anim.Animator.SetBool("IsHolding", false);
        _anim.Animator.ResetTrigger("OnDraw");
        _anim.SetTrigger("OnRelease");
        Debug.Log("Atacando");

    }

}
