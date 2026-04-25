using Fusion;
using UnityEngine;

public class HandSlot : MonoBehaviour
{
    public SpriteRenderer handSprite;
    GameObject currentWeapon;
    IAttack cachedAttack;
    IItemData cachedItemData;
    private ShieldWeapon _cachedShield;
    public bool rightHand;
    Animator _anim;

    public void Pickup(WeaponsData data)
    {
        if (!IsEmpty()) { Drop(); }
        if (handSprite != null) handSprite.enabled = false;
        currentWeapon = Instantiate(data.prefab, transform);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        _anim = currentWeapon.GetComponent<Animator>();       
        cachedAttack = currentWeapon.GetComponent<IAttack>();
        cachedItemData = currentWeapon.GetComponent<IItemData>();
        _cachedShield = currentWeapon.GetComponent<ShieldWeapon>();
        if (_anim != null)
        {
            _anim.runtimeAnimatorController = rightHand ? data.rightOverride : data.leftOverride;
        }        
        cachedItemData.SetData(data);
    }

    public void Drop()
    {
        if(IsEmpty()) return;
        SetHandVisible(true);
        WeaponsData dataToDrop = cachedItemData.GetData();
        EventManager.TriggerEvent(EventType.OnWeaponDropped, dataToDrop, transform.position, (Vector2)transform.right);
        Destroy(currentWeapon);
        _cachedShield = null;
        currentWeapon = null;
        cachedAttack = null;
        cachedItemData = null;
        _anim = null;
    }
    public void SetHandVisible(bool visible)
    {
        if (handSprite != null) handSprite.enabled = visible;
    }
    public void ProcessInput(int mouseButton)
    {
        if (Input.GetMouseButtonDown(mouseButton)) ActionDown();
        if (Input.GetMouseButton(mouseButton)) ActionHold();
        if (Input.GetMouseButtonUp(mouseButton)) ActionUp();
    }
    public bool IsBlocking()
    {
        return _cachedShield != null && _cachedShield.isBlocking;
    }

    public void ActionDown()=> cachedAttack?.Attack();
    public void ActionHold() => cachedAttack?.ActionHold();
    public void ActionUp() => cachedAttack?.ActionUp();

    public WeaponsData GetCurrentData() => cachedItemData?.GetData();
    public bool IsEmpty()=> currentWeapon == null;
    

}
