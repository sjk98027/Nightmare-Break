using UnityEngine;
using System.Collections;

public class WaitUIManager : MonoBehaviour {

    public const int maxRoomNum = 20;
    public const int maxPlayerNum = 4;

    Room[] rooms;

    public Room[] Rooms { get { return rooms; } }

    public void SetRoom(RoomListData roomListData)
    {
        rooms = roomListData.Rooms;
    }
}
public class Room
{
    string roomName;
    int dungeonId;
    int dungeonLevel;
    int[] userClass;
    CharacterManager.Gender[] userGender;
    string[] userName;
    int[] userLevel;

    public string RoomName { get { return roomName; } }
    public int DungeonId { get { return dungeonId; } }
    public int DungeonLevel { get { return dungeonLevel; } }
    public string[] UserName { get { return userName; } }
    public CharacterManager.Gender[] UserGender { get { return userGender; } }
    public int[] UserClass { get { return userClass; } }
    public int[] UserLevel { get { return userLevel; } }

    public Room()
    {
        roomName = "";
        dungeonId = 0;
        dungeonLevel = 0;
        userName = new string[WaitUIManager.maxPlayerNum];
        userGender = new CharacterManager.Gender[WaitUIManager.maxPlayerNum];
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
            userGender[i] = (CharacterManager.Gender)newUserGender[i];
            userClass[i] = newUserClass[i];
            userLevel[i] = newUserLevel[i];
        }        
    }
}