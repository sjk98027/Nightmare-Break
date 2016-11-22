using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
    DataSender dataSender;

    Text id;
    Text pw;

    public void ManagerInitialize()
    {
        dataSender = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<DataSender>();

        SetUIObject();
    }

    public void SetUIObject()
    {
        id = GameObject.Find("Id").GetComponent<Text>();
        pw = GameObject.Find("Pw").GetComponent<Text>();
    }

    public void OnClickCreateAccountButton()
    {
        if (id.text.Length != 0 && pw.text.Length != 0)
        {
            Debug.Log("회원가입");
            dataSender.CreateAccount(id.text, pw.text);
        }
    }
}
