using System.Text;

public class MonsterDataPacket : Packet<MonsterDataList>
{
    public class MonsterDataSerializer : Serializer
    {
        public bool Serialize(MonsterDataList data)
        {
            bool ret = true;
            ret &= Serialize(data.MonsterNum);

            for (int i = 0; i < data.MonsterNum; i++)
            {

            }

            return ret;
        }

        public bool Deserialize(ref MonsterDataList element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte monsterNum = 0;
            byte id = 0;
            byte nameLength = 0;
            string _name;
            byte level = 0;
            byte attack = 0;
            byte defense = 0;
            byte healthPoint = 0;
            byte moveSpeed = 0;

            ret &= Deserialize(ref monsterNum);
            element = new MonsterDataList(monsterNum);

            for (int i = 0; i < monsterNum; i++)
            {
                ret &= Deserialize(ref id);
                ret &= Deserialize(ref nameLength);
                ret &= Deserialize(out _name, nameLength);
                ret &= Deserialize(ref level);
                ret &= Deserialize(ref attack);
                ret &= Deserialize(ref defense);
                ret &= Deserialize(ref healthPoint);
                ret &= Deserialize(ref moveSpeed);
                
            }

            return ret;
        }
    }

    public MonsterDataPacket(MonsterDataList data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public MonsterDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new MonsterDataList();
        MonsterDataSerializer serializer = new MonsterDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        MonsterDataSerializer serializer = new MonsterDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class MonsterDataList
{
    byte monsterNum;
    MonsterBaseData[] monsterBaseData;

    public byte MonsterNum { get { return monsterNum; } }
    public MonsterBaseData[] MonsterBaseData { get { return monsterBaseData; } }

    public MonsterDataList()
    {
        monsterNum = 0;
        monsterBaseData = new MonsterBaseData[monsterNum];
    }

    public MonsterDataList(byte newMonsterNum)
    {
        monsterNum = newMonsterNum;
        monsterBaseData = new MonsterBaseData[monsterNum];

        for (int i = 0; i < monsterNum; i++)
        {
            MonsterBaseData[i] = new MonsterBaseData();
        }
    }

    public MonsterDataList(byte newMonsterNum, MonsterBaseData[] newMonsterData)
    {
        monsterNum = newMonsterNum;
        monsterBaseData = newMonsterData;
    }
}