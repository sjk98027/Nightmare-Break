using System.Collections.Generic;
using System;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    NetworkManager networkManager;
    DungeonManager dungeonManager;

    public Queue<DataPacket> receiveMsgs;

    public delegate P2PPacketId P2PRecvNotifier(byte[] data);
    public delegate ServerPacketId ServerRecvNotifier(byte[] data);
    P2PRecvNotifier p2pRecvNotifier;
    ServerRecvNotifier serverRecvNotifier;
    private Dictionary<int, P2PRecvNotifier> p2p_notifier = new Dictionary<int, P2PRecvNotifier>();
    private Dictionary<int, ServerRecvNotifier> server_notifier = new Dictionary<int, ServerRecvNotifier>();

    public DateTime dTime;

    public void Initialize(Queue<DataPacket> receiveQueue, Queue<DataPacket> sendQueue)
    {
        receiveMsgs = receiveQueue;

        networkManager = GetComponent<NetworkManager>();

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
        p2p_notifier.Add((int)P2PPacketId.CharacterPosition, CharacterPosition);
        p2p_notifier.Add((int)P2PPacketId.CharacterAction, CharacterAction);
    }

    public void DataHandle()
    {
        if (receiveMsgs.Count > 0)
        {
            //패킷을 Dequeue 한다 
            //패킷 : 메시지 타입 + 메시지 내용
            DataPacket packet;

            packet = receiveMsgs.Dequeue();
            
            byte[] msg = packet.msg;

            HeaderData headerData = new HeaderData();
            HeaderSerializer headerSerializer = new HeaderSerializer();
            headerSerializer.SetDeserializedData(msg);
            headerSerializer.Deserialize(ref headerData);

            //Debug.Log("패킷 길이 : " + msg.Length);
            //Debug.Log("패킷 아이디 : " + headerData.id);
            //Debug.Log("패킷 출처 : " + headerData.source);

            DataReceiver.ResizeByteArray(0, NetworkManager.packetSource + NetworkManager.packetId, ref msg);

            if (packet.endPoint == null)
            {
                if (server_notifier.TryGetValue(headerData.id, out serverRecvNotifier))
                {
                    serverRecvNotifier(msg);
                }
                else
                {
                    Debug.Log("DataHandler::Server.TryGetValue 에러 " + headerData.id);
                }
            }
            else if (packet.endPoint != null)
            {
                if (p2p_notifier.TryGetValue(headerData.id, out p2pRecvNotifier))
                {
                    p2pRecvNotifier(msg);
                }
                else
                {
                    Debug.Log("DataHandler::P2P.TryGetValue 에러 " + headerData.id);
                }
            }
        }
    }

    //Server
    public ServerPacketId Match(byte[] data)
    {
        Debug.Log("매치 완료");
        MatchDataPacket matchDataPacket = new MatchDataPacket(data);
        MatchData matchData = matchDataPacket.GetData();

        networkManager.ConnectP2P(matchData.ip);

        return ServerPacketId.None;
    }

    //Client
    public P2PPacketId ConnectionAnswer(byte[] data)
    {
        Debug.Log("연결 확인 답장");
        dungeonManager = GameObject.FindGameObjectWithTag("DungeonManager").GetComponent<DungeonManager>();
        dungeonManager.CreatePlayer(0);
        dTime = DateTime.Now;
        Debug.Log("시간 지정 : " + dTime.ToString("hh:mm:ss"));

        return P2PPacketId.ConnectionAnswer;
    }

    //Client
    public P2PPacketId CreateUnit(byte[] data)
    {
        Debug.Log("유닛 생성");
        CreateUnitPacket createUnitPacket = new CreateUnitPacket(data);
        CreateUnitData createUnitData = createUnitPacket.GetData();

        dungeonManager.CreateUnit(createUnitData.ID, new Vector3(createUnitData.PosX, createUnitData.PosY, createUnitData.PosZ));

        return P2PPacketId.None;
    }

    //Client
    public P2PPacketId CharacterPosition(byte[] data)
    {
        Debug.Log("캐릭터 상태 수신");

        CharacterPositionPacket characterPositionPacket = new CharacterPositionPacket(data);
        CharacterPositionData characterPositionData = characterPositionPacket.GetData();

        Debug.Log("현재시간 : " + (DateTime.Now - dTime).TotalSeconds);
        Debug.Log("시간 : " + characterPositionData.time);
        Debug.Log("캐릭터 방향 : " + characterPositionData.dir);
        Debug.Log("캐릭터 위치 : " + characterPositionData.posX + ", " + characterPositionData.posY + ", " + characterPositionData.posZ + ", ");
        
        //if ((DateTime.Now - dTime).TotalSeconds - characterPositionData.time > 1)
        //{
        //    return P2PPacketId.None;
        //}

        CharacterManager characterManager = dungeonManager.Players[1].GetComponent<CharacterManager>();
        characterManager.SetPosition(characterPositionData);

        return P2PPacketId.None;
    }

    //Client
    public P2PPacketId CharacterAction(byte[] data)
    {
        Debug.Log("캐릭터 행동 수신");

        CharacterActionPacket characterActionPacket = new CharacterActionPacket(data);
        CharacterActionData characterActionData = characterActionPacket.GetData();

        Debug.Log("캐릭터 행동" + characterActionData.action);

        CharacterManager characterManager = dungeonManager.Players[1].GetComponent<CharacterManager>();
        characterManager.CharState(characterActionData.action);

        return P2PPacketId.None;
    }
}