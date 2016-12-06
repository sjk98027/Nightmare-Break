using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterCreateUI : MonoBehaviour {

	private const int maxClass = 4;
	private string nickName; 
	private bool input;
	//private Animator[] characterAnim;
	[SerializeField]
	private Animator characterAnim;
	private GameObject[] classPrefeb;
	private GameObject[] selectImage;
	[SerializeField]
	private InputField nickNameInputField;
	[SerializeField]
	private Button characterCreateBtn;
	[SerializeField]
	private Button cancleBtn;


	void Start()
	{
		//characterAnim = new Animator[maxClass];
		selectImage = new GameObject[maxClass];
		for (int i = 0; i < maxClass; i++) {
			selectImage [i] = GameObject.Find ("Select" + (i + 1));
			selectImage [i].SetActive (false);
		}
	}

	public void LeftArrow()
	{

	}

	public void RightArrow()
	{

	}
	public void CreateCharacter()
	{
		
	}

	public void Cancle()
	{

	}

	public void InputFinish()
	{
		nickName = nickNameInputField.text;
		characterCreateBtn.interactable = true;
	}

	public void ClassSelect(int _index)
	{
		for (int i = 0; i < maxClass; i++) {
			selectImage [i].SetActive (false);
		}

		if (!selectImage [_index].activeSelf) {
			selectImage [_index].SetActive (true);
			characterAnim.SetTrigger ("CreateSelect");
		}
	}
}
