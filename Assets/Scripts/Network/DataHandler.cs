using System;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    NetworkManager networkManager;
    DungeonManager dungeonManager;

    public Queue<DataPacket> receiveMsgs;

    object receiveLock;

    byte[] msg;

    public delegate P2PPacketId P2PRecvNotifier(byte[] data);
    public delegate ServerPacketId ServerRecvNotifier(byte[] data);
    P2PRecvNotifier p2pRecvNotifier;
    ServerRecvNotifier serverRecvNotifier;
    private Dictionary<int, P2PRecvNotifier> p2p_notifier = new Dictionary<int, P2PRecvNotifier>();
    private Dictionary<int, ServerRecvNotifier> server_notifier = new Dictionary<int, ServerRecvNotifier>();

    public void Initialize(Queue<DataPacket> receiveQueue, object newReceiveLock, Queue<DataPacket> sendQueue, object newSendLock)
    {
        receiveMsgs = receiveQueue;
        receiveLock = newReceiveLock;

        networkManager = GetComponent<NetworkManager>();

        msg = new byte[1024];

        SetServerNotifier();
        SetUdpNotifier();
    }

    public void SetServerNotifier()
    {
        server_notifier.Add((int)ServerPacketId.Match, Match);
    }

    public void SetUdpNotifier()
    {
        p2p_notifier.Add((int)P2PPacketId.ConnectionCheck, ConnectionAnswer);
        p2p_notifier.Add((int)P2PPacketId.CreateUnit, CreateUnit);
        p2p_notifier.Add((int)P2PPacketId.CharacterState, CharacterState);
    }

    public void DataHandle()
    {
        if (receiveMsgs.Count != 0)
        {
            //패킷을 Dequeue 한다 
            //패킷 : 메시지 타입 + 메시지 내용
            DataPacket packet;

            lock (receiveLock)
            {
                packet = receiveMsgs.Dequeue();
            }
            
            msg = packet.msg;

            Debug.Log("Dequeue Message Length : " + msg.Length);

            if (packet.headerData.source == (byte)NetworkManager.Source.ServerSource)
            {
                if (server_notifier.TryGetValue(packet.headerData.id, out serverRecvNotifier))
                {
                    serverRecvNotifier(msg);
                }
                else
                {
                    Debug.Log("DataHandler::Server.TryGetValue 에러 " + packet.headerData.id);
                    packet.headerData.id = (byte)ServerPacketId.None;
                }
            }
            else if (packet.headerData.source == (byte)NetworkManager.Source.ClientSource)
            {
                if (p2p_notifier.TryGetValue(packet.headerData.id, out p2pRecvNotifier))
                {
                    p2pRecvNotifier(msg);
                }
                else
                {
                    Debug.Log("DataHandler::P2P.TryGetValue 에러 " + packet.headerData.id);
                    packet.headerData.id = (byte)P2PPacketId.None;
                }
            }
        }
    }

    public ServerPacketId Match(byte[] data)
    {
        Debug.Log("매치 완료");
        MatchDataPacket matchDataPacket = new MatchDataPacket(data);
        MatchData matchData = matchDataPacket.GetData();

        networkManager.ConnectP2P(matchData.ip);

        return ServerPacketId.None;
    }

    public P2PPacketId ConnectionAnswer(byte[] data)
    {
        Debug.Log("연결 확인 답장");
        dungeonManager = GameObject.FindGameObjectWithTag("DungeonManager").GetComponent<DungeonManager>();
        dungeonManager.CreatePlayer(0);

        return P2PPacketId.ConnectionAnswer;
    }

    public P2PPacketId CreateUnit(byte[] data)
    {
        CreateUnitPacket createUnitPacket = new CreateUnitPacket(data);
        CreateUnitData createUnitData = createUnitPacket.GetData();

        dungeonManager.CreateUnit(createUnitData.ID, new Vector3(createUnitData.PosX, createUnitData.PosY, createUnitData.PosZ));

        return P2PPacketId.None;
    }

    //원래는 보낸 IP를 체크해서 몇번째 플레이어인지 확인 후 그 플레이어의 캐릭터를 조정해야한다.
    //현재는 고정적으로 1번 플레이어를 설정
    public P2PPacketId CharacterState(byte[] data)
    {
        Debug.Log("캐릭터 상태 수신");

        CharacterStatePacket characterStateDataPacket = new CharacterStatePacket(data);
        CharacterStateData characterStateData = characterStateDataPacket.GetData();

        Debug.Log("캐릭터 State : " + characterStateData.state);
        Debug.Log("캐릭터 Hor, Ver : " + characterStateData.hor + ", " + characterStateData.ver);
        Debug.Log("캐릭터 위치 : " + characterStateData.posX + ", " + characterStateData.posY + ", " + characterStateData.posZ + ", ");

        CharacterManager characterManager = dungeonManager.Players[1].GetComponent<CharacterManager>();
        characterManager.SetState(characterStateData);

        return P2PPacketId.None;
    }
}

public class HeaderData
{
    // 헤더 == [2바이트 - 패킷길이][1바이트 - ID]
    public short length; // 패킷의 길이
    public byte source;
    public byte id; // 패킷 ID
}