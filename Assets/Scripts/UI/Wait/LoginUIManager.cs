using UnityEngine;
using UnityEngine.UI;

public class LoginUIManager
{
    DataSender dataSender;

    Button loginButton;
    Button createAccountButton;

    Text id;
    Text pw;

    public void ManagerInitialize()
    {
        dataSender = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<DataSender>();

        SetUIObject();
        SetAddListener();
    }

    public void SetUIObject()
    {
        loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        createAccountButton = GameObject.Find("CreateAccountButton").GetComponent<Button>();

        id = GameObject.Find("Id").GetComponent<Text>();
        pw = GameObject.Find("Pw").GetComponent<Text>();
    }

    public void SetAddListener()
    {
        loginButton.onClick.AddListener(() => OnClickLoginButton());
        createAccountButton.onClick.AddListener(() => OnClickCreateAccountButton());
    }

    public void OnClickCreateAccountButton()
    {
        Debug.Log("회원가입");

        if (id.text.Length != 0 && pw.text.Length != 0)
        {
            dataSender.CreateAccount(id.text, pw.text);
        }
    }

    public void OnClickLoginButton()
    {
        Debug.Log("로그인");

        if (id.text.Length != 0 && pw.text.Length != 0)
        {
            dataSender.Login(id.text, pw.text);
        }
    }
}
