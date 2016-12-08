using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterCreateUI : MonoBehaviour {

	private const int maxClass = 4;
    private const int rotateValue = 30;
	private string nickName; 
	private bool input;
	private int currentPickClass;
    private int currentGender; //0은 남자, 1은 여자
    private bool[] btnPushCheck;
    private Animator characterAnim;
	private GameObject[] selectImage;

	public Transform characterPos;
	public GameObject[] classPrefeb;
	public InputField nickNameInputField;
	public Button characterCreateBtn;
	public Button[] rotateBtn;
	public Button cancleBtn;
	public Button[] genderBtn;
	public GameObject[] classSkill;

	void Start()
	{
	    btnPushCheck = new bool[2];
		selectImage = new GameObject[maxClass];
		for (int i = 0; i < maxClass; i++) {
           // classPrefeb[i] = Resources.Load<GameObject>("Class" + (i + 1));
			selectImage [i] = GameObject.Find ("Select" + (i + 1));
			selectImage [i].SetActive (false);
		}

		for (int i = 0; i < classPrefeb.Length; i++)
		{
			classPrefeb [i].transform.position = characterPos.position;
		}
	}

	public void PrefebRotate(int _index)
	{
        btnPushCheck[_index] = true;
        StartCoroutine(CharacterRotate(_index));
	}

    public void PushCheckOut(int _index)
    {
        btnPushCheck[_index] = false;
    }

	public void CreateCharacter()
	{
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.TitleScene);
	}

	public void Cancle()
	{
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.TitleScene);
    }

	public void InputFinish()
	{
		nickName = nickNameInputField.text;
		characterCreateBtn.interactable = true;
	}

	public void ClassSelect(int _index)
	{
		for (int i = 0; i < maxClass * genderBtn.Length; i++) {
			if (i < maxClass) {
				selectImage [i].SetActive (false);
				classSkill [i].SetActive (false);
			}
            classPrefeb[i].SetActive(false);
		}
		if (!selectImage [_index].activeSelf) {
			selectImage [_index].SetActive (true);
			classSkill [_index].SetActive (true);
			currentGender = 0;
			currentPickClass = _index + _index;
			classPrefeb[_index + _index].SetActive(true);
			characterAnim = classPrefeb[_index + _index].GetComponent<Animator>();
            for(int i=0; i < rotateBtn.Length; i++)
            {
				genderBtn[i].interactable = true;
                rotateBtn[i].interactable = true;
            }
        }
	}

	public void GenderChange(int _genderindex)
	{
		if (currentGender == _genderindex) {
			return;
		} else if (_genderindex == 0) {
			classPrefeb [_genderindex + currentGender + currentPickClass].SetActive (false);
			currentGender = _genderindex;
			classPrefeb [currentGender].SetActive (true);
			characterAnim = classPrefeb[currentGender].GetComponent<Animator>();
		}else if  (_genderindex == 1) {
			classPrefeb [currentGender + currentPickClass].SetActive (false);
			currentGender = _genderindex;
			classPrefeb [currentGender + currentPickClass].SetActive (true);
			characterAnim = classPrefeb[currentGender + currentPickClass].GetComponent<Animator>();
		}
	}

	public void StartSkillAnim(int _skillNum)
	{
		characterAnim.SetTrigger ("Skill" + _skillNum);
	}

    IEnumerator CharacterRotate(int _index)
    {
        float time = Time.smoothDeltaTime;
        while(btnPushCheck[_index])
        {
            if (btnPushCheck[0])
            {
				classPrefeb[currentPickClass + currentGender].transform.Rotate(0, rotateValue * time, 0);
            }
            else if (btnPushCheck[1])
            {
				classPrefeb[currentPickClass + currentGender].transform.Rotate(0, -rotateValue * time, 0);
            }
            yield return null;
        }
        time = 0;
    }

}
