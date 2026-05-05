using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector2 move;
    public float rotation;
    public NetworkBool dashPressed;
}