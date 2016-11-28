using System.Collections.Generic;
using System;
using UnityEngine;

public enum Result
{
    Success = 0,
    Fail,
}

public class DataHandler : MonoBehaviour
{
    NetworkManager networkManager;
    DungeonManager dungeonManager;
    UIManager uiManager;
    CharacterStatus characterStatus;

    public Queue<DataPacket> receiveMsgs;

    public delegate void P2PRecvNotifier(byte[] data);
    public delegate void ServerRecvNotifier(byte[] data);
    P2PRecvNotifier p2pRecvNotifier;
    ServerRecvNotifier serverRecvNotifier;
    private Dictionary<int, P2PRecvNotifier> p2p_notifier = new Dictionary<int, P2PRecvNotifier>();
    private Dictionary<int, ServerRecvNotifier> server_notifier = new Dictionary<int, ServerRecvNotifier>();

    public DateTime dTime;

    public void Initialize(Queue<DataPacket> receiveQueue, Queue<DataPacket> sendQueue)
    {
        receiveMsgs = receiveQueue;

        networkManager = GetComponent<NetworkManager>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        SetServerNotifier();
        SetUdpNotifier();
    }

    public void SetCharacter(GameObject character)
    {
        characterStatus = character.GetComponent<CharacterStatus>();
    }

