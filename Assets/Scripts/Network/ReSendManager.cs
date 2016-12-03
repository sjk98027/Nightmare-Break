using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSendManager : MonoBehaviour
{
    public class ReSend
    {
        SendData sendData;
        ReSendCallBack reSendCallBack;
        
        public SendData SendData { get { return sendData; } }
        public ReSendCallBack ReSendCallBack { get { return reSendCallBack; } }

        public ReSend()
        {
            sendData = null;
            reSendCallBack = null;
        }

        public ReSend(SendData newSendData, ReSendCallBack newReSendCallBack)
        {
            sendData = newSendData;
            reSendCallBack = newReSendCallBack;
        }
    }

    NetworkManager networkManager;
    
    public delegate void ReSendCallBack(EndPoint endPoint, SendData sendData);
    ReSend reSendData;
    private Dictionary<int, ReSend>[] reSendDatum;
    List<int> reSendKey;
    List<ReSend> reSendValue;

    bool isConnecting;

    public void Initialize(int userNum)
    {
        networkManager = GetComponent<NetworkManager>();
        reSendDatum = new Dictionary<int, ReSend>[userNum - 1];

        for (int i = 0; i < userNum - 1; i++)
        {
            reSendDatum[i] = new Dictionary<int, ReSend>();
        }

        isConnecting = true;
    }

    public void AddReSendData(SendData sendData, ReSendCallBack reSendData)
    {
        ReSend resend = new ReSend(sendData, reSendData);
        int index = networkManager.DataHandler.userNum[sendData.EndPoint];

        try
        {
            Debug.Log(index + "번 유저에 " + sendData.UdpId + " 아이디 메소드 추가");
            reSendDatum[index].Add(sendData.UdpId, resend);
            Debug.Log("메소드 개수 : " + reSendDatum[index].Count);
        }
        catch
        {
            Debug.Log("ReSendManager::AddReSendData.Add 에러");
        }
    }

    public void RemoveReSendData(SendData sendData)
    {
        int index = networkManager.DataHandler.userNum[sendData.EndPoint];

        try
        {
            Debug.Log(index + "번 유저에 " + sendData.UdpId + " 아이디 메소드 삭제");
            reSendDatum[index].Remove(sendData.UdpId);
            Debug.Log("메소드 개수 : " + reSendDatum[index].Count);
        }
        catch
        {
            Debug.Log("ReSendManager::AddReSendData.Remove 에러");
        }
    }

    public IEnumerator StartCheckSendData()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            //모든 플레이어들의 ReSend Dictionary를 확인한다 //Error
            for (int i = 0; i < reSendDatum.Length; i++)
            {
                reSendKey = new List<int>(reSendDatum[i].Keys);

                //i번 플레이어의 ReSendData를 확인한다
                foreach (int key in reSendKey)
                {
                    //i번 플레이어의 foreach문에 걸린 method를 하나 실행한다.
                    if (reSendDatum[i].TryGetValue(key, out reSendData))
                    {
                        reSendData.ReSendCallBack((reSendDatum[i])[key].SendData.EndPoint, (reSendDatum[i])[key].SendData);
                    }
                }
            }

            if (isConnecting)
            {
                for (int i = 0; i < reSendDatum.Length; i++)
                {
                    Debug.Log(reSendDatum[i].Count);
                    if (reSendDatum[i].Count != 0)
                    {
                        isConnecting = true;
                        break;
                    }
                    else
                    {
                        isConnecting = false;
                    }

                    Debug.Log(i);
                    Debug.Log(isConnecting);
                }

                if (!isConnecting)
                {
                    DataSender.Instance.UdpConnectComplete();
                }                
            }
        }
    }
}

public class SendData
{
    EndPoint endPoint;
    int udpId;
    int characterId;
    float posX;
    float posY;
    float posZ;

    public EndPoint EndPoint { get { return endPoint; } }
    public int UdpId { get { return udpId; } }
    public int CharacterId { get { return characterId; } }
    public float PosX { get { return posX; } }
    public float PosY { get { return posY; } }
    public float PosZ { get { return posZ; } }

    public SendData()
    {
        endPoint = null;
        udpId = 0;
        characterId = 0;
        posX = 0;
        posY = 0;
        posZ = 0;
    }

    public SendData(EndPoint newEndPoint, int newUdpId)
    {
        endPoint = newEndPoint;
        udpId = newUdpId;
    }

    public SendData(EndPoint newEndPoint, int newUdpId, int newCharacterId, float newPosX, float newPosY, float newPosZ)
    {
        endPoint = newEndPoint;
        udpId = newUdpId;
        characterId = newCharacterId;
        posX = newPosX;
        posY = newPosY;
        posZ = newPosZ;
    }
}