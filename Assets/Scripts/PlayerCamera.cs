using UnityEngine;
using Fusion;

public class PlayerCamera : NetworkBehaviour
{
    public static Camera LocalCamera;

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            LocalCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (!HasInputAuthority || LocalCamera == null) return;

        LocalCamera.transform.position = transform.position + new Vector3(0, 0, -10);
    }
}