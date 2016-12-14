using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private EventTrigger.Entry[] clickEvent;

    CharacterList characterList;
    
    public CharacterList CharacterList { get { return characterList; } set { characterList = value; } }

    public void SetUIObject()
    {
      
        createCharacterButton = GameObject.Find("CreateCharacterButton").GetComponent<Button>();
        deleteCharacterButton = GameObject.Find("DeleteCharacterButton").GetComponent<Button>();
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        returnToMainButton = GameObject.Find("ReturnToMainButton").GetComponent<Button>();

        backImage = new Image[maxCharacterNum];
        selectImage = new GameObject[maxCharacterNum];
        characterPos = new GameObject[maxCharacterNum];
        characterAnim = new Animator[maxCharacterNum];
        clickEvent = new EventTrigger.Entry[maxCharacterNum];
        alphaChange[0] = new Color(0, 0, 0, 0);
        alphaChange[1] = new Color(0, 0, 0, 1);

        for (int i = 0; i < maxCharacterNum; i++)
        {
            clickEvent[i]= new EventTrigger.Entry();
            clickEvent[i].eventID = EventTriggerType.PointerClick; 
            backImage[i] = GameObject.Find("BackImage" + (i + 1)).GetComponent<Image>();
            backImage[i].GetComponent<EventTrigger>().triggers.Add(clickEvent[i]);
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
        returnToMainButton.onClick.AddListener(() => OnClickReturnToMainButton());
        clickEvent[0].callback.AddListener((data) => Select(0));
        clickEvent[1].callback.AddListener((data) => Select(1));
        clickEvent[2].callback.AddListener((data) => Select(2));
    }

    public void Select(int _imageindex)
    {
        checkNum = _imageindex;
        for(int i = 0; i < maxCharacterNum; i++)
        {
            if (selectImage[i].activeSelf)
            {
                selectImage[i].SetActive(false);
                backImage[i].color = alphaChange[1];
                characterAnim[i].speed = 0;
            }
        }
        if(!selectImage[_imageindex].activeSelf)
        {
            backImage[_imageindex].color = alphaChange[0];
            characterPos[_imageindex].SetActive(true);
            selectImage[_imageindex].SetActive(true);
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
        for (int CharacterIndex = 0; CharacterIndex < maxCharacterNum; CharacterIndex++)
        {
            if (characterList.CharacterData[CharacterIndex].Level > 0)
            {
                string className = "";

                if (characterList.CharacterData[CharacterIndex].Gender == (byte)CharacterStatus.Gender.Male)
                {
                    if (characterList.CharacterData[CharacterIndex].HClass == (byte)CharacterStatus.CharClass.Warrior)
                    {
                        className = "Class1";
                    }
                    else if (characterList.CharacterData[CharacterIndex].HClass == (byte)CharacterStatus.CharClass.Mage)
                    {
                        className = "Class3";
                    }
                }
                else if(characterList.CharacterData[CharacterIndex].Gender == (byte)CharacterStatus.Gender.FeMale)
                {
                    if (characterList.CharacterData[CharacterIndex].HClass == (byte)CharacterStatus.CharClass.Warrior)
                    {
                        className = "Class2";
                    }
                    else if (characterList.CharacterData[CharacterIndex].HClass == (byte)CharacterStatus.CharClass.Mage)
                    {
                        className = "Class4";
                    }
                }

                if (className != "")
                {
                    GameObject character = Instantiate(Resources.Load<GameObject>("UI/" + className), characterPos[CharacterIndex].transform) as GameObject;
                    character.SetActive(true);
                    character.transform.localPosition = Vector3.zero;
                    character.transform.localRotation = Quaternion.identity;
                    characterAnim[CharacterIndex] = character.GetComponent<Animator>();
                    characterAnim[CharacterIndex].speed = 0;
                }
            }
        }
    }

    public void OnClickCreateCharacterButton()
    {
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.CreateScene, false);
    }

    public void OnClickReturnToMainButton()
    {
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.TitleScene, true);
    }
}
