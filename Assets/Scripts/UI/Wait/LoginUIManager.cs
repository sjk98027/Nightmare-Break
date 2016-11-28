using UnityEngine;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
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
    Button selectCharacterButton;
    Button returnToMainButton;
    Button maleButton;
    Button femaleButton;
    Button warriorButton;
    Button mageButton;
    Button gunnerButton;
    Button shieldWarriorButton;
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

    int currentCharacter;
    CharacterStatus.Gender gender;
    CharacterStatus.CharClass charClass;

    public void ManagerInitialize()
    {
        SetUIObject();
        InitializeAddListener();
    }

    public void SetUIObject()
    {
        loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        createAccountButton = GameObject.Find("CreateAccountButton").GetComponent<Button>();
        deleteAccountButton = GameObject.Find("DeleteAccountButton").GetComponent<Button>();

        createCharacterButton = GameObject.Find("CreateCharacterButton").GetComponent<Button>();
        deleteCharacterButton = GameObject.Find("DeleteCharacterButton").GetComponent<Button>();
        selectCharacterButton = GameObject.Find("SelectCharacterButton").GetComponent<Button>();
        maleButton = GameObject.Find("MaleButton").GetComponent<Button>();
        femaleButton = GameObject.Find("FeMaleButton").GetComponent<Button>();
        warriorButton = GameObject.Find("WarriorButton").GetComponent<Button>();
        mageButton = GameObject.Find("MageButton").GetComponent<Button>();
        gunnerButton = GameObject.Find("GunnerButton").GetComponent<Button>();
        shieldWarriorButton = GameObject.Find("ShieldWarriorButton").GetComponent<Button>();

        loginId = GameObject.Find("LoginId").GetComponent<Text>();
        loginPw = GameObject.Find("LoginPw").GetComponent<Text>();
        createId = GameObject.Find("LoginId").GetComponent<Text>();
        createPw = GameObject.Find("LoginPw").GetComponent<Text>();
        deleteId = GameObject.Find("LoginId").GetComponent<Text>();
        deletePw = GameObject.Find("LoginPw").GetComponent<Text>();

        createCharacterName = GameObject.Find("CreateCharacterName").GetComponent<Text>();
    }

    public void InitializeAddListener()
    {
        loginButton.onClick.AddListener(() => OnClickLoginButton());
        createAccountButton.onClick.AddListener(() => OnClickCreateAccountButton());
        deleteAccountButton.onClick.AddListener(() => OnClickDeleteAccountButton());

        createCharacterButton.onClick.AddListener(() => OnClickCreateCharacterButton());
        deleteCharacterButton.onClick.AddListener(() => OnClickDeleteCharacterButton());
        selectCharacterButton.onClick.AddListener(() => OnClickSelectCharacterButton());
    }

    public void OnClickCreateAccountButton()
    {
        Debug.Log("회원가입");

        if (createId.text.Length >= 4 && createPw.text.Length >= 6)
        {
            DataSender.Instance.CreateAccount(createId.text, createPw.text);
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
            DataSender.Instance.DeleteAccount(deleteId.text, deletePw.text);
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
            DataSender.Instance.Login(loginId.text, loginPw.text);
        }
        else
        {
            Debug.Log("아이디 4글자 이상, 비밀번호 6글자 이상 입력하세요");
        }
    }

    public void OnClickMaleButton()
    {
        gender = CharacterStatus.Gender.Male;
    }

    public void OnClickFeMaleButton()
    {
        gender = CharacterStatus.Gender.FeMale;
    }

    public void OnClickWarriorButton()
    {
        charClass = CharacterStatus.CharClass.Warrior;
    }

    public void OnClickMageButton()
    {
        charClass = CharacterStatus.CharClass.Mage;
    }

    public void OnClickGunnerButton()
    {
        charClass = CharacterStatus.CharClass.Gunner;
    }

    public void OnClickShildWarriorButton()
    {
        charClass = CharacterStatus.CharClass.ShildWarrior;
    }

    public void OnClickCreateCharacterButton()
    {
        Debug.Log("캐릭터 생성");

        if (createCharacterName.text.Length >= 1)
        {
            Debug.Log("캐릭터 이름을 1글자 이상 입력하세요");
        }
        else
        {
            DataSender.Instance.CreateCharacter((int)gender, (int)charClass, createCharacterName.text);
        }
    }

    public void OnClickDeleteCharacterButton()
    {
        Debug.Log("캐릭터 삭제");

        DataSender.Instance.DeleteCharacter(currentCharacter);
    }

    public void OnClickSelectCharacterButton()
    {
        Debug.Log("캐릭터 선택");

        DataSender.Instance.SelectCharacter(currentCharacter);
    }
}