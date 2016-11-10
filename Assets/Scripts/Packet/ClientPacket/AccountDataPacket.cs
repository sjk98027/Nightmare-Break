
public class AccountDataPacket : IPacket<AccountData>
{
    public class AccountDataSerializer : Serializer
    {
        public bool Serialize(AccountData data)
        {
            bool ret = true;
            ret &= Serialize(data.Id);
            ret &= Serialize(".");
            ret &= Serialize(data.Pw);
            return ret;
        }

        public bool Deserialize(ref AccountData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            string total;
            ret &= Deserialize(out total, (int)GetDataSize());

            string[] str = total.Split('.');
            if (str.Length < AccountData.accountDataLength)
            {
                return false;
            }

            element = new AccountData(str[0], str[1]);

            return ret;
        }
    }

    AccountData m_data;
    int packetId;

    public int PacketId { get { return packetId; } set { packetId = value; } }

    public AccountDataPacket(AccountData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public AccountDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new AccountData();
        AccountDataSerializer serializer = new AccountDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        AccountDataSerializer serializer = new AccountDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public AccountData GetData()
    {
        return m_data;
    }
}

public class AccountData
{
    public const int accountDataLength = 2;
    string id;
    string pw;

    public string Id { get { return id; } }
    public string Pw { get { return pw; } }

    public AccountData()
    {
        id = "";
        pw = "";
    }

    public AccountData(string newId, string newPw)
    {
        id = newId;
        pw = newPw;
    }
}