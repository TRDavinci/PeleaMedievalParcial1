using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject _playerPrefab;
    public void PlayerJoined(PlayerRef player)
    {
        if(player== Runner.LocalPlayer)
        {
            Runner.Spawn(_playerPrefab, Vector3.zero,Quaternion.identity);
        }
    }
}
