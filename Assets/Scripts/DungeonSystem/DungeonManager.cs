﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


//this class manage monsterStageLevel, sumon, player sumon, player death;
public class DungeonManager : MonoBehaviour
{
	//MonsterController change-> DungeonManager;
	//DungeonScene change -> DungeonManager;
    private const int MaxPlayerNum = 4;
    private GameObject[] players = new GameObject[MaxPlayerNum];
	public GameObject[] Players { get { return players; } }
	public SceneChangeObject nextSceneObject;
	public SceneChangeObject beforeScneObject;

	public int monsterCount;
	public BoomMonster[] boomMonster;
	public WarriroMonster[] warriorMonster;

    InputManager inputManager;
    DataSender dataSender;
    GameObject m_camera;

    public void InitializeManager()
    {
    }

    protected bool modeForm; //false  -> normalBattle, true -> Defence; 
    public bool ModeForm
    {
        get { return modeForm; }
        set { modeForm = value; }
    }

    public Section[] section;

    [SerializeField]
    int mapNumber;

    //defence mode, normal mode
    public void ModeChange(bool modeForm)
    {
        if (!modeForm)
        {
            modeForm = true;
            //player1,player2 ->  nextScene; 
            //respwanstart;
        }

        if (modeForm)
        {
            //(close socket) send to otherclient socket;
        }
    }

    public void SceneChange()
    {
		if(mapNumber<3){
		SceneManager.LoadScene (mapNumber+1);// loadScene;
		}
		//mapNumber == 3 -> 2 player SceneChange

//        if (mapNumber < section.Length)
//        {
//            mapNumber += 1;
//			//SceneManager.LoadScene ();
//        }
//
//        if (mapNumber == section.Length)
//        {
//            ModeChange(modeForm);
//        }
    }

	void DungeonConstruct()
    {
//        for (int i = 0; i <= section.Length - 1; i++)
//        {
//
//            section[i].MonsterSet();
//            section[i].GateNumber = i;
//            section[i].ModeForm = modeForm;
//            //			defenceWave[i].StartDefenceMonsterSet ();
//        }

		MonsterSet ();

		if (mapNumber == 0) {
			nextSceneObject.SceneChangeObjectSet (mapNumber+1);
		}
		else if(mapNumber!= 0 || mapNumber!=4){
			nextSceneObject.SceneChangeObjectSet (mapNumber+1);
			beforeScneObject.SceneChangeObjectSet (mapNumber-1);
		}
		else if (mapNumber == 4) {
			beforeScneObject.SceneChangeObjectSet (mapNumber - 1);
		}
    }

	public void MonsterSet(){
		
		boomMonster = gameObject.transform.GetComponentsInChildren<BoomMonster> ();
		//		warriorMonster = gameObject.GetComponentsInChildren<WarriroMonster> ();
		monsterCount = (boomMonster.Length + warriorMonster.Length);

		for (int i = 0; i < boomMonster.Length; i++) {
			boomMonster [i].PlayerSearch ();
			boomMonster [i].MonsterSet ();
			boomMonster [i].Mode = modeForm;
			boomMonster [i].GateArrayNumber = mapNumber;
			boomMonster [i].MonsterArrayNumber = i;
		}

	}

	public void RemoveMonsterArray(){
		monsterCount -= 1;
		if (monsterCount == 0) {
			SceneChange ();
		}

	}

    void Start()
    {
		DungeonConstruct();//mapNumber - > inspector define
        modeForm = false;
        ModeChange(modeForm);//client get modeform and ingame play ;
    }

    void Update()
    {
        if (!modeForm)
        {
			for (int i = 0; i < boomMonster.Length; i++) {
				boomMonster [i].UpdateConduct ();
				//			warriorMonster [i].UpdateConduct ();
			}

//            for (int i = 0; i < section.Length; i++)
//            {
//                section[i].UpdateConduct(); //this.method change //section [mapNumber].UpdateConduct ();
//            }
        }

        //if (modeForm)
        //{
        //    for (int i = 0; i < defenceWave.Length; i++)
        //    {
        //        defenceWave[i].WaveRespwan();
        //    }
        //}
    }



    public GameObject CreatePlayer(int CharacterId)
    {
        //여기서는 플레이어 캐릭터 딕셔너리 -> 각 직업에 따른 플레이어 스탯과 능력치, 스킬, 이름을 가지고 있음
        //딕셔너리를 사용하여 그에 맞는 캐릭터를 소환해야 하지만 Prototype 진행 시에는 고정된 플레이어를 소환하도록 함.

        GameObject player = Instantiate(Resources.Load("Warrior")) as GameObject;
        player.transform.position = Vector3.zero;
        player.GetComponent<CharacterManager>().enabled = true;
        player.name = "Warrior";
        player.tag = "Player";
        players[0] = player;

        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        StartCoroutine(m_camera.GetComponent<CameraController>().CameraCtrl(player.transform));

        dataSender = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<DataSender>();
        dataSender.CreateUnitSend(0, player.transform.position);

        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        inputManager.InitializeManager();
        StartCoroutine(inputManager.GetKeyInput());

        return player;
    }

    public GameObject CreateUnit(int unitId, Vector3 newPosition)
    {
        //위와 같은 생성이지만 이곳에서는 다른 플레이어의 캐릭터를 생성한다.
        //DataHandler 에서 데이타를 받아서 실행된다.

        GameObject unit = Instantiate(Resources.Load("Warrior")) as GameObject;
        unit.transform.position = newPosition;
        unit.name = "Warrior";

        players[1] = unit;

        return unit;
    }
}