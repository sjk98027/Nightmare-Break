public class CharacterPositionPacket : Packet<CharacterPositionData>
{
    public class CharacterPositionSerializer : Serializer
    {
        public bool Serialize(CharacterPositionData data)
        {
            bool ret = true;

            ret &= Serialize(data.Dir);
            ret &= Serialize(data.UserIndex);
            ret &= Serialize(data.PosX);
            ret &= Serialize(data.PosY);
            ret &= Serialize(data.PosZ);
            return ret;
        }

        public bool Deserialize(ref CharacterPositionData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            bool dir = true;
            byte userIndex = 0;
            float posX = 0;
            float posY = 0;
            float posZ = 0;
            
            ret &= Deserialize(ref dir);
            ret &= Deserialize(ref userIndex);
            ret &= Deserialize(ref posX);
            ret &= Deserialize(ref posY);
            ret &= Deserialize(ref posZ);

            element = new CharacterPositionData(dir, userIndex, posX, posY, posZ);

            return ret;
        }
    }

    public CharacterPositionPacket(CharacterPositionData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public CharacterPositionPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new CharacterPositionData();
        CharacterPositionSerializer serializer = new CharacterPositionSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        CharacterPositionSerializer serializer = new CharacterPositionSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class CharacterPositionData
{
    bool dir;
    byte userIndex;
    float posX;
    float posY;
    float posZ;

    public bool Dir { get { return dir; } }
    public byte UserIndex { get { return userIndex; } }
    public float PosX { get { return posX; } }
    public float PosY { get { return posY; } }
    public float PosZ { get { return posZ; } }
    public CharacterPositionData()
    {
        dir = true;
        userIndex = 0;
        posX = 0;
        posY = 0;
        posZ = 0;
    }

    public CharacterPositionData(bool newDir, byte newUserIndex, float newX, float newY, float newZ)
    {
        dir = newDir;
        userIndex = newUserIndex;
        posX = newX;
        posY = newY;
        posZ = newZ;
    }
}