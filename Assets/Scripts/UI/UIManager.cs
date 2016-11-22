using UnityEngine;

public class UIManager : MonoBehaviour
{
    LoginUIManager loginUIManager;
    WaitUIManager waitUIManager;
    BattleUIManager battleUIManager;

    void Start()
    {
       battleUIManager = gameObject.GetComponent<BattleUIManager>();
    }

    public LoginUIManager LoginUIManager { get { return loginUIManager; } }
    public WaitUIManager WaitUIManager { get { return waitUIManager; } }
    public BattleUIManager BattleUIManager { get { return battleUIManager; } }


}
