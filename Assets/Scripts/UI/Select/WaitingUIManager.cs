using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaitingUIManager : MonoBehaviour {

	private int selectNum;
    private Button roomCreateBtn;
    private Button skillAddBtn;
    private Button equipInfoBtn;
    private Button myInfoBtn;
    private Button roomCreateExitBtn;
    private Button skillAddExitBtn;
    private Button equipInfoExitBtn;
    private Button myInfoExitBtn;
	private GameObject roomCreateUI;
	private GameObject skillAddUI;
	private GameObject equipInfoUI;
	private GameObject myInfoUI;

	void Start()
	{
        roomCreateBtn = GameObject.Find("RoomCreateBtn").GetComponent<Button>();
        skillAddBtn = GameObject.Find("SkillAddBtn").GetComponent<Button>();
        equipInfoBtn = GameObject.Find("EquipBtn").GetComponent<Button>();
        myInfoBtn = GameObject.Find("MyInfoBtn").GetComponent<Button>();
		roomCreateUI = GameObject.Find ("RoomCreateUI");
		skillAddUI = GameObject.Find ("SkillAddUI");
		equipInfoUI = GameObject.Find ("EquipInfoUI");
		myInfoUI = GameObject.Find ("MyInfoUI");
        roomCreateExitBtn = roomCreateUI.transform.GetChild(6).GetComponent<Button>();
        skillAddExitBtn = skillAddUI.transform.GetChild(3).GetComponent<Button>();
        equipInfoExitBtn = equipInfoUI.transform.GetChild(2).GetComponent<Button>();
        myInfoExitBtn = myInfoBtn.transform.GetChild(1).GetComponent<Button>();

		roomCreateUI.SetActive (false);
		skillAddUI.SetActive (false);
		equipInfoUI.SetActive (false);
		myInfoUI.SetActive (false);
        InitializeAddListner();
    }

    public void InitializeAddListner()
    {
        roomCreateBtn.onClick.AddListener(() => RoomCreate());
        skillAddBtn.onClick.AddListener(() => SkillAdd());
        equipInfoBtn.onClick.AddListener(() => EquipInfo());
        myInfoBtn.onClick.AddListener(() => MyInfo());
        roomCreateBtn.onClick.AddListener(() => BtnActiveCheck(0));
        skillAddExitBtn.onClick.AddListener(() => BtnActiveCheck(1));
        equipInfoExitBtn.onClick.AddListener(() => BtnActiveCheck(2));
        myInfoExitBtn.onClick.AddListener(() => BtnActiveCheck(4));
    }
    public void RoomCreate()
	{
		BtnActiveCheck (selectNum = 0);

		roomCreateUI.SetActive (true);
	}

	public void SkillAdd()
	{
		BtnActiveCheck ();
		selectNum = 1;
		skillAddUI.SetActive (true);
	}

	public void EquipInfo()
	{
		BtnActiveCheck ();
		selectNum = 2;
		equipInfoUI.SetActive(true);
	}

	public void MyInfo()
	{
		BtnActiveCheck ();
		selectNum = 3;
		myInfoUI.SetActive (true);
	}

	private void BtnActiveCheck()
	{
		if (selectNum == 0) {
			roomCreateUI.SetActive (false);
		} else if (selectNum == 1) {
			skillAddUI.SetActive (false);
		} else if (selectNum == 2) {
			equipInfoUI.SetActive (false);
		} else if (selectNum == 3) {
			myInfoUI.SetActive (false);
		}
	}

	public void BtnActiveCheck(int btnIndex)
	{
		if (btnIndex == 0) {
			roomCreateUI.SetActive (false);
		} else if (btnIndex == 1) {
			skillAddUI.SetActive (false);
		} else if (btnIndex == 2) {
			equipInfoUI.SetActive (false);
		} else if (btnIndex == 3) {
			myInfoUI.SetActive (false);
		}
	}
}
