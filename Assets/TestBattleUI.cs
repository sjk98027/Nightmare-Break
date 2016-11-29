using UnityEngine;
using System.Collections;

public class TestBattleUI : MonoBehaviour {

    BattleUIManager battleUI;

	// Use this for initialization
	void Start () {
        battleUI = new BattleUIManager();
        battleUI.SetUIObject();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
