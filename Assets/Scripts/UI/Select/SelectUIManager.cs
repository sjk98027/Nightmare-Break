using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectUIManager : MonoBehaviour {
    private int checkNum;
    private const int maxCharacterNum = 3;
    GameObject characterPanel;
    GameObject createCharacterPanel;

    [SerializeField]Button createCharacterButton;
    Button deleteCharacterButton;
    Button returnToMainButton;
    Button startButton;

    Text character1Name;
    Text character2Name;
    Text character3Name;    

	[SerializeField]
	private Image[] backImage;
	[SerializeField]
	private GameObject[] selectImage;
	[SerializeField]
	private GameObject[] characterPos;
	[SerializeField]
	private Animator[] characterAnim;
    private Color[] alphaChange = new Color[2];

    CharacterList characterList;
    
    public CharacterList CharacterList { get { return characterList; } set { characterList = value; } }

    void Start()
    {

    }

    public void SetUIObject()
    {
        createCharacterButton = GameObject.Find("CreateCharacterButton").GetComponent<Button>();
        deleteCharacterButton = GameObject.Find("DeleteCharacterButton").GetComponent<Button>();
        startButton = GameObject.Find("StartButton").GetComponent<Button>();

        backImage = new Image[maxCharacterNum];
        selectImage = new GameObject[maxCharacterNum];
        characterPos = new GameObject[maxCharacterNum];
        characterAnim = new Animator[maxCharacterNum];

        alphaChange[0] = new Color(0, 0, 0, 0);
        alphaChange[1] = new Color(0, 0, 0, 1);

        for (int i = 0; i < maxCharacterNum; i++)
        {
            backImage[i] = GameObject.Find("BackImage" + (i + 1)).GetComponent<Image>();
            selectImage[i] = GameObject.Find("SelectEdge" + (i + 1));
            characterPos[i] = GameObject.Find("Pos" + (i + 1));

            //if (characterPos[i].transform.GetChild(1))
            //{
            //    characterAnim[i] = characterPos[i].transform.GetChild(1).GetComponent<Animator>();
            //    characterAnim[i].speed = 0;
            //}

            selectImage[i].SetActive(false);
        }
    }

    public void InitializeAddListener()
    {
        createCharacterButton.onClick.AddListener(() => OnClickCreateCharacterButton());
    }

    public void Select(int _imageindex)
    {
        checkNum = _imageindex;
        for(int i = 0; i < maxCharacterNum; i++)
        {
            if (selectImage[i].activeSelf)
            {
                selectImage[i].SetActive(false);
				characterAnim [i].SetBool("Select", false);
                backImage[i].color = alphaChange[1];
                characterAnim[i].speed = 0;
            }
        }
        if(!selectImage[_imageindex].activeSelf)
        {
            backImage[_imageindex].color = alphaChange[0];
            characterPos[_imageindex].SetActive(true);
            selectImage[_imageindex].SetActive(true);
			characterAnim [_imageindex].SetBool ("Select", true);
            characterAnim[_imageindex].speed = 1;
            startButton.interactable = true;
            deleteCharacterButton.interactable = true;
        } else
        {
            return;
        }
    }

    public void SetCharacter()
    {
        for (int i =0; i < maxCharacterNum; i++)
        {
            string className = "";

            if (characterList.CharacterData[i].Gender == (byte)CharacterStatus.Gender.Male)
            {
                if (characterList.CharacterData[i].HClass == (byte)CharacterStatus.CharClass.Warrior)
                {
                    className = "Class1";
                }
                else if (characterList.CharacterData[i].HClass == (byte)CharacterStatus.CharClass.Mage)
                {
                    className = "Class3";
                }
            }
            else if (characterList.CharacterData[i].Gender == (byte)CharacterStatus.Gender.FeMale)
            {
                if (characterList.CharacterData[i].HClass == (byte)CharacterStatus.CharClass.Warrior)
                {
                    className = "Class2";
                }
                else if (characterList.CharacterData[i].HClass == (byte)CharacterStatus.CharClass.Mage)
                {
                    className = "Class4";
                }
            }

            if (className != "")
            {
                GameObject character = Instantiate(Resources.Load<GameObject>(className), characterPos[0].transform) as GameObject;
                character.transform.position = Vector3.zero;
            }            
        }
    }

    public void OnClickCreateCharacterButton()
    {
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.CreateScene, false);
    }
}
