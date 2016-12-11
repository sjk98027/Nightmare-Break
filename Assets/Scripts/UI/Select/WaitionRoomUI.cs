using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaitionRoomUI : MonoBehaviour {

	private int selectNum;
	private GameObject roomCreateUI;
	private GameObject skillAddUI;
	private GameObject equipInfoUI;
	private GameObject myInfoUI;

	void Start()
	{
		roomCreateUI = GameObject.Find ("RoomCreateUI");
		skillAddUI = GameObject.Find ("SkillAddUI");
		equipInfoUI = GameObject.Find ("EquipInfoUI");
		myInfoUI = GameObject.Find ("MyInfoUI");

		roomCreateUI.SetActive (false);
		skillAddUI.SetActive (false);
		equipInfoUI.SetActive (false);
		myInfoUI.SetActive (false);
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
