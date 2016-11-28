using UnityEngine;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
    DataSender dataSender;

    GameObject createAccountPanel;
    GameObject deleteAccountPanel;
    GameObject characterPanel;
    GameObject createCharacterPanel;

    Button loginButton;
    Button createAccountButton;
    Button deleteAccountButton;
    Button createAccountCancelButton;
    Button deleteAccountCancelButton;
    Button createCharacterButton;
    Button deleteCharacterButton;
    Button selectcharacterButton;
    Button returnToMainButton;
    Button maleButton;
    Button femaleButton;
    Button warriorButton;
    Button mageButton;
    Button archerButton;
    Button paladinButton;
    Button createCharacterYesButton;
    Button createCharacterNoButton;
    
    Text loginId;
    Text loginPw;
    Text createId;
    Text createPw;
    Text deleteId;
    Text deletePw;
    Text character1Name;
    Text character2Name;
    Text character3Name;
    Text character4Name;
    Text createCharacterName;

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
        deleteAccountButton = GameObject.Find("DeleteAccountButton").GetComponent<Button>();

        loginId = GameObject.Find("LoginId").GetComponent<Text>();
        loginPw = GameObject.Find("LoginPw").GetComponent<Text>();
        createId = GameObject.Find("LoginId").GetComponent<Text>();
        createPw = GameObject.Find("LoginPw").GetComponent<Text>();
        deleteId = GameObject.Find("LoginId").GetComponent<Text>();
        deletePw = GameObject.Find("LoginPw").GetComponent<Text>();
    }

    public void SetAddListener()
    {
        loginButton.onClick.AddListener(() => OnClickLoginButton());
        createAccountButton.onClick.AddListener(() => OnClickCreateAccountButton());

    }

    public void OnClickCreateAccountButton()
    {
        Debug.Log("회원가입");

        if (createId.text.Length >= 4 && createPw.text.Length >= 6)
        {
            dataSender.CreateAccount(createId.text, createPw.text);
        }
        else
        {
            Debug.Log("아이디 4글자 이상, 비밀번호 6글자 이상 입력하세요");
        }
    }

    public void OnClickDeleteAccountButton()
    {
        Debug.Log("계정 탈퇴");

        if (deleteId.text.Length >= 4 && deletePw.text.Length >= 6)
        {
            dataSender.DeleteAccount(deleteId.text, deletePw.text);
        }
        else
        {
            Debug.Log("아이디 4글자 이상, 비밀번호 6글자 이상 입력하세요");
        }
    }

    public void OnClickLoginButton()
    {
        Debug.Log("로그인");

        if (loginId.text.Length >= 4 && loginPw.text.Length >= 6)
        {
            dataSender.Login(loginId.text, loginPw.text);
        }
        else
        {
            Debug.Log("아이디 4글자 이상, 비밀번호 6글자 이상 입력하세요");
        }
    }
}