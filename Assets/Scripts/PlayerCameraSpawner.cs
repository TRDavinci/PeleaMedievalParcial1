using UnityEngine;
using Fusion;

public class PlayerCameraSpawner : NetworkBehaviour
{
    public GameObject cameraPrefab;

    private Camera myCamera;

    public override void Spawned()
    {
        
        if (!HasInputAuthority) return;

        GameObject cam = Instantiate(cameraPrefab);

        myCamera = cam.GetComponent<Camera>();

        
        cam.transform.position = transform.position + new Vector3(0, 0, -10);

        
        cam.transform.SetParent(transform);
    }
}