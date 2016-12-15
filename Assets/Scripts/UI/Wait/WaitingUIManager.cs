using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaitingUIManager : MonoBehaviour
{
    public const int maxCharacterNum = 3;
    public const int maxRoomNum = 20;
    public const int maxPlayerNum = 4;

    [SerializeField]private int selectNum;

    private Button roomCreateBtn;
    private Button skillAddBtn;
    private Button equipInfoBtn;
    private Button myInfoBtn;
    private Button roomCreateExitBtn;
    private Button skillAddExitBtn;
    private Button equipInfoExitBtn;
    private Button myInfoExitBtn;
    private Button roomCreateYesBtn;

    private InputField roomName;

	[SerializeField] GameObject roomCreateUI;
	private GameObject skillAddUI;
	private GameObject equipInfoUI;
	private GameObject myInfoUI;

    Room[] rooms;

    Text createRoomName;
    int currentRoomNum;
    int dungeonId;
    int dungeonLevel;

    public Room[] Rooms { get { return rooms; } }

    public void ManagerInitialize()
    {
        selectNum = -1;
        dungeonId = 0;
        dungeonLevel = 1;

        SetUIObject();
        InitializeAddListner();

        roomCreateUI.SetActive(false);
        skillAddUI.SetActive(false);
        equipInfoUI.SetActive(false);
        myInfoUI.SetActive(false);
    }

    public void SetUIObject()
    {
        roomCreateBtn = GameObject.Find("RoomCreateBtn").GetComponent<Button>();
        roomCreateYesBtn = GameObject.Find("RoomCreateYesBtn").GetComponent<Button>();
        skillAddBtn = GameObject.Find("SkillAddBtn").GetComponent<Button>();
        equipInfoBtn = GameObject.Find("EquipBtn").GetComponent<Button>();
        myInfoBtn = GameObject.Find("MyInfoBtn").GetComponent<Button>();
        roomCreateUI = GameObject.Find("RoomCreateUI");
        skillAddUI = GameObject.Find("SkillAddUI");
        equipInfoUI = GameObject.Find("EquipInfoUI");
        myInfoUI = GameObject.Find("MyInfoUI");
        roomName = GameObject.Find("RoomCreateInputField").GetComponent<InputField>();
        createRoomName = GameObject.Find("CreateRoomName").GetComponent<Text>();
        roomCreateExitBtn = roomCreateUI.transform.GetChild(5).GetComponent<Button>();
        skillAddExitBtn = skillAddUI.transform.GetChild(3).GetComponent<Button>();
        equipInfoExitBtn = equipInfoUI.transform.GetChild(2).GetComponent<Button>();
        myInfoExitBtn = myInfoUI.transform.GetChild(1).GetComponent<Button>();
    }

    public void InitializeAddListner()
    {
        roomCreateBtn.onClick.AddListener(() => RoomCreate());
        roomCreateYesBtn.onClick.AddListener(() => OnClickCreateRoomButton()); 
        skillAddBtn.onClick.AddListener(() => SkillAdd());
        equipInfoBtn.onClick.AddListener(() => EquipInfo());
        myInfoBtn.onClick.AddListener(() => MyInfo());
        roomCreateExitBtn.onClick.AddListener(() => BtnActiveCheck());
        skillAddExitBtn.onClick.AddListener(() => BtnActiveCheck());
        equipInfoExitBtn.onClick.AddListener(() => BtnActiveCheck());
        myInfoExitBtn.onClick.AddListener(() => BtnActiveCheck());
    }

    public void RoomCreate()
	{
		BtnActiveCheck ();

		roomCreateUI.SetActive (true);
	}

	public void SkillAdd()
	{
		BtnActiveCheck ();
		selectNum = 1;
		skillAddUI.SetActive (true);
	}

	public void EquipInfo()
	{
		BtnActiveCheck ();
		selectNum = 2;
		equipInfoUI.SetActive(true);
	}

	public void MyInfo()
	{
		BtnActiveCheck ();
		selectNum = 3;
		myInfoUI.SetActive (true);
	}

	public void BtnActiveCheck()
	{
        if(roomCreateUI.activeSelf)
        {
            roomName.text ="";
            roomCreateUI.SetActive(false);
        }
        else if (skillAddUI.activeSelf)
        {
            skillAddUI.SetActive(false);
        }
        else if (equipInfoUI.activeSelf)
        {
            equipInfoUI.SetActive(false);
        }
        else if (myInfoUI.activeSelf)
        {
            myInfoUI.SetActive(false);
        }
    }

    public void SetRoom(RoomListData roomListData)
    {
        rooms = roomListData.Rooms;
    }

    public void CreateRoom(int roomNum)
    {
        Debug.Log("방 생성 성공");
        currentRoomNum = roomNum;
        DataSender.Instance.EnterRoom(roomNum);
    }

    public void OnClickCreateRoomButton()
    {
        DataSender.Instance.CreateRoom(createRoomName.text, dungeonId, dungeonLevel);
    }

    public void OnClickEnterRoomButton()
    {
        DataSender.Instance.EnterRoom(currentRoomNum);
    }

    public void OnClickExitRoomButton()
    {
        DataSender.Instance.ExitRoom(currentRoomNum);
    }

    public void OnClickStartGameButton()
    {
        DataSender.Instance.StartGame();
        DataSender.Instance.RequestMonsterSpawnList();
        DataSender.Instance.RequestDungeonData();
    }
}

public class Room
{
    string roomName;
    int dungeonId;
    int dungeonLevel;
    RoomUserData[] roomUserData;

    public string RoomName { get { return roomName; } }
    public int DungeonId { get { return dungeonId; } }
    public int DungeonLevel { get { return dungeonLevel; } }
    public RoomUserData[] RoomUserData { get { return roomUserData; } }

    public Room()
    {
        roomName = "";
        dungeonId = 0;
        dungeonLevel = 0;
        roomUserData = new RoomUserData[WaitingUIManager.maxPlayerNum];

        for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
        {
            roomUserData[i] = new RoomUserData();
        }
    }

    public Room(string newName, int newDungeonId, int newDungeonLevel)
    {
        roomName = newName;
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
        roomUserData = new RoomUserData[WaitingUIManager.maxPlayerNum];

        for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
        {
            roomUserData[i] = new RoomUserData();
        }
    }

    public Room(string newName, byte newDungeonId, byte newDungeonLevel, RoomUserData[] newRoomUserData)
    {
        roomName = newName;
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
        roomUserData = newRoomUserData;
    }
}

public class RoomUserData
{
    string userName;
    int userGender;
    int userClass;
    int userLevel;

    public string UserName { get { return userName; } }
    public int UserGender { get { return userGender; } }
    public int UserClass { get { return userClass; } }
    public int UserLevel { get { return userLevel; } }

    public RoomUserData()
    {
        userName = "";
        userGender = 0;
        userClass = 0;
        userLevel = 0;
    }

    public RoomUserData(string newUserName, int newUserGender, int newUserClass, int newUserLevel)
    {
        userName = newUserName;
        userGender = newUserGender;
        userClass = newUserClass;
        userLevel = newUserLevel;
    }
}