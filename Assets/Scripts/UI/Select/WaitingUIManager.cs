using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaitingUIManager : MonoBehaviour
{
    public const int maxCharacterNum = 3;
    public const int maxRoomNum = 20;
    public const int maxPlayerNum = 4;

    private int selectNum;
    private Button roomCreateBtn;
    private Button skillAddBtn;
    private Button equipInfoBtn;
    private Button myInfoBtn;
    private Button roomCreateExitBtn;
    private Button skillAddExitBtn;
    private Button equipInfoExitBtn;
    private Button myInfoExitBtn;
	private GameObject roomCreateUI;
	private GameObject skillAddUI;
	private GameObject equipInfoUI;
	private GameObject myInfoUI;

    Room[] rooms;

    Text createRoomName;
    int dungeonId;
    int dungeonLevel;

    public Room[] Rooms { get { return rooms; } }

    //
    int currentRoomNum;
    int userNum;

    public int UserNum { get { return userNum; } }

    public void ManagerInitialize()
    {
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
        skillAddBtn = GameObject.Find("SkillAddBtn").GetComponent<Button>();
        equipInfoBtn = GameObject.Find("EquipBtn").GetComponent<Button>();
        myInfoBtn = GameObject.Find("MyInfoBtn").GetComponent<Button>();
        roomCreateUI = GameObject.Find("RoomCreateUI");
        skillAddUI = GameObject.Find("SkillAddUI");
        equipInfoUI = GameObject.Find("EquipInfoUI");
        myInfoUI = GameObject.Find("MyInfoUI");
        roomCreateExitBtn = roomCreateUI.transform.GetChild(6).GetComponent<Button>();
        skillAddExitBtn = skillAddUI.transform.GetChild(3).GetComponent<Button>();
        equipInfoExitBtn = equipInfoUI.transform.GetChild(2).GetComponent<Button>();
        myInfoExitBtn = myInfoBtn.transform.GetChild(1).GetComponent<Button>();
    }

    public void InitializeAddListner()
    {
        roomCreateBtn.onClick.AddListener(() => RoomCreate());
        skillAddBtn.onClick.AddListener(() => SkillAdd());
        equipInfoBtn.onClick.AddListener(() => EquipInfo());
        myInfoBtn.onClick.AddListener(() => MyInfo());
        roomCreateBtn.onClick.AddListener(() => BtnActiveCheck(0));
        skillAddExitBtn.onClick.AddListener(() => BtnActiveCheck(1));
        equipInfoExitBtn.onClick.AddListener(() => BtnActiveCheck(2));
        myInfoExitBtn.onClick.AddListener(() => BtnActiveCheck(4));
    }

    public void RoomCreate()
	{
		BtnActiveCheck (selectNum = 0);

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

	private void BtnActiveCheck()
	{
		if (selectNum == 0) {
			roomCreateUI.SetActive (false);
		} else if (selectNum == 1) {
			skillAddUI.SetActive (false);
		} else if (selectNum == 2) {
			equipInfoUI.SetActive (false);
		} else if (selectNum == 3) {
			myInfoUI.SetActive (false);
		}
	}

	public void BtnActiveCheck(int btnIndex)
	{
		if (btnIndex == 0) {
			roomCreateUI.SetActive (false);
		} else if (btnIndex == 1) {
			skillAddUI.SetActive (false);
		} else if (btnIndex == 2) {
			equipInfoUI.SetActive (false);
		} else if (btnIndex == 3) {
			myInfoUI.SetActive (false);
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

    public void SetUserNum(int newUserNum)
    {
        userNum = newUserNum;
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
        DataSender.Instance.RequestSpawnMonsterList();
        DataSender.Instance.RequestDungeonData();
    }
}

public class Room
{
    string roomName;
    int dungeonId;
    int dungeonLevel;
    int[] userClass;
    CharacterStatus.Gender[] userGender;
    string[] userName;
    int[] userLevel;

    public string RoomName { get { return roomName; } }
    public int DungeonId { get { return dungeonId; } }
    public int DungeonLevel { get { return dungeonLevel; } }
    public string[] UserName { get { return userName; } }
    public CharacterStatus.Gender[] UserGender { get { return userGender; } }
    public int[] UserClass { get { return userClass; } }
    public int[] UserLevel { get { return userLevel; } }

    public Room()
    {
        roomName = "";
        dungeonId = 0;
        dungeonLevel = 0;
        userName = new string[WaitingUIManager.maxPlayerNum];
        userGender = new CharacterStatus.Gender[WaitingUIManager.maxPlayerNum];
        userClass = new int[WaitingUIManager.maxPlayerNum];
        userLevel = new int[WaitingUIManager.maxPlayerNum];
    }

    //데이터 수신시 초기화
    public Room(string newName, int newDungeonId, int newDungeonLevel, string[] newUserName, byte[] newUserGender, byte[] newUserClass, byte[] newUserLevel)
    {
        roomName = newName;
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;

        for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
        {
            userName[i] = newUserName[i];
            userGender[i] = (CharacterStatus.Gender)newUserGender[i];
            userClass[i] = newUserClass[i];
            userLevel[i] = newUserLevel[i];
        }
    }
}