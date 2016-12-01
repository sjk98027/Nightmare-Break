using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public enum Result
{
    Success = 0,
    Fail,
}

public class DataHandler : MonoBehaviour
{
    GameManager gameManager;
    NetworkManager networkManager;
    DungeonManager dungeonManager;
    UIManager uiManager;
    CharacterStatus characterStatus;

    public Queue<DataPacket> receiveMsgs;

    public delegate void P2PRecvNotifier(DataPacket packet);
    public delegate void ServerRecvNotifier(DataPacket packet);
    P2PRecvNotifier p2pRecvNotifier;
    ServerRecvNotifier serverRecvNotifier;
    private Dictionary<int, P2PRecvNotifier> p2p_notifier = new Dictionary<int, P2PRecvNotifier>();
    private Dictionary<int, ServerRecvNotifier> server_notifier = new Dictionary<int, ServerRecvNotifier>();

    Dictionary<EndPoint, bool> connectionCheck;
    public DateTime dTime;

    public void Initialize(Queue<DataPacket> receiveQueue, Queue<DataPacket> sendQueue)
    {
        receiveMsgs = receiveQueue;

        networkManager = GetComponent<NetworkManager>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        SetServerNotifier();
        SetUdpNotifier();

        StartCoroutine(DataHandle());
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
        server_notifier.Add((int)ServerPacketId.EnterRoomResult, EnterRoomResult);
        server_notifier.Add((int)ServerPacketId.ExitRoomResult, ExitRoomResult);
        server_notifier.Add((int)ServerPacketId.StartGame, StartGame);
        server_notifier.Add((int)ServerPacketId.UDPConnection, UDPConnection);
        server_notifier.Add((int)ServerPacketId.StartDungeon, StartDungeon);
    }

    public void SetUdpNotifier()
    {
        p2p_notifier.Add((int)P2PPacketId.RequestConnectionCheck, ConnectionCheckAnswer);
        p2p_notifier.Add((int)P2PPacketId.ConnectionCheckAnswer, ConnectionCheck);
        p2p_notifier.Add((int)P2PPacketId.CreateUnit, CreateUnit);
        p2p_notifier.Add((int)P2PPacketId.CharacterPosition, CharacterPosition);
        p2p_notifier.Add((int)P2PPacketId.CharacterAction, CharacterAction);
    }