    public void SetServerNotifier()
    {
        server_notifier.Add((int)ServerPacketId.CreateAccountResult, CreateAccountResult);
        server_notifier.Add((int)ServerPacketId.DeleteAccountResult, DeleteAccountResult);
        server_notifier.Add((int)ServerPacketId.LoginResult, LoginResult);
        server_notifier.Add((int)ServerPacketId.CreateCharacterResult, CreateCharacterResult);
        server_notifier.Add((int)ServerPacketId.DeleteChracterResult, DeleteCharacterResult);
        server_notifier.Add((int)ServerPacketId.SelectCharacterResult, SelectCharacterResult);
        server_notifier.Add((int)ServerPacketId.RoomList, RoomList);
        server_notifier.Add((int)ServerPacketId.CreateRoomResult, CreateRoomResult);
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
    
    //Server - 가입 결과
    public void CreateAccountResult(byte[] data)
    {
        Debug.Log("가입 결과 수신");
        ResultPacket resultPacket = new ResultPacket(data);
        ResultData resultData = resultPacket.GetData();
        
        if(resultData.Result == (byte)Result.Success)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "가입 성공"));
        }
        else if (resultData.Result == (byte)Result.Fail)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "가입 실패"));
        }
    }

    //Server - 탈퇴 결과
    public void DeleteAccountResult(byte[] data)
    {
        Debug.Log("탈퇴 결과 수신");
        ResultPacket resultPacket = new ResultPacket(data);
        ResultData resultData = resultPacket.GetData();

        if (resultData.Result == (byte)Result.Success)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "탈퇴 성공"));
        }
        else if (resultData.Result == (byte)Result.Fail)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "탈퇴 실패"));
        }
    }

    //Server - 로그인 결과
    public void LoginResult(byte[] data)
    {
        Debug.Log("로그인 결과 수신");

        ResultPacket resultPacket = new ResultPacket(data);
        ResultData resultData = resultPacket.GetData();

        if (resultData.Result == (byte)Result.Success)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "로그인 성공"));
        }
        else if (resultData.Result == (byte)Result.Fail)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "로그인 실패"));
        }
    }

    //Server - 캐릭터 생성 결과
    public void CreateCharacterResult(byte[] data)
    {
        Debug.Log("캐릭터 생성 결과 수신");
        
        ResultPacket resultPacket = new ResultPacket(data);
        ResultData resultData = resultPacket.GetData();

        if (resultData.Result == (byte)Result.Success)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "캐릭터 생성 성공"));
        }
        else if (resultData.Result == (byte)Result.Fail)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "캐릭터 생성 실패"));
        }
    }

    //Server - 캐릭터 삭제 결과
    public void DeleteCharacterResult(byte[] data)
    {
        Debug.Log("캐릭터 삭제 결과 수신");

        ResultPacket resultPacket = new ResultPacket(data);
        ResultData resultData = resultPacket.GetData();

        if (resultData.Result == (byte)Result.Success)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "캐릭터 삭제 성공"));
        }
        else if (resultData.Result == (byte)Result.Fail)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "캐릭터 삭제 실패"));
        }
    }

    //Server - 캐릭터 선택 결과
    public void SelectCharacterResult(byte[] data)
    {
        Debug.Log("캐릭터 선택 결과 수신");

        ResultPacket resultPacket = new ResultPacket(data);
        ResultData resultData = resultPacket.GetData();

        if (resultData.Result == (byte)Result.Success)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "캐릭터 선택 성공"));
        }
        else if (resultData.Result == (byte)Result.Fail)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "캐릭터 선택 실패"));
        }
    }

    //Server - 방 목록 수신
    public void RoomList(byte[] data)
    {
        Debug.Log("방 목록 수신");
        RoomListPacket roomListPacket = new RoomListPacket(data);
        RoomListData roomListData = roomListPacket.GetData();

        uiManager.WaitUIManager.SetRoom(roomListData);
    }

    //Server - 캐릭터 정보 수신
    public void CharacterData(byte[] data)
    {
        Debug.Log("캐릭터 정보 수신");
        CharacterStatusPacket characterStatusPacket = new CharacterStatusPacket(data);
        CharacterStatusData characterStatusData = characterStatusPacket.GetData();

        characterStatus.SetCharacterStatus(characterStatusData);
    }

    //Server - 방 생성 결과 수신
    public void CreateRoomResult(byte[] data)
    {
        Debug.Log("방 생성 결과 수신");
        CreateRoomResultPacket resultPacket = new CreateRoomResultPacket(data);
        CreateRoomResultData resultData = resultPacket.GetData();

        if (resultData.RoomNum == 0)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "방 생성 실패"));
        }
        else if (resultData.RoomNum <= WaitUIManager.maxRoomNum)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "방 생성 성공"));
            uiManager.WaitUIManager.CreateRoom(resultData.RoomNum);
        }
    }

    //Server - 방 입장 결과 수신
    public void EnterRoomResult(byte[] data)
    {
        Debug.Log("방 입장 결과 수신");
        ResultPacket resultPacket = new ResultPacket(data);
        ResultData resultData = resultPacket.GetData();

        if (resultData.Result == (byte)Result.Success)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "방 입장 성공"));
        }
        else if (resultData.Result == (byte)Result.Fail)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "방 입장 실패"));
        }
    }

    //Server
    public void Match(byte[] data)
    {
        Debug.Log("매치 완료");
        MatchDataPacket matchDataPacket = new MatchDataPacket(data);
        MatchData matchData = matchDataPacket.GetData();

        networkManager.ConnectP2P(matchData.ip);
    }

    //Client
    public void ConnectionAnswer(byte[] data)
    {
        Debug.Log("연결 확인 답장");
        dungeonManager = GameObject.FindGameObjectWithTag("DungeonManager").GetComponent<DungeonManager>();
        dungeonManager.CreatePlayer(0);
        dTime = DateTime.Now;
        Debug.Log("시간 지정 : " + dTime.ToString("hh:mm:ss"));
    }

    //Client
    public void CreateUnit(byte[] data)
    {
        Debug.Log("유닛 생성");
        CreateUnitPacket createUnitPacket = new CreateUnitPacket(data);
        CreateUnitData createUnitData = createUnitPacket.GetData();

        dungeonManager.CreateUnit(createUnitData.ID, new Vector3(createUnitData.PosX, createUnitData.PosY, createUnitData.PosZ));
    }

    //Client
    public void CharacterPosition(byte[] data)
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
    }

    //Client
    public void CharacterAction(byte[] data)
    {
        Debug.Log("캐릭터 행동 수신");

        CharacterActionPacket characterActionPacket = new CharacterActionPacket(data);
        CharacterActionData characterActionData = characterActionPacket.GetData();

        Debug.Log("캐릭터 행동" + characterActionData.action);

        CharacterManager characterManager = dungeonManager.Players[1].GetComponent<CharacterManager>();
        characterManager.CharState(characterActionData.action);
    }
}