public abstract class Packet<T>
{
    protected int packetId;
    public void SetPacketId(int newId) { packetId = newId; }
    public int GetPacketId() { return packetId; }
    public abstract T GetData(); // 데이터 얻기
    public abstract byte[] GetPacketData(); // 바이너리 데이터 얻기
}