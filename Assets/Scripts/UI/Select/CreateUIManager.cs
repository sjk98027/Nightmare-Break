using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateUIManager : MonoBehaviour {

	private const int maxClass = 4;
    private const int rotateValue = 30;
    private const int currentMaxClass = 2;
    private const int maxRotateBtn = 2;
	private int currentClass;
    private int currentGender; //0은 남자, 1은 여자
    private bool[] btnPushCheck;
    private Animator characterAnim;
	private GameObject[] selectImage;
	private GameObject[] genderSelectImage;
	private Transform characterPos;
    [SerializeField]
	private GameObject[] classPrefeb;
	private Text nickName;
	private Button characterCreateBtn;
	private Button[] rotateBtn;
	private Button cancleBtn;
	private Button[] genderBtn;
	private GameObject[] classSkill;
	private Button[] classBtn;
	private EventTrigger.Entry[] exitEvent;
	private EventTrigger.Entry[] downEvent;
	private EventTrigger.Entry[] upEvent;

    public void SetUIObject()
    {
        currentGender = 0;
        rotateBtn = new Button[maxRotateBtn];
        btnPushCheck = new bool[maxRotateBtn];
        genderSelectImage = new GameObject[currentMaxClass];
        genderBtn = new Button[currentMaxClass];
        classSkill = new GameObject[currentMaxClass];
        selectImage = new GameObject[maxClass];
        classPrefeb = new GameObject[maxClass];
		classBtn = new Button[maxClass];

        characterPos = GameObject.Find("CharacterPrefebPos").transform;
        nickName = GameObject.Find("NickName").GetComponent<Text>();
        characterCreateBtn = GameObject.Find("CharacterCreateBtn").GetComponent<Button>();
        cancleBtn = GameObject.Find("CancleBtn").GetComponent<Button>();

		exitEvent = new EventTrigger.Entry[rotateBtn.Length];
		downEvent = new EventTrigger.Entry[rotateBtn.Length];
		upEvent = new EventTrigger.Entry[rotateBtn.Length];

        for (int i = 0; i < maxClass; i++)
        {
			classBtn[i] = GameObject.Find ("ClassBtn"+(i + 1)).GetComponent<Button> ();
            selectImage[i] = GameObject.Find("Select" + (i + 1));
            selectImage[i].SetActive(false);
        }

        for (int i = 0; i < genderSelectImage.Length; i++)
        {
			exitEvent [i].eventID = EventTriggerType.PointerExit;
			downEvent [i].eventID = EventTriggerType.PointerDown;
			upEvent [i].eventID = EventTriggerType.PointerUp;
            rotateBtn[i] = GameObject.Find("RotateArrow" + i).GetComponent<Button>();
			rotateBtn [i].GetComponent<EventTrigger> ().triggers.Add (exitEvent [i]);
			rotateBtn [i].GetComponent<EventTrigger> ().triggers.Add (downEvent [i]);
			rotateBtn [i].GetComponent<EventTrigger> ().triggers.Add (upEvent [i]);
            classSkill[i] = GameObject.Find("SkillUI" + i);
        //  genderSelectImage[i] = GameObject.Find("Gender" + i).transform.GetChild(0).gameObject;
            genderBtn[i] = GameObject.Find("Gender" + i).transform.GetChild(1).gameObject.GetComponent<Button>();
			genderBtn [i].onClick.AddListener (() => GenderChange (i));
            classSkill[i].SetActive(false);
            genderSelectImage[i].SetActive(false);
        }

        for (int i = 0; i < classPrefeb.Length; i++)
        {
            classPrefeb[i] = Instantiate(Resources.Load<GameObject>("UI/Class" + (i + 1)), characterPos.transform) as GameObject;
            classPrefeb[i].transform.position = characterPos.position;
        }

    }

    public void InitializeAddListner()
    {
        characterCreateBtn.onClick.AddListener(() => OnClickCreateCharacterButton());
        cancleBtn.onClick.AddListener(() => Cancle());
		genderBtn [0].onClick.AddListener (() => GenderChange (0));
		genderBtn [1].onClick.AddListener (() => GenderChange (1));
		classBtn [0].onClick.AddListener (() => ClassSelect (0));
		classBtn [1].onClick.AddListener (() => ClassSelect (1));
		classBtn [2].onClick.AddListener (() => ClassSelect (2));
		classBtn [3].onClick.AddListener (() => ClassSelect (3));
		exitEvent[0].callback.AddListener((data)=> RotateCheckOut(0));
		exitEvent[1].callback.AddListener((data)=> RotateCheckOut(1));
		downEvent[0].callback.AddListener((data)=> PrefebRotate(0));
		downEvent[1].callback.AddListener((data)=> PrefebRotate(1));
		upEvent[0].callback.AddListener((data)=> RotateCheckOut(0));
		upEvent[1].callback.AddListener((data)=> RotateCheckOut(1));
    }

	public void PrefebRotate(int _index)
	{
        btnPushCheck[_index] = true;
        StartCoroutine(CharacterRotate(_index));
	}

    public void RotateCheckOut(int _index)
    {
        btnPushCheck[_index] = false;
    }

	public void CreateCharacter()
	{
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.SelectScene, false);
	}

	public void Cancle()
	{
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.SelectScene, false);
    }

	public void InputFinish()
	{
		characterCreateBtn.interactable = true;
	}

	public void ClassSelect(int index)
	{
		for (int i = 0; i < maxClass * currentMaxClass; i++) {
			if (i < maxClass) {
				selectImage [i].SetActive (false);
                classPrefeb [i].SetActive(false);
            }

			if (i < genderSelectImage.Length) {
				genderSelectImage [i].SetActive (false);
                classSkill[i].SetActive(false);
            }
        }
		if (!selectImage [index].activeSelf) {
            selectImage [index].SetActive (true);
			classSkill [index].SetActive (true);
			currentGender = 0;
			genderSelectImage [currentGender].SetActive (true);
            currentClass = index + index;
			classPrefeb[index + index].SetActive(true);
			characterAnim = classPrefeb[index + index].GetComponent<Animator>();
            for(int i=0; i < currentMaxClass; i++)
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
			classPrefeb [_genderindex + currentGender + currentClass].SetActive (false);
			genderSelectImage[currentGender].SetActive(false);
            currentGender = _genderindex;
			classPrefeb [currentGender + currentClass].SetActive (true);
			genderSelectImage[currentGender].SetActive(true);
            characterAnim = classPrefeb[currentGender].GetComponent<Animator>();
        }
        else if  (_genderindex == 1) {
			classPrefeb [currentGender + currentClass].SetActive (false);
			genderSelectImage[currentGender].SetActive(false);
            currentGender = _genderindex;
			genderSelectImage[currentGender].SetActive(true);
            classPrefeb [currentGender + currentClass].SetActive (true);
			characterAnim = classPrefeb[currentGender + currentClass].GetComponent<Animator>();
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
				classPrefeb[currentClass + currentGender].transform.Rotate(0, rotateValue * time, 0);
            }
            else if (btnPushCheck[1])
            {
				classPrefeb[currentClass + currentGender].transform.Rotate(0, -rotateValue * time, 0);
            }
            yield return null;
        }
        time = 0;
    }

    public void OnClickCreateCharacterButton()
    {
        if (nickName.text.Length < 1)
        {
            Debug.Log("캐릭터 이름을 1글자 이상 입력하세요");
        }
        else
        {
            DataSender.Instance.CreateCharacter(currentGender, currentClass, nickName.text);
        }
    }

    //public void OnClickDeleteCharacterButton()
    //{
    //    DataSender.Instance.DeleteCharacter(currentCharacter);
    //}

    //public void OnClickSelectCharacterButton()
    //{
    //    DataSender.Instance.SelectCharacter(currentCharacter);
    //}
}
