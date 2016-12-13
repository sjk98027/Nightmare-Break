using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIManager : MonoBehaviour
{
	CharacterManager charManager;
    LoginUIManager loginUIManager;
    SelectUIManager selectUIManager;
    CreateUIManager createUIManager;
    WaitUIManager waitUIManager;
    BattleUIManager battleUIManager;

    public LoginUIManager LoginUIManager { get { return loginUIManager; } }
    public SelectUIManager SelectUIManager { get { return selectUIManager; } }
    public CreateUIManager CreateUIManager { get { return createUIManager; } }
    public WaitUIManager WaitUIManager { get { return waitUIManager; } }
    public BattleUIManager BattleUIManager { get { return battleUIManager; } }

    public GameObject dialogPanel;
    public Text dialog;

    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
            }

            return instance;
        }
    }

    public void SetLoginUIManager()
    {
        loginUIManager = GetComponent<LoginUIManager>();
        loginUIManager.ManagerInitialize();

        selectUIManager = GetComponent<SelectUIManager>();
    }

    public void SetSelectUIManager()
    {
        selectUIManager.SetUIObject();
        selectUIManager.InitializeAddListener();
        selectUIManager.SetCharacter();
    }

    public void SetCreateUIManager()
    {
        createUIManager = GetComponent<CreateUIManager>();
        createUIManager.SetUIObject();
        createUIManager.InitializeAddListner();
    }

    public void SetBattleUIManager()
    {
        charManager = GameObject.FindWithTag("Player").GetComponent<CharacterManager>();
        battleUIManager = GetComponent<BattleUIManager>();
        battleUIManager.SetUIObject();
    }

    public void SetWaitUIManager()
    {
        waitUIManager = GetComponent<WaitUIManager>();
        waitUIManager.ManagerInitialize();
    }

    public void SetDialog()
    {
        try
        {
            dialogPanel = GameObject.Find("DialogPanel");
            dialog = dialogPanel.transform.FindChild("Dialog").GetComponent<Text>();
            dialogPanel.SetActive(false);
        }
        catch
        {
            Debug.Log("Dialog Error");
        }
    }

    public IEnumerator Dialog(float delay, string text)
    {
        //dialogPanel.SetActive(true);

        //dialog.text = text;

        yield return new WaitForSeconds(delay);

        //dialog.text = "";

        //dialogPanel.SetActive(false);
    }
		 
	public void PointEnter(int skillIndex)
	{
		battleUIManager.SetPointEnterUI (skillIndex, 2, (int)charManager.CharStatus.HClass);
	}
	 
	public void OnPointExit()
	{
		battleUIManager.MouseOverUI.gameObject.transform.parent.gameObject.SetActive (false);
	}

    

}

    

    
