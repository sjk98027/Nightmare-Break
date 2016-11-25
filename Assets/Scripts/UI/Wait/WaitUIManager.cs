using UnityEngine;
using UnityEngine.UI;

public class WaitUIManager : MonoBehaviour
{
    public const int maxRoomNum = 20;
    public const int maxPlayerNum = 4;

    Button createRoomButton;
    Button enterRoomButton;
    Button exitRoomButton;
    Button startGameButton;

    Room[] rooms;
    int currentRoomNum;

    public Room[] Rooms { get { return rooms; } }

    public void SetRoom(RoomListData roomListData)
    {
        rooms = roomListData.Rooms;
    }

    public void OnClickCreateRoomButton(string roomName, int dungeonId, int dungeonLevel)
    {
        DataSender.Instance.CreateRoom(roomName, dungeonId, dungeonLevel);
    }

    public void OnClickEnterRoomButton(int roomNum)
    {
        DataSender.Instance.EnterRoom(roomNum);
    }

    public void OnClickExitRoomButton()
    {
        DataSender.Instance.ExitRoom(currentRoomNum);
    }

    public void OnClickStartGameButton()
    {
        DataSender.Instance.StartGame();
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
        userName = new string[WaitUIManager.maxPlayerNum];
        userGender = new CharacterStatus.Gender[WaitUIManager.maxPlayerNum];
        userClass = new int[WaitUIManager.maxPlayerNum];
        userLevel = new int[WaitUIManager.maxPlayerNum];
    }

    //데이터 수신시 초기화
    public Room(string newName, int newDungeonId, int newDungeonLevel, string[] newUserName, byte[] newUserGender, byte[] newUserClass, byte[] newUserLevel)
    {
        roomName = newName;
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;

        for(int i = 0; i< WaitUIManager.maxPlayerNum; i++)
        {
            userName[i] = newUserName[i];
            userGender[i] = (CharacterStatus.Gender)newUserGender[i];
            userClass[i] = newUserClass[i];
            userLevel[i] = newUserLevel[i];
        }        
    }
}