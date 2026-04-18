using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemWorldPrefab;

    private void Start()
    {
        //EventManager.SubscribeToEvent(EventType.OnWeaponDropped, SpawnFromDrop);   
    }

    private void SpawnFromDrop(params object[] parameters)
    {
        WeaponsData data =parameters[0] as WeaponsData;
        Vector3 pos = (Vector3)parameters[1];
        Vector2 dir = (Vector2)parameters[2];

        if (data != null)
        {
            GameObject item = TriggerSpawn(data, pos);
            item.GetComponent<ItemWorld>().SetUpDrop(dir);
        }
    }

    private GameObject TriggerSpawn(WeaponsData data, Vector3 pos)
    {
        GameObject item = Instantiate(itemWorldPrefab, pos, Quaternion.identity);
        item.GetComponent<ItemWorld>().SetWeapon(data);
        return item;
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
