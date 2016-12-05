using UnityEngine;

public class GameManager : MonoBehaviour
{
    DungeonManager dungeonManager;
    NetworkManager networkManager;
    InputManager inputManager;
    UIManager uiManager;
    CharacterStatus characterStatus;

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
        name = "GameManager";
        tag = "GameManager";

        networkManager = (Instantiate(Resources.Load("Manager/NetworkManager")) as GameObject).GetComponent<NetworkManager>();
        networkManager.name = "NetworkManager";
        networkManager.tag = "NetworkManager";

        uiManager = (Instantiate(Resources.Load("Manager/UIManager")) as GameObject).GetComponent<UIManager>();
        uiManager.name = "UIManager";
        uiManager.tag = "UIManager";
        uiManager.SetDialog();

        networkManager.InitializeManager(ip);

        SetManagerInWait();

        try
        {
            uiManager.SetLoginUIManager();
            uiManager.SetWaitUIManager();
        }
        catch
        {

        }
    }

    public void SetManagerInWait()
    {
        characterStatus = (Instantiate(Resources.Load("Manager/CharacterStatus")) as GameObject).GetComponent<CharacterStatus>();
        characterStatus.name = "CharacterStatus";
        characterStatus.tag = "CharStatus";
    }

    public void SetManagerInDungeon()
    {
        dungeonManager = (Instantiate(Resources.Load("Manager/DungeonManager")) as GameObject).GetComponent<DungeonManager>();
        dungeonManager.name = "DungeonManager";
        dungeonManager.tag = "DungeonManager";

        inputManager = (Instantiate(Resources.Load("Manager/InputManager")) as GameObject).GetComponent<InputManager>();
        inputManager.name = "InputManager";
        inputManager.tag = "InputManager";

        dungeonManager.Initialize();
        //uiManager.SetBattleUIManager();
    }

    public void OnApplicationQuit()
    {
        Debug.Log("게임 종료");
        networkManager.DataSender.GameClose();
    }
}

