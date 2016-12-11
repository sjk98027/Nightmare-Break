using System.Text;

public class DungeonDataPacket : Packet<DungeonData>
{
    public class DungeonDataSerializer : Serializer
    {
        public bool Serialize(DungeonData data)
        {
            bool ret = true;
            ret &= Serialize(data.MonsterNum);

            for (int i = 0; i < data.MonsterNum; i++)
            {
                ret &= Serialize(data.MonsterData[i].Id);
                ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.MonsterData[i].Name).Length);
                ret &= Serialize(data.MonsterData[i].Name);
                ret &= Serialize(data.MonsterData[i].MonsterLevelData[0].Level);
                ret &= Serialize(data.MonsterData[i].MonsterLevelData[0].Attack);
                ret &= Serialize(data.MonsterData[i].MonsterLevelData[0].Defense);
                ret &= Serialize(data.MonsterData[i].MonsterLevelData[0].HealthPoint);
                ret &= Serialize(data.MonsterData[i].MonsterLevelData[0].MoveSpeed);
            }

            return ret;
        }

        public bool Deserialize(ref DungeonData element)
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
            element = new DungeonData(monsterNum);

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

                element.MonsterData[i] = new MonsterBaseData(id, _name);
                element.MonsterData[i].AddLevelData(new MonsterLevelData(level, attack, defense, healthPoint, moveSpeed));
            }

            return ret;
        }
    }

    public DungeonDataPacket(DungeonData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public DungeonDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new DungeonData();
        DungeonDataSerializer serializer = new DungeonDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        DungeonDataSerializer serializer = new DungeonDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class DungeonData
{
    byte monsterNum;
    MonsterBaseData[] monsterData;

    public byte MonsterNum { get { return monsterNum; } }
    public MonsterBaseData[] MonsterData { get { return monsterData; } }

    public DungeonData()
    {
        monsterNum = 0;
        monsterData = new MonsterBaseData[monsterNum];
    }

    public DungeonData(byte newMonsterNum)
    {
        monsterNum = newMonsterNum;
        monsterData = new MonsterBaseData[monsterNum];

        for (int i = 0; i < monsterNum; i++)
        {
            monsterData[i] = new MonsterBaseData();
        }
    }

    public DungeonData(byte newMonsterNum, MonsterBaseData[] newMonsterData)
    {
        monsterNum = newMonsterNum;
        monsterData = newMonsterData;
    }
}