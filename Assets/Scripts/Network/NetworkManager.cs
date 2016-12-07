using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    //패킷의 길이
    //패킷이 어디서 오는지
    //패킷의 종류
    //Udp receive 확인 번호
    public const int packetLength = 2;
    public const int packetSource = 1;
    public const int packetId = 1;
    public const int udpId = 4;

    //패킷이 어디서 오는지 - 서버/클라이언트
    public enum Source
    {
        ServerSource = 0,
        ClientSource = 1,
    }

    [SerializeField]
    string myIP;

    public const string serverIP = "192.168.94.88";
    public const int serverPortNumber = 8800;
    public const int clientPortNumber = 9000;
    public IPEndPoint serverEndPoint;
    public IPEndPoint clientEndPoint;
    
    Queue<DataPacket> receiveMsgs;
    Queue<DataPacket> sendMsgs;

    object receiveLock;

    Socket clientSock;
    Socket serverSock;

    DataReceiver dataReceiver;
    DataHandler dataHandler;
    DataSender dataSender;
    ReSendManager reSendManager;

    Dictionary<EndPoint, int> userIndex;
    int myIndex;

    public string MyIP
    {
        get
        {
            return myIP;
        }
    }
    public int MyIndex { get { return myIndex; } }
    public Socket ClientSock { get { return clientSock; } }
    public Dictionary<EndPoint, int> UserIndex { get { return userIndex; } }
    public DataReceiver DataReceiver { get { return dataReceiver; } }
    public DataHandler DataHandler { get { return dataHandler; } }
    public DataSender DataSender { get { return dataSender; } }
    public ReSendManager ReSendManager { get { return reSendManager; } }

    public void InitializeManager()
    {
        receiveMsgs = new Queue<DataPacket>();
        sendMsgs = new Queue<DataPacket>();
        receiveLock = new object();

        InitializeTcpConnection();

        dataReceiver = GetComponent<DataReceiver>();
        dataHandler = GetComponent<DataHandler>();
        dataSender = GetComponent<DataSender>();

        dataReceiver.Initialize(receiveMsgs, serverSock, receiveLock);
        dataHandler.Initialize(receiveMsgs, sendMsgs, receiveLock);
        dataSender.Initialize(sendMsgs, serverSock, clientSock);
    }

    public void InitializeTcpConnection()
    {
        serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPortNumber);
        serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        ConnectServer();
    }

    public void InitializeUdpConnection(string clientIP)
    {
        clientEndPoint = new IPEndPoint(IPAddress.Parse(clientIP), clientPortNumber + myIndex);
        clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        clientSock.Bind(clientEndPoint);

        userIndex = new Dictionary<EndPoint, int>();
        reSendManager = GetComponent<ReSendManager>();

        DataReceiver.SetUdpSocket(clientSock);
    }

    public void ConnectServer()
    {
        try
        {
            serverSock.Connect(serverEndPoint);
            Debug.Log("서버 연결 성공");
        }
        catch (Exception e)
        {
            Debug.Log("서버 연결 실패" + e.Message);
        }
    }

    public void ConnectP2P(string newIp)
    {
        IPEndPoint newClient = new IPEndPoint(IPAddress.Parse(newIp), clientPortNumber);
        dataReceiver.StartUdpReceive(newClient);
        int index = userIndex[(EndPoint)newClient];
        dataSender.RequestConnectionCheck((EndPoint)newClient);
    }

    public void DisconnectP2P()
    {

    }

    public void SocketClose()
    {
        Debug.Log("소켓 닫기");
        clientSock.Close();
        serverSock.Close();
    }

    public int GetUserIndex(EndPoint endPoint)
    {
        return userIndex[endPoint];
    }

    public void SetMyIndex(int newIndex)
    {
        myIndex = newIndex;
    }
}
