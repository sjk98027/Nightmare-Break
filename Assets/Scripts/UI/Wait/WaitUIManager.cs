using UnityEngine;
using System.Collections;

public class WaitUIManager : MonoBehaviour {

    public const int maxRoomNum = 20;
    public const int maxPlayerNum = 4;

    Room[] rooms;

    public Room[] Rooms { get { return rooms; } }

    public void SetRoom(RoomListData roomListData)
    {
        string[] tmpName = new string[maxPlayerNum];
        CharacterManager.Gender[] tmpGender = new CharacterManager.Gender[maxPlayerNum];
        int[] tmpClass = new int[maxPlayerNum];
        int[] tmpLevel = new int[maxPlayerNum];

        for (int i = 0; i < maxRoomNum; i++)
        {
            for(int j = 0; j< maxPlayerNum; j++)
            {
                tmpName[j] = roomListData.UserName[i, j];
                tmpGender[j] = (CharacterManager.Gender)roomListData.UserGender[i, j];
                tmpClass[j] = roomListData.UserClass[i, j];
                tmpLevel[j] = roomListData.UserLevel[i, j];
            }

            rooms[i] = new Room(roomListData.RoomName[i], roomListData.DungeonId[i], roomListData.DungeonLevel[i], tmpName, tmpGender, tmpClass, tmpLevel);
        }
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
    public string[] USerName { get { return userName; } }
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

    public Room(string newName, int newDungeonId, int newDungeonLevel)
    {
        roomName = newName;
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
        userName = new string[WaitUIManager.maxPlayerNum];
        userGender = new CharacterManager.Gender[WaitUIManager.maxPlayerNum];
        userClass = new int[WaitUIManager.maxPlayerNum];
        userLevel = new int[WaitUIManager.maxPlayerNum];
    }

    public Room(string newName, int newDungeonId, int newDungeonLevel, string[] newUserName, CharacterManager.Gender[] newUserGender, int[] newUserClass, int[] newUserLevel)
    {
        roomName = newName;
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
        userName = newUserName;
        userGender = newUserGender;
        userClass = newUserClass;
        userLevel = newUserLevel;
    }
}