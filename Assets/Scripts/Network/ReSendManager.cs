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

    public delegate void ReSendData(EndPoint endPoint, int id);
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

    public void AddReSendData(int id, EndPoint endPoint, ReSendData reSendData)
    {
        ReSend resend = new ReSend(endPoint, reSendData);
        int index = networkManager.DataHandler.userNum[endPoint];

        try
        {
            Debug.Log(index + "번 유저에 " + id + " 아이디 메소드 추가");
            reSendDatum[index].Add(id, resend);
            Debug.Log("메소드 개수 : " + reSendDatum[index].Count);
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
            Debug.Log(index + "번 유저에 " + id + " 아이디 메소드 삭제");
            reSendDatum[index].Remove(id);
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
                        reSendData.ReSendData((reSendDatum[i])[key].EndPoint, key);
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