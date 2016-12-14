using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomUIManager : MonoBehaviour {

    private const int maxUser = 4;
	private Button skillBtn;
    private Button equipBtn;
    private Button myInfoBtn;
    private Button nextTypeBtn;
    private Button previousTypeBtn;
    private Button nextLevelBtn;
    private Button previousLevelBtn;
    private Image[] characterBackImage;
    private GameObject equipInfoUI;
    private GameObject skillAddUI;
    private GameObject myInfoUI;

    void Start()
    {
        characterBackImage = new Image[maxUser];

        skillBtn = GameObject.Find("SkillBtn").GetComponent<Button>();
        equipBtn = GameObject.Find("EquipBtn").GetComponent<Button>();
        myInfoBtn = GameObject.Find("MyInfoBtn").GetComponent<Button>();
        nextTypeBtn = GameObject.Find("NextTypeBtn").GetComponent<Button>();
        previousLevelBtn = GameObject.Find("PreviousTypeBtn").GetComponent<Button>();
        nextLevelBtn = GameObject.Find("NextLevelBtn").GetComponent<Button>();
        previousLevelBtn = GameObject.Find("PreviousLevelBtn").GetComponent<Button>();
        equipInfoUI = GameObject.Find("EquipInfoUI");
        skillAddUI = GameObject.Find("SkillAddUI");
        myInfoUI = GameObject.Find("MyInfoUI");
        for (int i = 0; i < maxUser; i++)
        { 
            characterBackImage[i] = GameObject.Find("CharacterBackImage"+(i+1)).GetComponent<Image>();
            characterBackImage[i].gameObject.SetActive(false);
        }
    }

    public  void InitializeAddListner()
    {

    }

   
}
