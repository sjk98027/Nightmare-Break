public class CharacterPositionPacket : Packet<CharacterPositionData>
{
    public class CharacterPositionSerializer : Serializer
    {
        public bool Serialize(CharacterPositionData data)
        {
            bool ret = true;

            ret &= Serialize(data.dir);
            ret &= Serialize(data.userIndex);
            ret &= Serialize(data.posX);
            ret &= Serialize(data.posY);
            ret &= Serialize(data.posZ);
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
            element.dir = dir;
            element.userIndex = userIndex;
            element.posX = posX;
            element.posY = posY;
            element.posZ = posZ;

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
    public bool dir;
    public byte userIndex;
    public float posX;
    public float posY;
    public float posZ;

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