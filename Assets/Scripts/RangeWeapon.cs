using UnityEngine;
using Fusion;

public class RangeWeapon : MonoBehaviour, IAttack, IItemData
{
    public WeaponsData data;
    public GameObject prefabArrow;
    //Animator _anim;
    NetworkMecanimAnimator _anim;
    public Transform shootPoint;


    [Header("Settings")]
    public float minForce = 10f;
    public float maxForce = 30f;
    public float maxChargeTime = 1.5f;

    private float chargeTimer;
    private bool isCharging;
    private void Awake()
    {
        _anim = GetComponent<NetworkMecanimAnimator>();
    }
    public void Attack()
    {
        isCharging = true;
        chargeTimer = 0;
        _anim.Animator.ResetTrigger("OnRelease");
        _anim.Animator.SetTrigger("OnDraw");
    }
    public void LaunchProjectile()
    {
        if (prefabArrow != null && shootPoint != null)
        {
            if (prefabArrow == null || shootPoint == null) return;
            
            float chargePercent = Mathf.Clamp01(chargeTimer / maxChargeTime);            
            float finalForce = Mathf.Lerp(minForce, maxForce, chargePercent);
            float finalDamage = Mathf.Lerp(data.damage * 0.5f, data.damage, chargePercent);

            
            Vector2 fireDirection = transform.right;
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            
            GameObject arrow = Instantiate(prefabArrow, shootPoint.position, Quaternion.Euler(0, 0, angle));

            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.damage = finalDamage;                
                arrowScript.owner = transform.root.gameObject;
            }

            Rigidbody2D rbArrow = arrow.GetComponent<Rigidbody2D>();
            if (rbArrow != null)
            {
                rbArrow.linearVelocity = fireDirection * finalForce;
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
        _anim.Animator.SetTrigger("OnRelease");

    }

}
