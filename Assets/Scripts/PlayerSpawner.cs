using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform[] spawnPoints;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        if (player == runner.LocalPlayer)
        {
            if (_playerPrefab == null) return;



            int index = runner.ActivePlayers.Count() - 1;
            index = Mathf.Clamp(index, 0, spawnPoints.Length - 1);
            Vector3 spawnPos = spawnPoints[index].position;

            Debug.Log($"Spawneando mi jugador local: {player}");


            NetworkObject playerObj = runner.Spawn(
                _playerPrefab,
                spawnPos,
                Quaternion.identity,
                inputAuthority: player
            );

            runner.SetPlayerObject(player, playerObj);
        }
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, System.Collections.Generic.List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, System.Collections.Generic.Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}