using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class ItemSpawner : NetworkBehaviour
{
    public GameObject itemWorldPrefab;
    public List<Transform> spawnPoints;

    public override void Spawned()
    {        
        if (HasStateAuthority)
        {
            foreach (Transform point in spawnPoints)
            {               
                Runner.Spawn(itemWorldPrefab, point.position, Quaternion.identity);
            }
        }
    }

    private void SpawnFromDrop(params object[] parameters)
    {       
        
        WeaponsData data =parameters[0] as WeaponsData;
        int id = WeaponDataBase.Instance.allWeapons.IndexOf(data);
        Vector3 pos = (Vector3)parameters[1];
        Vector2 dir = (Vector2)parameters[2];

        if (HasStateAuthority)
        {
            ExecuteSpawn(id, pos, dir);
        }
        else
        {
            RPC_RequestSpawnFromDrop(id, pos, dir);
        }



    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestSpawnFromDrop(int id, Vector3 pos, Vector2 dir)
    {
        ExecuteSpawn(id, pos, dir);
    }

    private void ExecuteSpawn(int id, Vector3 pos, Vector2 dir)
    {
        NetworkObject item = Runner.Spawn(itemWorldPrefab, pos, Quaternion.identity);
        item.GetComponent<ItemWorld>().RPC_SetupItem(id, dir);
    }



    private void OnEnable() 
    {
        EventManager.SubscribeToEvent(EventType.OnWeaponDropped, SpawnFromDrop);
    }

    private void OnDisable() 
    {
        EventManager.UnsubscribeToEvent(EventType.OnWeaponDropped, SpawnFromDrop);
    }
}
