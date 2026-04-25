using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class ItemWorld : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(LoadWeaponData))] //Chequea la variable y si cambia ejecuta el metodo
    public int weaponID { get; set; } = -1;

    public WeaponsData weaponData;
    SpriteRenderer spriteRenderer;

   

    public override void Spawned()
    {        
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (HasStateAuthority && weaponID == -1)
        {
            weaponID = Random.Range(0, WeaponDataBase.Instance.allWeapons.Count);
            
        }
        if (weaponID != -1)
        {
            LoadWeaponData();
        }
    }    

    private void LoadWeaponData()
    {
        if (weaponID != -1)
        {
            weaponData = WeaponDataBase.Instance.allWeapons[weaponID];
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = weaponData.sprite;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetupItem(int id, Vector2 dir)
    {
        weaponID = id;
        SetUpDrop(dir);
    }

    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestDespawn()
    {
        Runner.Despawn(Object);
    }

    public void Collect()
    {
        if (HasStateAuthority)
        {
            Runner.Despawn(Object);
        }
        else
        {
            RPC_RequestDespawn();
        }
    }

    public void SetUpDrop(Vector2 direction)
    {
        if (TryGetComponent(out NetworkRigidbody2D rb))
        {
            rb.Rigidbody.AddForce(direction * 1.5f, ForceMode2D.Impulse);
        }
    }

}
