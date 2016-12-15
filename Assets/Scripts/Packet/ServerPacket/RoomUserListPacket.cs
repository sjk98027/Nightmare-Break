﻿using System.Text;

public class RoomUserListPacket : Packet<RoomUserList>
{
    public class RoomUserListSerializer : Serializer
    {
        public bool Serialize(RoomUserList data)
        {
            bool ret = true;

            for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
            {
                ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.RoomUserData[i].UserName).Length);
                ret &= Serialize(data.RoomUserData[i].UserName);
                ret &= Serialize((byte)data.RoomUserData[i].UserGender);
                ret &= Serialize((byte)data.RoomUserData[i].UserClass);
                ret &= Serialize((byte)data.RoomUserData[i].UserLevel);
            }

            return ret;
        }

        public bool Deserialize(ref RoomUserList element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte[] userNameLength = new byte[WaitingUIManager.maxPlayerNum];
            string[] userName = new string[WaitingUIManager.maxPlayerNum];
            byte[] userGender = new byte[WaitingUIManager.maxPlayerNum];
            byte[] userClass = new byte[WaitingUIManager.maxPlayerNum];
            byte[] userLevel = new byte[WaitingUIManager.maxPlayerNum];

            for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
            {
                ret &= Deserialize(ref userNameLength[i]);
                ret &= Deserialize(out userName[i], userNameLength[i]);
                ret &= Deserialize(ref userGender[i]);
                ret &= Deserialize(ref userClass[i]);
                ret &= Deserialize(ref userLevel[i]);
            }

            element = new RoomUserList(userName, userGender, userClass, userLevel);

            return ret;
        }
    }

    public RoomUserListPacket(RoomUserList data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public RoomUserListPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new RoomUserList();
        RoomUserListSerializer serializer = new RoomUserListSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        RoomUserListSerializer serializer = new RoomUserListSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class RoomUserList
{
    RoomUserData[] roomUserData;

    public RoomUserData[] RoomUserData { get { return roomUserData; } }

    public RoomUserList()
    {
        roomUserData = new RoomUserData[WaitingUIManager.maxPlayerNum];

        for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
        {
            roomUserData[i] = new RoomUserData();
        }
    }

    public RoomUserList(string[] newUserName, byte[] newUserGender, byte[] newUserClass, byte[] newUserLevel)
    {
        roomUserData = new RoomUserData[WaitingUIManager.maxPlayerNum];

        for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
        {
            roomUserData[i] = new RoomUserData(newUserName[i], newUserGender[i], newUserClass[i], newUserLevel[i]);
        }
    }

    public RoomUserList(Room room)
    {
        roomUserData = room.RoomUserData;
    }
}