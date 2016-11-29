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
        try
        {
            uiManager.SetLoginUIManager();
            uiManager.SetWaitUIManager();
        }
        catch
        {

        }
        uiManager.SetBattleUIManager();
    }

    public void SetManagerInDungeon()
    {
        dungeonManager = (Instantiate(Resources.Load("Manager/DungeonManager")) as GameObject).GetComponent<DungeonManager>();
        dungeonManager.name = "DungeonManager";
        dungeonManager.tag = "DungeonManager";

        inputManager = (Instantiate(Resources.Load("Manager/DungeonManager")) as GameObject).GetComponent<InputManager>();
        inputManager.name = "InputManager";
        inputManager.tag = "InputManager";

    }

    public void OnApplicationQuit()
    {
        Debug.Log("게임 종료");
        networkManager.DataSender.GameClose();
    }
}