    public IEnumerator DataHandle()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (receiveMsgs.Count > 0)
            {
                //패킷을 Dequeue 한다 
                //패킷 : 메시지 타입 + 메시지 내용
                DataPacket packet;

                packet = receiveMsgs.Dequeue();

                HeaderData headerData = new HeaderData();
                HeaderSerializer headerSerializer = new HeaderSerializer();
                headerSerializer.SetDeserializedData(packet.msg);
                headerSerializer.Deserialize(ref headerData);

                //Debug.Log("패킷 길이 : " + msg.Length);
                //Debug.Log("패킷 아이디 : " + headerData.id);
                //Debug.Log("패킷 출처 : " + headerData.source);

                DataReceiver.ResizeByteArray(0, NetworkManager.packetSource + NetworkManager.packetId, ref packet.msg);

                if (packet.endPoint == null)
                {
                    if (server_notifier.TryGetValue(headerData.id, out serverRecvNotifier))
                    {
                        serverRecvNotifier(packet);
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
                        p2pRecvNotifier(packet);
                    }
                    else
                    {
                        Debug.Log("DataHandler::P2P.TryGetValue 에러 " + headerData.id);
                    }
                }
            }
        }        
    }
    
    //Server - 가입 결과
    public void CreateAccountResult(DataPacket packet)
    {
        Debug.Log("가입 결과 수신");
        ResultPacket resultPacket = new ResultPacket(packet.msg);
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
    public void DeleteAccountResult(DataPacket packet)
    {
        Debug.Log("탈퇴 결과 수신");
        ResultPacket resultPacket = new ResultPacket(packet.msg);
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
    public void LoginResult(DataPacket packet)
    {
        Debug.Log("로그인 결과 수신");

        ResultPacket resultPacket = new ResultPacket(packet.msg);
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
    public void CreateCharacterResult(DataPacket packet)
    {
        Debug.Log("캐릭터 생성 결과 수신");
        
        ResultPacket resultPacket = new ResultPacket(packet.msg);
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
    public void DeleteCharacterResult(DataPacket packet)
    {
        Debug.Log("캐릭터 삭제 결과 수신");

        ResultPacket resultPacket = new ResultPacket(packet.msg);
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
    public void SelectCharacterResult(DataPacket packet)
    {
        Debug.Log("캐릭터 선택 결과 수신");

        ResultPacket resultPacket = new ResultPacket(packet.msg);
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
    public void RoomList(DataPacket packet)
    {
        Debug.Log("방 목록 수신");
        RoomListPacket roomListPacket = new RoomListPacket(packet.msg);
        RoomListData roomListData = roomListPacket.GetData();

        uiManager.WaitUIManager.SetRoom(roomListData);
    }

    //Server - 캐릭터 정보 수신
    public void CharacterData(DataPacket packet)
    {
        Debug.Log("캐릭터 정보 수신");
        CharacterStatusPacket characterStatusPacket = new CharacterStatusPacket(packet.msg);
        CharacterStatusData characterStatusData = characterStatusPacket.GetData();

        characterStatus.SetCharacterStatus(characterStatusData);
    }

    //Server - 방 생성 결과 수신
    public void CreateRoomResult(DataPacket packet)
    {
        Debug.Log("방 생성 결과 수신");
        RoomResultPacket resultPacket = new RoomResultPacket(packet.msg);
        RoomResultData resultData = resultPacket.GetData();

        if (resultData.RoomNum < 0)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "방 생성 실패"));
        }
        else if (resultData.RoomNum <= WaitUIManager.maxRoomNum)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "방 생성 성공"));
            uiManager.WaitUIManager.CreateRoom(resultData.RoomNum - 1);
        }
    }

    //Server - 방 입장 결과 수신
    public void EnterRoomResult(DataPacket packet)
    {
        Debug.Log("방 입장 결과 수신");
        RoomResultPacket resultPacket = new RoomResultPacket(packet.msg);
        RoomResultData resultData = resultPacket.GetData();

        if (resultData.RoomNum < 0)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "방 입장 실패"));
        }
        else if (resultData.RoomNum <= WaitUIManager.maxPlayerNum)
        {
            StartCoroutine(uiManager.Dialog(1.0f, "방 입장 성공"));
            uiManager.WaitUIManager.SetUserNum(resultData.RoomNum);
        }
    }

    //Server - 방 퇴장 결과 수신
    public void ExitRoomResult(DataPacket packet)
    {

    }

    //Server - 게임 시작
    public void StartGame(DataPacket packet)
    {
        Debug.Log("게임 시작");

        ResultPacket resultPacket = new ResultPacket(packet.msg);
        ResultData resultData = resultPacket.GetData();

        if (resultData.Result == (byte)Result.Success)
        {
            Debug.Log("게임 시작");
            DataSender.Instance.RequestUDPConnection();
        }
        else if (resultData.Result == (byte)Result.Fail)
        {
            Debug.Log("게임 시작 실패");
        }
    }

    //Server - 연결 시작
    public void UDPConnection(DataPacket packet)
    {
        Debug.Log("연결 시작");
        MatchDataPacket matchDataPacket = new MatchDataPacket(packet.msg);
        MatchData matchData = matchDataPacket.GetData();

        connectionCheck = new Dictionary<EndPoint, bool>();

        for (int i = 0; i < matchData.ip.Length; i++)
        {
            string ip = matchData.ip[i];
            ip = ip.Substring(0, ip.IndexOf(":"));

            Debug.Log("온 아이피 : " + ip);
            Debug.Log("내 아이피 " + networkManager.client.ToString().Substring(0, networkManager.client.ToString().IndexOf(":")));

            if (ip != networkManager.client.ToString().Substring(0, networkManager.client.ToString().IndexOf(":")))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), NetworkManager.clientPortNumber);
                connectionCheck.Add((EndPoint)endPoint, false);
                networkManager.ConnectP2P(ip);
            }
        }

        StartCoroutine(ConnectionCheck());
    }

    //Client - 연결 확인 답장
    public void ConnectionCheckAnswer(DataPacket packet)
    {
        Debug.Log("연결 확인 답장");

        DataSender.Instance.ConnectionCheckAnswer(packet.endPoint);
    }

    //Client - 연결 확인
    public void ConnectionCheck(DataPacket packet)
    {
        Debug.Log("연결 확인");

        string ip = packet.endPoint.ToString();
        ip = ip.Substring(0, ip.IndexOf(":"));

        Debug.Log(ip);

        try
        {
            connectionCheck[packet.endPoint] = true;
        }
        catch
        {
            Debug.Log("DataHandler.ConnectionCheck::KeyValue 에러");
        }
    }

    //Server - 던전 시작
    public void StartDungeon(DataPacket packet)
    {
        Debug.Log("던전 시작");

        gameManager.SetManagerInDungeon();
        dungeonManager = GameObject.FindGameObjectWithTag("DungeonManager").GetComponent<DungeonManager>();
        dungeonManager.CreatePlayer(0);
        dTime = DateTime.Now;
        Debug.Log("시간 지정 : " + dTime.ToString("hh:mm:ss"));
    }

    //Client
    public void CreateUnit(DataPacket packet)
    {
        Debug.Log("유닛 생성");
        CreateUnitPacket createUnitPacket = new CreateUnitPacket(packet.msg);
        CreateUnitData createUnitData = createUnitPacket.GetData();

        dungeonManager.CreateUnit(createUnitData.ID, new Vector3(createUnitData.PosX, createUnitData.PosY, createUnitData.PosZ));
    }

    //Client
    public void CharacterPosition(DataPacket packet)
    {
        Debug.Log("캐릭터 위치 수신");

        CharacterPositionPacket characterPositionPacket = new CharacterPositionPacket(packet.msg);
        CharacterPositionData characterPositionData = characterPositionPacket.GetData();

        Debug.Log("현재시간 : " + (DateTime.Now - dTime).TotalSeconds);
        Debug.Log("시간 : " + characterPositionData.time);
        Debug.Log("캐릭터 방향 : " + characterPositionData.dir);
        Debug.Log("캐릭터 위치 : " + characterPositionData.posX + ", " + characterPositionData.posY + ", " + characterPositionData.posZ + ", ");

        //if ((DateTime.Now - dTime).TotalSeconds - characterPositionData.time > 1)
        //{
        //    return;
        //}

        CharacterManager characterManager = dungeonManager.Players[1].GetComponent<CharacterManager>();
        characterManager.SetPosition(characterPositionData);
    }

    //Client
    public void CharacterAction(DataPacket packet)
    {
        Debug.Log("캐릭터 행동 수신");

        CharacterActionPacket characterActionPacket = new CharacterActionPacket(packet.msg);
        CharacterActionData characterActionData = characterActionPacket.GetData();

        Debug.Log("캐릭터 행동" + characterActionData.action);

        CharacterManager characterManager = dungeonManager.Players[1].GetComponent<CharacterManager>();
        characterManager.CharState(characterActionData.action);
    }

    public IEnumerator ConnectionCheck()
    {
        bool check = false;

        while (!check)
        {
            yield return new WaitForSeconds(1.0f);

            foreach (KeyValuePair<EndPoint, bool> coCheck in connectionCheck)
            {
                Debug.Log(coCheck.Key);
                Debug.Log(coCheck.Value);
                if (!coCheck.Value)
                {
                    DataSender.Instance.RequestConnectionCheck(coCheck.Key);
                }
            }

            foreach(KeyValuePair<EndPoint, bool> coCheck in connectionCheck)
            {
                if (!coCheck.Value)
                {
                    check = false;
                    break;
                }
                else
                {
                    check = true;
                }
            }

            if (check)
            {
                StopCoroutine(ConnectionCheck());
                DataSender.Instance.UDPConnectComplete();
            }            
        }
    }
}