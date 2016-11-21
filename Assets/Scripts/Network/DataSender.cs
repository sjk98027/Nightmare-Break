using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class DataSender : MonoBehaviour
{
    NetworkManager networkManager;
    CharacterManager characterManager;

    Socket tcpSock;
    Socket udpSock;

    Queue<DataPacket> sendMsgs;

    byte[] udpMsg;

    public void Initialize(Queue<DataPacket> newSendMsgs, Socket newTcpSock, Socket newUdpSock)
    {
        networkManager = GetComponent<NetworkManager>();

        sendMsgs = newSendMsgs;
        tcpSock = newTcpSock;
        udpSock = newUdpSock;

        udpMsg = new byte[0];
    }

    //데이타를 전송하는 메소드. byte[] msg 를 newIPEndPoint로 전송한다.
    public void DataSend()
    {
        if (sendMsgs.Count > 0)
        {
            DataPacket packet;
            
            packet = sendMsgs.Dequeue();

            if (packet.endPoint != null)
            {
                udpSock.BeginSendTo(packet.msg, 0, packet.msg.Length, SocketFlags.None, packet.endPoint, new AsyncCallback(SendData), null);
            }
            else if (packet.endPoint == null)
            {
                tcpSock.Send(packet.msg, 0, packet.msg.Length, SocketFlags.None);
            }
        }
    }

    //비동기 콜백 메소드
    private void SendData(IAsyncResult ar)
    {
        udpSock.EndSend(ar);
    }

    //계정 생성 - Tcp
    public void CreateAccount(string id, string pw)
    {
        Debug.Log("가입 요청");

        AccountData accountData = new AccountData(id, pw);
        AccountPacket accountDataPacket = new AccountPacket(accountData);
        accountDataPacket.SetPacketId((int)ClientPacketId.CreateAccount);
        
        byte[] packet = CreatePacket(accountDataPacket);
    }

    //게임 종료 - Tcp
    public void GameClose()
    {
        Debug.Log("게임 종료");

        ResultData resultData = new ResultData();
        ResultDataPacket resultDataPacket = new ResultDataPacket(resultData);
        resultDataPacket.SetPacketId((int)ClientPacketId.GameClose);

        byte[] msg = CreatePacket(resultDataPacket);

        Debug.Log("메시지 보냄 (길이) : " + msg.Length);
        Debug.Log("메시지 보냄 (출처) : " + msg[2]);
        Debug.Log("메시지 보냄 (타입) : " + msg[3]);

        tcpSock.Send(msg, 0, msg.Length, SocketFlags.None);

        try
        {
            tcpSock.Close();
            udpSock.Close();
        }
        catch
        {
            Debug.Log("이미 소켓이 닫혀있습니다.");
        }        
    }

    //연결 확인 - Udp
    public void ConnectionCheck(List<EndPoint> newEndPoint)
    {
        Debug.Log("연결 체크");
        ResultData resultData = new ResultData(new byte());
        ResultDataPacket resultDataPacket = new ResultDataPacket(resultData);
        resultDataPacket.SetPacketId((int)P2PPacketId.ConnectionCheck);

        DataPacket packet = new DataPacket(CreatePacket(resultDataPacket), null);

        foreach (EndPoint client in networkManager.Clients)
        {
            packet.endPoint = client;
            sendMsgs.Enqueue(packet);
        }
    }

    //캐릭터의 생성 - Udp
    public void CreateUnitSend(short newId, Vector3 position)
    {
        short id = newId;
        float xPos = position.x;
        float yPos = position.y;
        float zPos = position.z;

        CreateUnitData createUnitData = new CreateUnitData(id, xPos, yPos, zPos);
        CreateUnitPacket createUnitDataPacket = new CreateUnitPacket(createUnitData);
        createUnitDataPacket.SetPacketId((int)P2PPacketId.CreateUnit);

        byte[] packet = CreatePacket(createUnitDataPacket);
        
        udpMsg = CombineByte(udpMsg, packet);
    }

    //캐릭터 움직임 - Udp
    public IEnumerator CharacterPositionSend()
    {
        characterManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterManager>();

        while (true)
        {
            yield return null;

            short time = (short)((DateTime.Now - networkManager.DataHandler.dTime).TotalSeconds);
            bool dir = characterManager.charDir;
            float xPos = characterManager.transform.position.x;
            float yPos = characterManager.transform.position.y;
            float zPos = characterManager.transform.position.z;

            CharacterPositionData CharacterPosition = new CharacterPositionData(time, dir, xPos, yPos, zPos);
            CharacterPositionPacket characterStatePacket = new CharacterPositionPacket(CharacterPosition);
            characterStatePacket.SetPacketId((int)P2PPacketId.CharacterPosition);

            byte[] packet = CreatePacket(characterStatePacket);

            udpMsg = CombineByte(udpMsg, packet);
        }
    }

    //캐릭터 움직임(공격, 점프, 스킬 등등) - Udp
    public void CharacterActionSend(int action)
    {
        CharacterActionData characterActionData = new CharacterActionData(action);
        CharacterActionPacket characterActionPacket = new CharacterActionPacket(characterActionData);
        characterActionPacket.SetPacketId((int)P2PPacketId.CharacterAction);

        byte[] packet = CreatePacket(characterActionPacket);

        udpMsg = CombineByte(udpMsg, packet);
    }

    //0.1초 마다 Udp메시지를 큐에 넣는다.
    public IEnumerator EnqueueMessage()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            DataPacket packet = new DataPacket(udpMsg, null);

            foreach (EndPoint client in networkManager.Clients)
            {
                packet.endPoint = client;
                sendMsgs.Enqueue(packet);
            }

            udpMsg = new byte[0];
        }        
    }

    //패킷의 헤더 생성
    byte[] CreateHeader<T>(Packet<T> data)
    {
        byte[] msg = data.GetPacketData();

        HeaderData headerData = new HeaderData();
        HeaderSerializer headerSerializer = new HeaderSerializer();

        headerData.length = (short)msg.Length;
        headerData.source = (byte)NetworkManager.Source.ServerSource;
        headerData.id = (byte)data.GetPacketId();

        headerSerializer.Serialize(headerData);
        byte[] header = headerSerializer.GetSerializedData();

        return header;
    }

    //패킷 생성
    byte[] CreatePacket<T>(Packet<T> data)
    {
        byte[] msg = data.GetPacketData();
        byte[] header = CreateHeader(data);
        byte[] packet = CombineByte(header, msg);

        return packet;
    }

    public static byte[] CombineByte(byte[] array1, byte[] array2)
    {
        byte[] array3 = new byte[array1.Length + array2.Length];
        Array.Copy(array1, 0, array3, 0, array1.Length);
        Array.Copy(array2, 0, array3, array1.Length, array2.Length);
        return array3;
    }

    public static byte[] CombineByte(byte[] array1, byte[] array2, byte[] array3)
    {
        byte[] array4 = CombineByte(CombineByte(array1, array2), array3);
        return array4;
    }
}