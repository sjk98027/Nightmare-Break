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
                ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.Rooms[i].RoomName).Length);
                ret &= Serialize(data.Rooms[i].RoomName);
                ret &= Serialize(data.Rooms[i].DungeonId);
                ret &= Serialize(data.Rooms[i].DungeonLevel);

                for (int j = 0; j < WaitUIManager.maxPlayerNum; j++)
                {
                    ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.Rooms[i].UserName[j]).Length);
                    ret &= Serialize(data.Rooms[i].UserName[j]);
                    ret &= Serialize((byte)data.Rooms[i].UserGender[j]);
                    ret &= Serialize(data.Rooms[i].UserClass[j]);
                    ret &= Serialize(data.Rooms[i].UserLevel[j]);
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

            byte roomNameLength = 0;
            string roomName;
            byte dungeonId = 0;
            byte dungeonLevel = 0;
            byte[] userNameLength = new byte[WaitUIManager.maxPlayerNum];
            string[] userName = new string[ WaitUIManager.maxPlayerNum];
            byte[] userGender = new byte[WaitUIManager.maxPlayerNum];
            byte[] userClass = new byte[WaitUIManager.maxPlayerNum];
            byte[] userLevel = new byte[WaitUIManager.maxPlayerNum];

            for (int i = 0; i < WaitUIManager.maxRoomNum; i++)
            {
                ret &= Deserialize(ref roomNameLength);
                ret &= Deserialize(out roomName, roomNameLength);
                ret &= Deserialize(ref dungeonId);
                ret &= Deserialize(ref dungeonLevel);

                for (int j = 0; j< WaitUIManager.maxPlayerNum; j++)
                {
                    ret &= Deserialize(ref userNameLength[j]);
                    ret &= Deserialize(out userName[j], userNameLength[j]);
                    ret &= Deserialize(ref userGender[j]);
                    ret &= Deserialize(ref userClass[j]);
                    ret &= Deserialize(ref userLevel[j]);
                }

                element.Rooms[i] = new Room(roomName, dungeonId, dungeonLevel, userName, userGender, userClass, userLevel);
            }

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
    Room[] rooms;

    public Room[] Rooms { get { return rooms; } }

    public RoomListData()
    {
        rooms = new Room[WaitUIManager.maxRoomNum];

        for (int i =0; i< WaitUIManager.maxRoomNum; i++)
        {
            rooms[i] = new Room();
        }
    }
}