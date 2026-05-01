using Fusion;
using UnityEngine;

public class HandSlot : NetworkBehaviour
{
    public SpriteRenderer handSprite;
    public bool rightHand;

    GameObject currentWeapon;
    IAttack cachedAttack;
    IItemData cachedItemData;
    private ShieldWeapon _cachedShield;
    
    Animator _anim;

    [Networked] public NetworkBool IsHandVisible { get; set; } = true;
    [Networked] public NetworkObject CurrentWeaponNO { get; set; }

    public override void Spawned()
    {
        UpdateHandVisuals();
    }
    public override void Render()
    {
        UpdateHandVisuals();
        if (CurrentWeaponNO != null && currentWeapon == null)
        {            
            if (Runner.TryFindObject(CurrentWeaponNO.Id, out var weaponNO))
            {
                var dataComponent = weaponNO.GetComponent<IItemData>();
                if (dataComponent != null)
                {
                    
                    SetupWeaponLocally(weaponNO.gameObject, dataComponent.GetData());
                }
                
            }
        }
    }

    public void UpdateHandVisuals()
    {
        if (handSprite != null) handSprite.enabled = IsHandVisible;
    }
    public void SetHandVisible(bool visible)
    {
        IsHandVisible = visible;
    }
    public void Pickup(WeaponsData data)
    {
        if (!IsEmpty()) { Drop(); }
        SetHandVisible(false);        

        var weaponNO = Runner.Spawn(data.prefab, transform.position, Quaternion.identity, Object.InputAuthority);
        CurrentWeaponNO = weaponNO;

        var itemData = weaponNO.GetComponent<IItemData>();
        itemData.SetData(data);

        SetupWeaponLocally(weaponNO.gameObject, data);
    }
    private void SetupWeaponLocally(GameObject weaponObj, WeaponsData data)
    {
        currentWeapon = weaponObj;

        
        currentWeapon.transform.SetParent(this.transform);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        
        cachedAttack = currentWeapon.GetComponent<IAttack>();
        cachedItemData = currentWeapon.GetComponent<IItemData>();
        _cachedShield = currentWeapon.GetComponent<ShieldWeapon>();

        
        cachedItemData.SetData(data);

        var anim = currentWeapon.GetComponent<Animator>();
        if (anim != null)
        {
            anim.runtimeAnimatorController = rightHand ? data.rightOverride : data.leftOverride;
        }
    }

    public void Drop()
    {
        if(IsEmpty()) return;
        SetHandVisible(true);
        WeaponsData dataToDrop = cachedItemData.GetData();
        EventManager.TriggerEvent(EventType.OnWeaponDropped, dataToDrop, transform.position, (Vector2)transform.right);
        //Destroy(currentWeapon);
        Runner.Despawn(CurrentWeaponNO);
        _cachedShield = null;
        currentWeapon = null;
        cachedAttack = null;
        cachedItemData = null;
        _anim = null;
    }
    
    public void ProcessInput(int mouseButton)
    {
        if (!HasInputAuthority) return;
        if (CurrentWeaponNO != null && cachedAttack == null)
        {
            RefreshCache();
        }
        if (cachedAttack == null) return;
        if (Input.GetMouseButtonDown(mouseButton)) ActionDown();
        if (Input.GetMouseButton(mouseButton)) ActionHold();
        if (Input.GetMouseButtonUp(mouseButton)) ActionUp();
    }
    private void RefreshCache()
    {
        if (Runner.TryFindObject(CurrentWeaponNO.Id, out var weaponNO))
        {
            currentWeapon = weaponNO.gameObject;
            cachedAttack = currentWeapon.GetComponent<IAttack>();
            cachedItemData = currentWeapon.GetComponent<IItemData>();
            _cachedShield = currentWeapon.GetComponent<ShieldWeapon>();
        }
    }
    public bool IsBlocking()
    {
        return _cachedShield != null && _cachedShield.IsBlocking;
    }

    public void ActionDown()=> cachedAttack?.Attack();
    public void ActionHold() => cachedAttack?.ActionHold();
    public void ActionUp() => cachedAttack?.ActionUp();

    public WeaponsData GetCurrentData() => cachedItemData?.GetData();
    public bool IsEmpty()=> CurrentWeaponNO == null;
    

}
