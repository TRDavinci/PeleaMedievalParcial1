using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject waitingPanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        waitingPanel.SetActive(!GameManager.Instance.GameStarted);
    }

    public void ShowVictory()
    {
        victoryPanel.SetActive(true);
    }

    public void ShowDefeat()
    {
        defeatPanel.SetActive(true);
    }
}