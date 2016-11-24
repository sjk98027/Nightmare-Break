using System.Text;

public class RoomListPacket : Packet<RoomListData>
{
    public class RoomListSerializer : Serializer
    {
        public bool Serialize(RoomListData data)
        {
            bool ret = true;

            for (int i = 0; i < WaitUIManager.maxRoomNum; i++)
            {
                ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.RoomName[i]).Length);
                ret &= Serialize(data.RoomName[i]);
                ret &= Serialize(data.DungeonId[i]);
                ret &= Serialize(data.DungeonLevel[i]);

                for (int j = 0; j < WaitUIManager.maxPlayerNum; j++)
                {
                    ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.UserName[i, j]).Length);
                    ret &= Serialize(data.UserName[i, j]);
                    ret &= Serialize(data.UserGender[i, j]);
                    ret &= Serialize(data.UserClass[i, j]);
                    ret &= Serialize(data.UserLevel[i, j]);
                }
            }

            return ret;
        }

        public bool Deserialize(ref RoomListData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte[] roomNameLength = new byte[WaitUIManager.maxRoomNum];
            string[] roomName = new string[WaitUIManager.maxRoomNum];
            byte[] dungeonId = new byte[WaitUIManager.maxRoomNum];
            byte[] dungeonLevel = new byte[WaitUIManager.maxRoomNum];
            byte[,] userNameLength = new byte[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];
            string[,] userName = new string[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];
            byte[,] userGender = new byte[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];
            byte[,] userClass = new byte[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];
            byte[,] userLevel = new byte[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];

            for (int i = 0; i < WaitUIManager.maxRoomNum; i++)
            {
                ret &= Deserialize(ref roomNameLength[i]);
                ret &= Deserialize(out roomName[i], roomNameLength[i]);
                ret &= Deserialize(ref dungeonId[i]);
                ret &= Deserialize(ref dungeonLevel[i]);

                for (int j = 0; j < WaitUIManager.maxPlayerNum; j++)
                {
                    ret &= Deserialize(ref userNameLength[i, j]);
                    ret &= Deserialize(out userName[i, j], userNameLength[i, j]);
                    ret &= Deserialize(ref userGender[i, j]);
                    ret &= Deserialize(ref userClass[i, j]);
                    ret &= Deserialize(ref userLevel[i, j]);
                }
            }
            element = new RoomListData(roomName, dungeonId, dungeonLevel, userName, userGender, userClass, userLevel);

            return ret;
        }
    }

    public RoomListPacket(RoomListData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public RoomListPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new RoomListData();
        RoomListSerializer serializer = new RoomListSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        RoomListSerializer serializer = new RoomListSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class RoomListData
{
    byte[] roomNameLength;
    string[] roomName;
    byte[] dungeonId;
    byte[] dungeonLevel;
    byte[,] userNameLength;
    string[,] userName;
    byte[,] userGender;
    byte[,] userClass;
    byte[,] userLevel;

    public byte[] RoomNameLength { get { return roomNameLength; } }
    public string[] RoomName { get { return roomName; } }
    public byte[] DungeonId { get { return dungeonId; } }
    public byte[] DungeonLevel { get { return dungeonLevel; } }
    public byte[,] UserNameLength { get { return userNameLength; } }
    public string[,] UserName { get { return userName; } }
    public byte[,] UserGender { get { return userGender; } }
    public byte[,] UserClass { get { return userClass; } }
    public byte[,] UserLevel { get { return userLevel; } }

    public RoomListData()
    {
        roomNameLength = new byte[WaitUIManager.maxRoomNum];
        roomName = new string[WaitUIManager.maxRoomNum];
        dungeonId = new byte[WaitUIManager.maxRoomNum];
        dungeonLevel = new byte[WaitUIManager.maxRoomNum];
        userNameLength = new byte[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];
        userName = new string[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];
        userGender = new byte[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];
        userClass = new byte[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];
        userLevel = new byte[WaitUIManager.maxRoomNum, WaitUIManager.maxPlayerNum];
    }

    public RoomListData(string[] newRoomName, byte[] newDungeonId, byte[] newDungeonLevel, string[,] newUserName, byte[,]newUserGender, byte[,] newUserClass, byte[,] newUserLevel)
    {
        roomName = newRoomName;
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
        userName = newUserName;
        userGender = newUserGender;
        userClass = newUserClass;
        userLevel = newUserLevel;
    }

    public RoomListData(Room[] rooms)
    {
        for (int i = 0; i < WaitUIManager.maxRoomNum; i++)
        {
            roomName[i] = rooms[i].RoomName;
            dungeonId[i] = (byte)rooms[i].DungeonId;
            dungeonLevel[i] = (byte)rooms[i].DungeonLevel;

            for (int j = 0; j < WaitUIManager.maxPlayerNum; j++)
            {
                userName[i, j] = rooms[i].USerName[j];
                userGender[i, j] = (byte)rooms[i].UserGender[j];
                userClass[i, j] = (byte)rooms[i].UserClass[j];
                userLevel[i, j] = (byte)rooms[i].UserLevel[j];
            }
        }
    }
}