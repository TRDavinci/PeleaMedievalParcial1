using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner runnerPrefab;
    private NetworkRunner runner;
    public PlayerSpawner playerSpawner;

    async void Start()
    {
        runner = Instantiate(runnerPrefab);

        runner.ProvideInput = true;


        if (playerSpawner != null)
        {
            runner.AddCallbacks(playerSpawner);
        }
        else
        {
            Debug.LogError("¡No arrastraste el PlayerSpawner al Launcher en el Inspector!");
        }        
        runner.AddCallbacks(this);

        var sceneManager = runner.GetComponent<NetworkSceneManagerDefault>();
        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "MiSala",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            //SceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
            SceneManager = sceneManager
        });
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData data = new NetworkInputData();

        data.move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        
        var myPlayer = runner.GetPlayerObject(runner.LocalPlayer);
        if (myPlayer != null)
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (Vector2)mouse - (Vector2)myPlayer.transform.position;
            data.rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        data.dashPressed = Input.GetKey(KeyCode.Space);
        input.Set(data);
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
       
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
       
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
       
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
       
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
       
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
       
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
       
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
       
    }
}