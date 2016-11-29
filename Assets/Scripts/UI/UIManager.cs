using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	public CharacterManager charManager;
    LoginUIManager loginUIManager;
    WaitUIManager waitUIManager;
    BattleUIManager battleUIManager;

    public LoginUIManager LoginUIManager { get { return loginUIManager; } }
    public WaitUIManager WaitUIManager { get { return waitUIManager; } }
    public BattleUIManager BattleUIManager { get { return battleUIManager; } }

    public GameObject dialogPanel;
    public Text dialog;

    public void SetLoginUIManager()
    {
        loginUIManager = new LoginUIManager();
        loginUIManager.ManagerInitialize();
    }

    public void SetBattleUIManager()
    {
        charManager = GameObject.FindWithTag("Player").GetComponent<CharacterManager>();
        charManager.UIManager = GetComponent<UIManager>();
        battleUIManager = new BattleUIManager();
		battleUIManager.SetUIObject();
    }

    public void SetWaitUIManager()
    {
        waitUIManager = new WaitUIManager();
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
        dialogPanel.SetActive(true);

        dialog.text = text;

        yield return new WaitForSeconds(delay);

        dialog.text = "";

        dialogPanel.SetActive(false);
    }
		 
	public void OnPointEnter(int skillIndex)
	{
		battleUIManager.SetPointEnterUI (skillIndex, 2, (int)charManager.CharStatus.HClass);
	}
	 
	public void OnPointExit()
	{
		battleUIManager.MouseOverUI.gameObject.transform.parent.gameObject.SetActive (false);
	}
}

    

    
