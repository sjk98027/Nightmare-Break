using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IPointerEnterHandler
{
    LoginUIManager loginUIManager;
    WaitUIManager waitUIManager;
    BattleUIManager battleUIManager;

    public LoginUIManager LoginUIManager { get { return loginUIManager; } }
    public WaitUIManager WaitUIManager { get { return waitUIManager; } }
    public BattleUIManager BattleUIManager { get { return battleUIManager; } }

    public GameObject dialogPanel;
    public Text dialog;

    //void Start()
    //{
    //    battleUIManager = new BattleUIManager();
    //    battleUIManager.SetUIObject();
    //}

    public void InitializeManager()
    {
        loginUIManager = gameObject.AddComponent<LoginUIManager>();
        loginUIManager.ManagerInitialize();
        waitUIManager = gameObject.AddComponent<WaitUIManager>();
        waitUIManager.ManagerInitialize();

        //battleUIManager = new BattleUIManager();
        //battleUIManager.SetUIObject();
        SetDialog();
    }

    public void SetDialog()
    {
        dialogPanel = GameObject.Find("DialogPanel");
        dialog = dialogPanel.transform.FindChild("Dialog").GetComponent<Text>();
        dialogPanel.SetActive(false);
    }

    public IEnumerator Dialog(float delay, string text)
    {
        dialogPanel.SetActive(true);

        dialog.text = text;

        yield return new WaitForSeconds(delay);

        dialog.text = "";

        dialogPanel.SetActive(false);
    }

	public void OnPointerEnter (PointerEventData eventData)
	{
		
	}

	public void MouseOverEnter()
	{
		print ("OverIn");
	}
}

    

    
