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

    float dTime;
    float cTime;

    public void Initialize(Queue<DataPacket> newSendMsgs, Socket newTcpSock, Socket newUdpSock)
    {
        networkManager = GetComponent<NetworkManager>();

        sendMsgs = newSendMsgs;
        tcpSock = newTcpSock;
        udpSock = newUdpSock;
        cTime = Time.time;
        dTime = Time.time;
    }

    //데이타를 전송하는 메소드. byte[] msg 를 newIPEndPoint로 전송한다.
    public void DataSend()
    {
        dTime = Time.time;

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
        AccountDataPacket accountDataPacket = new AccountDataPacket(accountData);
        accountDataPacket.SetPacketId((int)ClientPacketId.CreateAccount);
        
        byte[] packet = CreatePacket(accountDataPacket);
    }

    //게임 종료 - Tcp
    public void GameClose()
    {
        Debug.Log("게임 종료");
        ResultData resultData = new ResultData(new byte());
        ResultDataPacket resultDataPacket = new ResultDataPacket(resultData);
        resultDataPacket.SetPacketId((int)ClientPacketId.GameClose);

        byte[] msg = resultDataPacket.GetPacketData();

        Debug.Log("메시지 보냄 (길이) : " + msg.Length);
        Debug.Log("메시지 보냄 (출처) : " + msg[2]);
        Debug.Log("메시지 보냄 (타입) : " + msg[3]);

        //tcpSock.Send(msg, 0, msg.Length, SocketFlags.None);

        tcpSock.Close();
        udpSock.Close();
    }

    //연결 확인 - Udp
    public void ConnectionCheck(List<EndPoint> newEndPoint)
    {
        Debug.Log("연결 체크");
        ResultData resultData = new ResultData(new byte());
        ResultDataPacket resultDataPacket = new ResultDataPacket(resultData);
        resultDataPacket.SetPacketId((int)P2PPacketId.ConnectionCheck);

        DataPacket packet = new DataPacket(resultDataPacket.GetPacketData(), null);

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
    public IEnumerator CharacterDataSend()
    {
        characterManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterManager>();

        while (true)
        {
            yield return null;

            byte state = (byte)characterManager.State;
            float vertical = characterManager.Animator.GetFloat("Ver");
            float horizontal = characterManager.Animator.GetFloat("Hor");
            float xPos = characterManager.transform.position.x;
            float yPos = characterManager.transform.position.y;
            float zPos = characterManager.transform.position.z;

            CharacterStateData characterStateData = new CharacterStateData(state, horizontal, vertical, xPos, yPos, zPos);
            CharacterStatePacket characterStatePacket = new CharacterStatePacket(characterStateData);

            byte[] packet = CreatePacket(characterStatePacket);

            udpMsg = CombineByte(udpMsg, packet);
        }
    }

    public void EnqueueMessage()
    {
        DataPacket packet = new DataPacket(udpMsg, null);

        foreach (EndPoint client in networkManager.Clients)
        {
            packet.endPoint = client;
            sendMsgs.Enqueue(packet);
        }
    }

    //패킷의 헤더 생성
    byte[] CreateHeader<T>(Packet<T> data)
    {
        byte[] msg = data.GetPacketData();

        HeaderData headerData = new HeaderData();
        HeaderSerializer headerSerializer = new HeaderSerializer();

        headerData.id = (byte)data.GetPacketId();
        headerData.source = (byte)NetworkManager.Source.ServerSource;
        headerData.length = (short)msg.Length;

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