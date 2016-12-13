using UnityEngine;
using System.Net;

public class GameManager : MonoBehaviour
{
    DungeonManager dungeonManager;
    NetworkManager networkManager;
    InputManager inputManager;
    UIManager uiManager;
    CharacterStatus characterStatus;
    
    string myIP;    
    public string MyIP
    {
        get
        {
            return myIP;
        }
    }

    void Start()
    {
        myIP = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
        InitializeManager();
    }

    void Update()
    {

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
        //uiManager.SetDialog();

        networkManager.InitializeManager();
        
        uiManager.SetLoginUIManager();

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(networkManager);
        DontDestroyOnLoad(uiManager);
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

        dungeonManager.Initialize(networkManager.UserIndex.Count);
        //uiManager.SetBattleUIManager();
    }

    public void OnApplicationQuit()
    {
        networkManager.DataSender.GameClose();
    }
}

