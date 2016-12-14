using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    public void InitializeUIManager()
    {
        if (loginUIManager != null) Destroy(loginUIManager);
        if (selectUIManager != null) Destroy(selectUIManager);
        if (createUIManager != null) Destroy(createUIManager);
        if (waitUIManager != null) Destroy(waitUIManager);
        if (battleUIManager != null) Destroy(battleUIManager);
    }

    public void SetLoginUIManager()
    {
        InitializeUIManager();

        loginUIManager = gameObject.AddComponent<LoginUIManager>();
        loginUIManager.ManagerInitialize();
    }

    public void CreateSelectUIManager()
    {
        InitializeUIManager();

        selectUIManager = gameObject.AddComponent<SelectUIManager>();
    }

    public void SetSelectUIManager()
    {
        selectUIManager.SetUIObject();
        selectUIManager.InitializeAddListener();
        selectUIManager.SetCharacter();
    }

    public void CreateCreateUIManager()
    {
        InitializeUIManager();

        createUIManager = gameObject.AddComponent<CreateUIManager>();
    }

    public void SetCreateUIManager()
    {
        InitializeUIManager();

        createUIManager.SetUIObject();
        createUIManager.InitializeAddListner();
    }

    public void SetBattleUIManager()
    {
        InitializeUIManager();

        charManager = GameObject.FindWithTag("Player").GetComponent<CharacterManager>();
        battleUIManager = GetComponent<BattleUIManager>();
        battleUIManager.SetUIObject();
    }

    public void SetWaitUIManager()
    {
        InitializeUIManager();

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

    

    
