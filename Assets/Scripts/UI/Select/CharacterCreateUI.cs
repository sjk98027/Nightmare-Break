using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterCreateUI : MonoBehaviour {

	private const int maxClass = 4;
    private const int rotateValue = 30;
	private string nickName; 
	private bool input;
    //private Animator[] characterAnim;
    private int pickClass;
    private int gender; //0은 남자, 1은 여자
    [SerializeField]
    private bool[] btnPushCheck;
    [SerializeField]
    private Animator characterAnim;
    [SerializeField]
    private Transform characterPos;
    [SerializeField]
    private GameObject[] classPrefeb;
    [SerializeField]
    private GameObject[] selectImage;
	[SerializeField]
	private InputField nickNameInputField;
	[SerializeField]
	private Button characterCreateBtn;
    [SerializeField]
    private Button[] rotateBtn;
	[SerializeField]
	private Button cancleBtn;


	void Start()
	{
        gender = 0;
        btnPushCheck = new bool[2];
		selectImage = new GameObject[maxClass];

        classPrefeb[0].transform.position = characterPos.position;
        classPrefeb[1].transform.position = characterPos.position; 

		for (int i = 0; i < maxClass; i++) {
           // classPrefeb[i] = Resources.Load<GameObject>("Class" + (i + 1));
			selectImage [i] = GameObject.Find ("Select" + (i + 1));
			selectImage [i].SetActive (false);
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
		for (int i = 0; i < maxClass; i++) {
			selectImage [i].SetActive (false);
            classPrefeb[i].SetActive(false);
            if(i < 2)
            {
                rotateBtn[i].interactable = false;
            }
		}
		if (!selectImage [_index].activeSelf) {
			selectImage [_index].SetActive (true);
            pickClass = _index;
            classPrefeb[_index].SetActive(true);
            characterAnim = classPrefeb[_index].GetComponent<Animator>();
            characterAnim.SetTrigger("CreateSelect");
            for(int i=0; i < rotateBtn.Length; i++)
            {
                rotateBtn[i].interactable = true;
            }
        }
	}

    IEnumerator CharacterRotate(int _index)
    {
        float time = Time.smoothDeltaTime;
        while(btnPushCheck[_index])
        {
            if (btnPushCheck[0])
            {
                classPrefeb[pickClass].transform.Rotate(0, rotateValue * time, 0);
            }
            else if (btnPushCheck[1])
            {
                classPrefeb[pickClass].transform.Rotate(0, -rotateValue * time, 0);
            }

            yield return null;
        }
        time = 0;
    }

}
