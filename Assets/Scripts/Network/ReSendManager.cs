using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSendManager : MonoBehaviour
{
    NetworkManager networkManager;
    
    private Dictionary<int, SendData>[] reSendDatum;
    SendData reSendData;

    List<int> reSendKey;

    public bool isConnecting;
    public bool characterCreating;

    public void Initialize(int userNum)
    {
        networkManager = GetComponent<NetworkManager>();
        reSendDatum = new Dictionary<int, SendData>[userNum - 1];

        for (int i = 0; i < userNum - 1; i++)
        {
            reSendDatum[i] = new Dictionary<int, SendData>();
        }
    }

    public void AddReSendData(SendData sendData)
    {
        int index = networkManager.DataHandler.GetUserNum(sendData.EndPoint);

        try
        {
            Debug.Log(index + "번 유저에 " + sendData.UdpId + " 아이디 메소드 추가");
            reSendDatum[index].Add(sendData.UdpId, sendData);
        }
        catch
        {
            Debug.Log("ReSendManager::AddReSendData.Add 에러");
        }
    }

    public void RemoveReSendData(SendData sendData)
    {
        int index = networkManager.DataHandler.GetUserNum(sendData.EndPoint);

        if (reSendDatum[index].ContainsKey(sendData.UdpId))
        {
            try
            {
                Debug.Log(index + "번 유저에 " + sendData.UdpId + " 아이디 메소드 삭제");
                reSendDatum[index].Remove(sendData.UdpId);
            }
            catch
            {
                Debug.Log("ReSendManager::AddReSendData.Remove 에러");
            }
        }
    }

    public void DataReSend(SendData sendData)
    {
        DataPacket packet = new DataPacket(sendData.Msg, sendData.EndPoint);
        DataSender.Instance.SendMsgs.Enqueue(packet);
    }

    public IEnumerator StartCheckSendData()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            //모든 플레이어들의 ReSendData를 확인한다
            for (int i = 0; i < reSendDatum.Length; i++)
            {
                reSendKey = new List<int>(reSendDatum[i].Keys);

                //i번 플레이어의 ReSendData를 확인한다
                foreach (int key in reSendKey)
                {
                    //i번 플레이어의 foreach문에 걸린 method를 하나 실행한다.
                    if (reSendDatum[i].TryGetValue(key, out reSendData))
                    {
                        DataReSend(reSendData);
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

            if (characterCreating)
            {
                for (int i = 0; i < reSendDatum.Length; i++)
                {
                    Debug.Log(reSendDatum[i].Count);
                    if (reSendDatum[i].Count != 0)
                    {
                        characterCreating = true;
                        break;
                    }
                    else
                    {
                        characterCreating = false;
                    }

                    Debug.Log(i);
                    Debug.Log(characterCreating);
                }

                if (!characterCreating)
                {
                    StartCoroutine(DataSender.Instance.CharacterPositionSend());
                }
            }
        }
    }
}

public class SendData
{
    int udpId;
    EndPoint endPoint;
    byte[] msg;

    public int UdpId { get { return udpId; } }
    public EndPoint EndPoint { get { return endPoint; } }
    public byte[] Msg { get { return msg; } }

    public SendData()
    {
        udpId = 0;
        endPoint = null;
        msg = new byte[0];
    }

    public SendData(int newUdpId, EndPoint newEndPoint)
    {
        udpId = newUdpId;
        endPoint = newEndPoint;
        msg = new byte[0];
    }

    public SendData(int newUdpId, EndPoint newEndPoint, byte[] newMsg)
    {
        udpId = newUdpId;
        endPoint = newEndPoint;
        msg = newMsg;
    }
}