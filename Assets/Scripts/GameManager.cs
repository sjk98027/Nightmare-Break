using UnityEngine;

public class GameManager : MonoBehaviour
{
    DungeonManager dungeonManager;
    NetworkManager networkManager;
    InputManager inputManager;
    UIManager uiManager;

    [SerializeField]
    string ip;

    void Start()
    {
        InitializeManager();
    }

    void Update()
    {
        networkManager.DataHandler.DataHandle();
        networkManager.DataSender.DataSend();
    }

    void InitializeManager()
    {
        networkManager = (Instantiate(Resources.Load("Manager/NetworkManager")) as GameObject).GetComponent<NetworkManager>();
        networkManager.name = "NetworkManager";
        networkManager.tag = "NetworkManager";

        uiManager = (Instantiate(Resources.Load("Manager/UIManager")) as GameObject).GetComponent<UIManager>();
        uiManager.name = "UIManager";
        uiManager.tag = "UIManager";
        uiManager.SetDialog();

        networkManager.InitializeManager(ip);
        uiManager.SetLoginUIManager();
        uiManager.SetWaitUIManager();
    }

    public void OnApplicationQuit()
    {
        Debug.Log("게임 종료");
        networkManager.DataSender.GameClose();
    }
}

