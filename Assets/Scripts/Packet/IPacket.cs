public interface IPacket<T>
{
    T GetData(); // 데이터 얻기
    byte[] GetPacketData(); // 바이너리 데이터 얻기
}