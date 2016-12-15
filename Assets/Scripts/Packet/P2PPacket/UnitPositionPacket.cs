public class UnitPositionPacket : Packet<UnitPositionData>
{
    public class UnitPositionSerializer : Serializer
    {
        public bool Serialize(UnitPositionData data)
        {
            bool ret = true;

            ret &= Serialize(data.Dir);
            ret &= Serialize(data.UnitIndex);
            ret &= Serialize(data.PosX);
            ret &= Serialize(data.PosY);
            ret &= Serialize(data.PosZ);
            return ret;
        }

        public bool Deserialize(ref UnitPositionData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            bool dir = true;
            byte unitIndex = 0;
            float posX = 0;
            float posY = 0;
            float posZ = 0;

            ret &= Deserialize(ref dir);
            ret &= Deserialize(ref unitIndex);
            ret &= Deserialize(ref posX);
            ret &= Deserialize(ref posY);
            ret &= Deserialize(ref posZ);

            element = new UnitPositionData(dir, unitIndex, posX, posY, posZ);

            return ret;
        }
    }

    public UnitPositionPacket(UnitPositionData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public UnitPositionPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new UnitPositionData();
        UnitPositionSerializer serializer = new UnitPositionSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        UnitPositionSerializer serializer = new UnitPositionSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class UnitPositionData
{
    bool dir;
    byte unitIndex;
    float posX;
    float posY;
    float posZ;

    public bool Dir { get { return dir; } }
    public byte UnitIndex { get { return unitIndex; } }
    public float PosX { get { return posX; } }
    public float PosY { get { return posY; } }
    public float PosZ { get { return posZ; } }
    public UnitPositionData()
    {
        dir = true;
        unitIndex = 0;
        posX = 0;
        posY = 0;
        posZ = 0;
    }

    public UnitPositionData(bool newDir, byte newUserIndex, float newX, float newY, float newZ)
    {
        dir = newDir;
        unitIndex = newUserIndex;
        posX = newX;
        posY = newY;
        posZ = newZ;
    }
}