using UnityEngine;

public class UIManager : MonoBehaviour
{
    LoginUIManager loginUIManager;
    WaitUIManager waitUIManager;
    BattleUIManager battleUIManager;

    public LoginUIManager LoginUIManager { get { return loginUIManager; } }
    public WaitUIManager WaitUIManager { get { return waitUIManager; } }
    public BattleUIManager BattleUIManager { get { return battleUIManager; } }

    void Start()
    {
        battleUIManager = new BattleUIManager();
        battleUIManager.SetUIObject();
    }

    void OnMouseOver()
    {
      

    }

    void OnMouseExit()
    {

    }
}
