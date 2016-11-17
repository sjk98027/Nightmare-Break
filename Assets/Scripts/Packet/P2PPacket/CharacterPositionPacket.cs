public class CharacterPositionPacket : Packet<CharacterPositionData>
{
    public class CharacterPositionSerializer : Serializer
    {
        public bool Serialize(CharacterPositionData data)
        {
            bool ret = true;
            ret &= Serialize(data.time);
            ret &= Serialize(data.dir);
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
            short time = 0;
            bool dir = true;
            float posX = 0;
            float posY = 0;
            float posZ = 0;

            ret &= Deserialize(ref time);
            ret &= Deserialize(ref dir);
            ret &= Deserialize(ref posX);
            ret &= Deserialize(ref posY);
            ret &= Deserialize(ref posZ);
            element.time = time;
            element.dir = dir;
            element.posX = posX;
            element.posY = posY;
            element.posZ = posZ;

            return ret;
        }
    }

    CharacterPositionData m_data;

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

    public override CharacterPositionData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }
}

public class CharacterPositionData
{
    public short time;   
    public bool dir;
    public float posX;
    public float posY;
    public float posZ;

    public CharacterPositionData()
    {
        time = 0;
        dir = true;
        posX = 0;
        posY = 0;
        posZ = 0;
    }

    public CharacterPositionData(short newTime, bool newDir, float newX, float newY, float newZ)
    {
        time = newTime;
        dir = newDir;
        posX = newX;
        posY = newY;
        posZ = newZ;
    }
}