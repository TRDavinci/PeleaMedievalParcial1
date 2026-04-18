using UnityEngine;

public class RangeWeapon : MonoBehaviour, IAttack, IItemData
{
    public WeaponsData data;
    public GameObject prefabArrow;
    Animator _anim;
    public Transform shootPoint;


    [Header("Settings")]
    public float minForce = 10f;
    public float maxForce = 30f;
    public float maxChargeTime = 1.5f;

    private float chargeTimer;
    private bool isCharging;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    public void Attack()
    {
        isCharging = true;
        chargeTimer = 0;
        _anim.ResetTrigger("OnRelease");
        _anim.SetTrigger("OnDraw");
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
            _anim.SetBool("IsHolding", true);
        }
    }
    public void ActionUp() 
    {
        isCharging = false;
        _anim.SetBool("IsHolding", false);
        _anim.ResetTrigger("OnDraw");
        _anim.SetTrigger("OnRelease");

    }

}
