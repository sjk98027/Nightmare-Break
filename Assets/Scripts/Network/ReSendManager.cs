using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSendManager : MonoBehaviour
{
    public class ReSend
    {
        EndPoint endPoint;
        ReSendData reSendData;

        public EndPoint EndPoint { get { return endPoint; } }
        public ReSendData ReSendData { get { return reSendData; } }

        public ReSend()
        {
            endPoint = null;
            reSendData = null;
        }

        public ReSend(EndPoint newEndPoint, ReSendData newReSendData)
        {
            endPoint = newEndPoint;
            reSendData = newReSendData;
        }
    }

    NetworkManager networkManager;
    public EndPoint[] endPoints;

    public delegate void ReSendData(EndPoint endPoint);
    ReSend reSendData;
    private Dictionary<int, ReSend>[] reSendDatum;

    bool isConnecting;

    public void Initialize(int userNum)
    {
        networkManager = GetComponent<NetworkManager>();
        reSendDatum = new Dictionary<int, ReSend>[userNum - 1];
        endPoints = new EndPoint[userNum - 1];
        isConnecting = true;
    }

    public void AddReSendData(int id, EndPoint endPoint, ReSendData reSendData)
    {
        ReSend resend = new ReSend(endPoint, reSendData);
        int index = networkManager.DataHandler.userNum[endPoint];

        try
        {
            reSendDatum[index].Add(id, resend);
        }
        catch
        {
            Debug.Log("ReSendManager::AddReSendData.Add 에러");
        }
    }

    public void RemoveReSendData(int id, EndPoint endPoint)
    {
        int index = networkManager.DataHandler.userNum[endPoint];

        try
        {
            reSendDatum[index].Remove(id);
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

            //모든 플레이어들의 ReSend Dictionary를 확인한다
            for (int i = 0; i < WaitUIManager.maxPlayerNum - 1; i++)
            {

                //i번 플레이어의 ReSendData를 확인한다
                foreach (KeyValuePair<int, ReSend> sendData in reSendDatum[i])
                {
                    //i번 플레이어의 foreach문에 걸린 method를 하나 실행한다.
                    if (reSendDatum[i].TryGetValue(sendData.Key, out reSendData))
                    {
                        reSendData.ReSendData((sendData.Value).EndPoint);
                    }
                }
            }

            if (isConnecting)
            {
                for (int i = 0; i < WaitUIManager.maxPlayerNum - 1; i++)
                {
                    if (reSendDatum[i].Count != 0)
                    {
                        isConnecting = true;
                        break;
                    }
                    else
                    {
                        isConnecting = false;
                    }
                }
            }

            if (isConnecting)
            {
                DataSender.Instance.UDPConnectComplete();
            }
        }
    }
}