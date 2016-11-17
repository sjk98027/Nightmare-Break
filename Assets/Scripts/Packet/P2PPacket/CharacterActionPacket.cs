public class CharacterActionPacket : Packet<CharacterActionData>
{
    public class CharacterActionSerializer : Serializer
    {
        public bool Serialize(CharacterActionData data)
        {
            bool ret = true;
            ret &= Serialize(data.action);
            return ret;
        }

        public bool Deserialize(ref CharacterActionData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte action = 0;

            ret &= Deserialize(ref action);
            element.action = action;

            return ret;
        }
    }

    CharacterActionData m_data;

    public CharacterActionPacket(CharacterActionData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public CharacterActionPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new CharacterActionData();
        CharacterActionSerializer serializer = new CharacterActionSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        CharacterActionSerializer serializer = new CharacterActionSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public override CharacterActionData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }
}

public class CharacterActionData
{
    public byte action;

    public CharacterActionData()
    {
        action = 0;
    }

    public CharacterActionData(int newAction)
    {
        action = (byte)newAction;
    }
}