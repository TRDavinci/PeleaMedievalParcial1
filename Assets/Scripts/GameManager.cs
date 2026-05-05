using Fusion;
using UnityEngine;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Networked] public int PlayerCount { get; set; }
    [Networked] public bool GameStarted { get; set; }
    [Networked] public bool GameEnded { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void FixedUpdateNetwork()
    {
        
        if (!HasStateAuthority) return;

       

        PlayerCount = Runner.ActivePlayers.Count();

        // Esperar 2 jugadores
        if (PlayerCount >= 2 && !GameStarted)
        {
            GameStarted = true;
            Debug.Log("GAME START");
        }

        if (GameStarted && !GameEnded)
        {
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
        var players = FindObjectsByType<Health>(FindObjectsSortMode.None);

        var alive = players.Where(p => p.CurrentHealth > 0).ToList();

        if (alive.Count == 1)
        {
            GameEnded = true;

            NetworkObject winner = alive[0].Object;

            RPC_AnnounceWinner(winner.InputAuthority);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_AnnounceWinner(PlayerRef winner)
    {
        if (Runner.LocalPlayer == winner)
        {
            UIManager.Instance.ShowVictory();
        }
        else
        {
            UIManager.Instance.ShowDefeat();
        }
    }
}