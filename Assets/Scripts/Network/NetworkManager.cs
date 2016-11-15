using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
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
    public const int udpId = 2;

    //패킷이 어디서 오는지 - 서버/클라이언트
    public enum Source
    {
        ServerSource = 0,
        ClientSource = 1,
    }

    //테스트 중에서는 하나의 컴퓨터에서 진행하므로 다른 ip 대신에 다른 port를 이용한다
    public const int mainServerPortNumber = 8800;
    public const int client1PortNumber = 9003;
    public const int client2PortNumber = 9001;
    public IPEndPoint mainServer;
    public IPEndPoint client1;
    public IPEndPoint client2;

    //udp Socket이 연결할 SocketList
    List<EndPoint> clients;

    Queue<DataPacket> receiveMsgs;
    Queue<DataPacket> sendMsgs;

    Socket clientSock;
    Socket serverSock;

    DataReceiver dataReceiver;
    DataHandler dataHandler;
    DataSender dataSender;

    public List<EndPoint> Clients { get { return clients; } }
    public DataReceiver DataReceiver { get { return dataReceiver; } }
    public DataHandler DataHandler { get { return dataHandler; } }
    public DataSender DataSender { get { return dataSender; } }

    public void InitializeManager(string ip)
    {
        mainServer = new IPEndPoint(IPAddress.Parse(ip), mainServerPortNumber);
        client1 = new IPEndPoint(IPAddress.Parse(ip), client1PortNumber);
        client2 = new IPEndPoint(IPAddress.Parse("192.168.94.67"), client2PortNumber);

        receiveMsgs = new Queue<DataPacket>();
        sendMsgs = new Queue<DataPacket>();

        serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        clientSock.Bind(client1);

        ConnectServer();

        dataReceiver = GetComponent<DataReceiver>();
        dataHandler = GetComponent<DataHandler>();
        dataSender = GetComponent<DataSender>();

        dataReceiver.Initialize(receiveMsgs, serverSock);
        dataHandler.Initialize(receiveMsgs, sendMsgs);
        dataSender.Initialize(sendMsgs, serverSock, clientSock);
    }

    public void ConnectServer()
    {
        try
        {
            serverSock.Connect(mainServer);
            Debug.Log("서버 연결 성공");
        }
        catch (Exception e)
        {
            Debug.Log("서버 연결 실패" + e.Message);
        }
    }

    public void ConnectP2P(string[] newIp)
    {
        //아이피 목록 생성
        clients = new List<EndPoint>();

        for (int i = 0; i < newIp.Length; i++)
        {
            Debug.Log(newIp[i]);
            Debug.Log(serverSock.LocalEndPoint.ToString());

            if (newIp[i] != serverSock.LocalEndPoint.ToString())
            {
                string n_ip = newIp[i].Substring(0, newIp[i].IndexOf(":"));

                Debug.Log(n_ip + "추가");

                clients.Add(new IPEndPoint(IPAddress.Parse(n_ip), client2PortNumber));
            }
        }

        dataReceiver.StartUdpReceive(clientSock, clients);
        dataSender.ConnectionCheck(clients);
    }

    public void SocketClose()
    {
        Debug.Log("소켓 닫기");
        clientSock.Close();
        serverSock.Close();
    }
}
