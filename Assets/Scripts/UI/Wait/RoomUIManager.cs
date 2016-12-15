using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomUIManager : MonoBehaviour {

    private const int maxUser = 4;

    private Button skillBtn;
    private Button equipBtn;
    private Button myInfoBtn;
    private Button gameStartBtn;
    private Button roomExitBtn;
    private Button nextTypeBtn;
    private Button previousTypeBtn;
    private Button nextLevelBtn;
    private Button previousLevelBtn;

    private Button equipCloseBtn;
    private Button skillCloseBtn;
    private Button myInfoCloseBtn;

    private Image[] characterBackImage;

    private GameObject equipInfoUI;
    private GameObject skillAddUI;
    private GameObject myInfoUI;

    private RoomData roomData;

    public void ManagerInitialize()
    {
        SetUIObject();
        InitializeAddListner();
    }

    public void SetUIObject()
    {
        characterBackImage = new Image[maxUser];

        skillBtn = GameObject.Find("SkillBtn").GetComponent<Button>();
        equipBtn = GameObject.Find("EquipBtn").GetComponent<Button>();
        myInfoBtn = GameObject.Find("MyInfoBtn").GetComponent<Button>();

        gameStartBtn = GameObject.Find("GameStartBtn").GetComponent<Button>();
        roomExitBtn = GameObject.Find("RoomExitBtn").GetComponent<Button>();

        nextTypeBtn = GameObject.Find("NextTypeBtn").GetComponent<Button>();
        previousLevelBtn = GameObject.Find("PreviousTypeBtn").GetComponent<Button>();
        nextLevelBtn = GameObject.Find("NextLevelBtn").GetComponent<Button>();
        previousLevelBtn = GameObject.Find("PreviousLevelBtn").GetComponent<Button>();

        equipInfoUI = GameObject.Find("EquipInfoUI");
        skillAddUI = GameObject.Find("SkillAddUI");
        myInfoUI = GameObject.Find("MyInfoUI");

        equipCloseBtn = equipInfoUI.transform.GetChild(2).GetComponent<Button>();
        skillCloseBtn = skillAddUI.transform.GetChild(3).GetComponent<Button>();
        myInfoBtn = myInfoUI.transform.GetChild(1).GetComponent<Button>();

        equipInfoUI.SetActive(false);
        skillAddUI.SetActive(false);
        myInfoUI.SetActive(false);

        for (int i = 0; i < maxUser; i++)
        {
            characterBackImage[i] = GameObject.Find("CharacterBackImage" + (i + 1)).GetComponent<Image>();
            characterBackImage[i].gameObject.SetActive(false);
        }
    }

    public void InitializeAddListner()
    {
        skillBtn.onClick.AddListener(() => OpenSkillUI());
        equipBtn.onClick.AddListener(() => OpenEquipUI());
        myInfoBtn.onClick.AddListener(() => OpenMyInfoUI());
        gameStartBtn.onClick.AddListener(() => GameStart());
        roomExitBtn.onClick.AddListener(() => RoomExit());
        equipCloseBtn.onClick.AddListener(() => CloseEquipUI());
        myInfoCloseBtn.onClick.AddListener(() => CloseMyInfoUI());
        skillCloseBtn.onClick.AddListener(() => CloseSkillUI());
    }

    public void SetUserList(RoomData newRoomUserList)
    {
        roomData = newRoomUserList;
    }

    public void SetUserData()
    {

    }

    void GameStart()
    {
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.InGameScene, true);
    }

    void RoomExit()
    {
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.WaitingScene, false);
    }

    void OpenEquipUI()
    {
        equipInfoUI.SetActive(true);
    }

    void OpenSkillUI()
    {
        skillAddUI.SetActive(true);
    }

    void OpenMyInfoUI()
    {
        myInfoUI.SetActive(true);
    }

    void CloseEquipUI()
    {
        equipInfoUI.SetActive(false);
    }

    void CloseSkillUI()
    {
        skillAddUI.SetActive(false);
    }

    void CloseMyInfoUI()
    {
        myInfoUI.SetActive(false);
    }
}
