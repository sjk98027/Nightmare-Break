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
    object sendLock;

    public void Initialize(Queue<DataPacket> newSendMsgs, object newSendLock, Socket newTcpSock, Socket newUdpSock)
    {
        networkManager = GetComponent<NetworkManager>();

        sendMsgs = newSendMsgs;
        sendLock = newSendLock;
        tcpSock = newTcpSock;
        udpSock = newUdpSock;
    }

    //데이타를 전송하는 메소드. byte[] msg 를 newIPEndPoint로 전송한다.
    public void DataSend()
    {
        if (sendMsgs.Count > 0)
        {
            DataPacket packet;

            lock (sendLock)
            {
                packet = sendMsgs.Dequeue();
            }

            Debug.Log("메시지 보냄 : " + packet.endPoint);
            Debug.Log("메시지 보냄 (길이) : " + packet.headerData.length);
            Debug.Log("메시지 보냄 (출처) : " + packet.headerData.source);
            Debug.Log("메시지 보냄 (타입) : " + packet.headerData.id);

            HeaderSerializer headerSerializer = new HeaderSerializer();
            headerSerializer.Serialize(packet.headerData);

            byte[] header = headerSerializer.GetSerializedData();
            byte[] msg = CombineByte(header, packet.msg);

            if (packet.headerData.source == (byte)NetworkManager.Source.ClientSource)
            {
                udpSock.BeginSendTo(msg, 0, msg.Length, SocketFlags.None, packet.endPoint, new AsyncCallback(SendData), null);
            }
            else if(packet.headerData.source == (byte)NetworkManager.Source.ServerSource)
            {
                tcpSock.Send(msg, 0, msg.Length, SocketFlags.None);
            }
        }
    }

    //비동기 콜백 메소드
    private void SendData(IAsyncResult ar)
    {
        udpSock.EndSend(ar);
    }

    //연결 확인 - Udp
    public void ConnectionCheck(List<EndPoint> newEndPoint)
    {
        Debug.Log("연결 체크");

        foreach (EndPoint client in newEndPoint)
        {
            Debug.Log("확인할 클라이언트 : " + client.ToString());
            DataPacket packet = CreatePacket(new byte[1], (int)P2PPacketId.ConnectionCheck);
            packet.endPoint = client;
            sendMsgs.Enqueue(packet);
        }
    }

    //계정 생성 - Tcp
    public void CreateAccount(string id, string pw)
    {
        Debug.Log("가입 요청");

        AccountData accountData = new AccountData(id, pw);
        AccountDataPacket accountDataPacket = new AccountDataPacket(accountData);
        accountDataPacket.PacketId = (int)ClientPacketId.CreateAccount;

        DataPacket packet = CreatePacket(accountDataPacket.GetPacketData(), accountDataPacket.PacketId);
    }

    //게임 종료 - Tcp
    public void GameClose()
    {
        Debug.Log("게임 종료");
        DataPacket packet = CreatePacket(new byte[1], (int)ClientPacketId.GameClose);

        HeaderSerializer headerSerializer = new HeaderSerializer();
        headerSerializer.Serialize(packet.headerData);

        byte[] header = headerSerializer.GetSerializedData();
        byte[] msg = CombineByte(header, packet.msg);

        Debug.Log("메시지 보냄 (길이) : " + msg.Length);
        Debug.Log("메시지 보냄 (출처) : " + msg[2]);
        Debug.Log("메시지 보냄 (타입) : " + msg[3]);

        //tcpSock.Send(msg, 0, msg.Length, SocketFlags.None);

        tcpSock.Close();
        udpSock.Close();
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

        DataPacket packet = CreatePacket(createUnitDataPacket.GetPacketData(), (int)P2PPacketId.CreateUnit);
        packet.endPoint = networkManager.client2;
        sendMsgs.Enqueue(packet);
    }

    //캐릭터 움직임 - Udp
    public IEnumerator CharacterDataSend()
    {
        characterManager = GameObject.FindGameObjectWithTag("CharacterManager").GetComponent<CharacterManager>();
        
        while (true)
        {
            yield return new WaitForEndOfFrame();

            byte state = (byte)characterManager.State;
            float vertical = characterManager.Animator.GetFloat("Ver");
            float horizontal = characterManager.Animator.GetFloat("Hor");
            bool direction = characterManager.Animator.GetBool("Direction");
            float xPos = characterManager.transform.position.x;
            float yPos = characterManager.transform.position.y;
            float zPos = characterManager.transform.position.z;

            CharacterStateData characterStateData = new CharacterStateData(state, direction, horizontal, vertical, xPos, yPos, zPos);
            CharacterStatePacket characterStateDataPacket = new CharacterStatePacket(characterStateData);
            
            //현재는 client로 고정되있지만
            //차후 수정으로 매개변수 newIPEndPoint를 설정하여 여러명의 클라이언트에 동시에 보내도록 수정할 예정
            DataPacket packet = CreatePacket(characterStateDataPacket.GetPacketData(), (int)P2PPacketId.CharacterState);
            packet.endPoint = networkManager.client2;
            sendMsgs.Enqueue(packet);
        }
    }

    //패킷의 헤더 생성
    HeaderData CreateHeader(short msgLength, int id)
    {
        HeaderData headerData = new HeaderData();

        headerData.id = (byte)id;
        headerData.source = (byte)NetworkManager.Source.ClientSource;
        headerData.length = msgLength;

        return headerData;
    }
    
    //패킷 생성
    DataPacket CreatePacket(byte[] msg, int id)
    {
        HeaderData header = CreateHeader((short)msg.Length, id);
        DataPacket packet = new DataPacket(header, msg);

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