using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Collections.Generic;


//this class manage monsterStageLevel, sumon, player sumon, player death;
public class DungeonManager : MonoBehaviour
{
	//need revise code;
	public enum HostGuest
	{
		Host = 0,
		SubHost,
		Guest
	}
	;
	public HostGuest hostGuest;
    //MonsterController change-> DungeonManager;
    //DungeonScene change -> DungeonManager;
    private const int MaxPlayerNum = 4;
    [SerializeField]
    private GameObject[] players = new GameObject[MaxPlayerNum];
    public GameObject[] Players { get { return players; } }
    public SceneChangeObject nextSceneObject;
    public SceneChangeObject beforeScneObject;

    public int monsterCount;
    public BoomMonster[] boomMonster;
    public WarriorMonster[] warriorMonster;
	public ShockWaveMonster[] shockWaveMonster;
	public BossMonsterKYW bossMonster;

    InputManager inputManager;
    UIManager uiManager;
    NetworkManager networkManager;
    GameObject m_camera;

    protected bool normalMode; //false  -> normalBattle, true -> Defence; 
	public bool NormalMode
    {
		get { return normalMode; }
		set { normalMode = value; }
    }

	public Vector3[] monsterTransForm;

    public Section[] section;

    [SerializeField]int mapNumber;


	//monsterSpawnPoint -> dungeonManager monsterinstantiate count send;
	void Start()
	{
		DungeonConstruct();//mapNumber - > inspector define
		//        modeForm = false;
//		        ModeChange(n);//client get modeform and ingame play ;
		//section = transform.GetComponentsInChildren<Section> ();
		normalMode = true;
		section = transform.GetComponentsInChildren<Section> ();
		if(section.Length != 0){
			SectionSet ();
		}

	}

	void Update()
	{
		if (normalMode)
		{
			for (int i = 0; i < boomMonster.Length; i++) {
				if (hostGuest != HostGuest.Host) {
					boomMonster [i].GuestMonsterUpdate ();	
				}
				if (hostGuest == HostGuest.Host) {
					boomMonster [i].UpdateNormalMode ();
				}
			}
			for (int j = 0; j < warriorMonster.Length; j++) {
				if (hostGuest != HostGuest.Host) {
					warriorMonster [j].GuestMonsterUpdate ();
				}
				if (hostGuest == HostGuest.Host) {
					warriorMonster [j].UpdateNormalMode ();
				}
				
			}
			for (int k = 0; k < shockWaveMonster.Length; k++) {
				if (hostGuest != HostGuest.Host) {
					shockWaveMonster [k].GuestMonsterUpdate ();
						
				}
				if (hostGuest == HostGuest.Host) {
					shockWaveMonster [k].UpdateNormalMode ();
				}
			}
		}

		if (!normalMode)
		{
			for (int i = 0; i < section.Length; i++) {
				section [i].UpdateConduct ();
			}
		}

		if (bossMonster != null) {
			if(hostGuest == HostGuest.Host){
				bossMonster.BossMonsterUpdate ();
			}
			if(hostGuest != HostGuest.Host){
				//shockWaveMonster [k].GuestMonsterUpdate ();
				bossMonster.BossMonsterUpdate ();
			}
		}
	}

    //각종 매니저 초기화
    public void Initialize()
    {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    //defence mode, normal mode
    public void ModeChange(bool modeForm)
    {
		if (normalMode)
        {
			modeForm = false;
            //player1,player2 ->  nextScene; 
            //respwanstart;
        }

        if (!modeForm)
        {
            //(close socket) send to otherclient socket;
        }
    }

    public void SceneChange()
    {
        if (mapNumber < 3)
        {
            SceneManager.LoadScene(mapNumber + 1);// loadScene;
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
     
		normalMode = true;
        MonsterSet();

        //if (mapNumber == 0) {
        //	nextSceneObject.SceneChangeObjectSet (mapNumber+1);
        //}
        //else if(mapNumber!= 0 || mapNumber!=4){
        //	nextSceneObject.SceneChangeObjectSet (mapNumber+1);
        //	beforeScneObject.SceneChangeObjectSet (mapNumber-1);
        //}
        //else if (mapNumber == 4) {
        //	beforeScneObject.SceneChangeObjec  tSet (mapNumber - 1);
        //}

    }

    public void MonsterSet()
    {
		


		//Instantiate(Resources.Load("MonsterPrefebs/"+boomMonster.ToString()));
		{
			boomMonster = gameObject.transform.GetComponentsInChildren<BoomMonster> ();
			shockWaveMonster = gameObject.transform.GetComponentsInChildren<ShockWaveMonster> ();
			warriorMonster = gameObject.GetComponentsInChildren<WarriorMonster> ();
			monsterCount = (boomMonster.Length + warriorMonster.Length + shockWaveMonster.Length);
		}


			for (int i = 0; i < boomMonster.Length; i++) {
				if (boomMonster.Length != 0) {
					boomMonster [i].PlayerSearch ();
					boomMonster [i].MonsterSet (900,2);
					boomMonster [i].NormalMode = normalMode;
					boomMonster [i].GateArrayNumber = mapNumber;
					boomMonster [i].MonsterArrayNumber = i;
					if (hostGuest != HostGuest.Host) {
						boomMonster [i].GuestMonsterPatternChange ();
					}
					else if (hostGuest == HostGuest.Host) {
						boomMonster [i].MonSterPatternUpdateConduct (normalMode);
					}
					//boomMonster [i].MonSterPatternUpdateConduct (normalMode);
				}
			}


			for (int j = 0; j < shockWaveMonster.Length; j++) {
				if (shockWaveMonster.Length != 0) {
					shockWaveMonster [j].PlayerSearch ();
					shockWaveMonster [j].MonsterSet (900,2);
					shockWaveMonster [j].NormalMode = normalMode;
					shockWaveMonster [j].GateArrayNumber = mapNumber;
					shockWaveMonster [j].MonsterArrayNumber = j;
					if (hostGuest == HostGuest.Host) {
						shockWaveMonster [j].MonSterPatternUpdateConduct (normalMode);
					}
					else if (hostGuest != HostGuest.Host) {
						shockWaveMonster [j].GuestMonsterPatternChange ();
					}
				}
			}

			for (int k = 0; k < warriorMonster.Length; k++) {
				if (warriorMonster.Length != 0) {
					warriorMonster [k].PlayerSearch ();
					warriorMonster [k].MonsterSet (900,2);
					warriorMonster [k].NormalMode = normalMode;
					warriorMonster [k].GateArrayNumber = mapNumber;
					warriorMonster [k].MonsterArrayNumber = k;
					if (hostGuest == HostGuest.Host) {
						warriorMonster [k].MonSterPatternUpdateConduct (normalMode);
					}
					else if (hostGuest != HostGuest.Host) {
						warriorMonster [k].GuestMonsterPatternChange ();
					}
				}
			}


			if (bossMonster != null) {
				bossMonster.PlayerSearch ();
				bossMonster.BossMonsterSet (900, 2);
				if (hostGuest == HostGuest.Host) {
					bossMonster.BossMonsterPatternUpdateConduct ();
				}
				else if (hostGuest != HostGuest.Host) {
					//bossMonster.
					bossMonster.BossMonsterPatternUpdateConduct ();
				}

			}
		}

    

	public void SectionSet(){
		
		for (int i = 0; i < section.Length; i++) {
			section [i].MonsterSet ();
			section [i].GateNumber = i;
		}
	}

    public void RemoveMonsterArray()
    {
        monsterCount -= 1;
        if (monsterCount == 0)
        {
            SceneChange();
        }

    }

	//monsterspawnPoint getting
	public void GetMonsterTransForm(Vector3[] _monsterTransForm){
		monsterTransForm = _monsterTransForm;
	}

    
    public GameObject CreatePlayer(int characterId)
    {
        //여기서는 플레이어 캐릭터 딕셔너리 -> 각 직업에 따른 플레이어 스탯과 능력치, 스킬, 이름을 가지고 있음
        //딕셔너리를 사용하여 그에 맞는 캐릭터를 소환해야 하지만 Prototype 진행 시에는 고정된 플레이어를 소환하도록 함.

        GameObject player = Instantiate(Resources.Load("Warrior")) as GameObject;
        player.transform.position = Vector3.zero;
        player.GetComponent<CharacterManager>().enabled = true;
        player.name = "Warrior";
        player.tag = "Player";
        players[0] = player;
        player.GetComponent<CharacterManager>().SetUserNum(0);

        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        StartCoroutine(m_camera.GetComponent<CameraController>().CameraCtrl(player.transform));

        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        inputManager.InitializeManager();
        StartCoroutine(inputManager.GetKeyInput());

        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //uiManager.SetBattleUIManager();
        player.GetComponent<CharacterManager>().UIManager = uiManager;
        player.GetComponent<CharacterManager>().SetCharacterStatus();
        player.GetComponent<CharacterManager>().SetCharacterType();

        foreach (KeyValuePair<EndPoint, int> user in networkManager.DataHandler.userNum)
        {
            DataSender.Instance.CreateUnitSend(user.Key, (short)characterId, player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }

        return player;
    }

    public GameObject CreateUnit(int unitId, int unitIndex, Vector3 newPosition)
    {
        //위와 같은 생성이지만 이곳에서는 다른 플레이어의 캐릭터를 생성한다.
        //DataHandler 에서 데이타를 받아서 실행된다.

        GameObject unit = Instantiate(Resources.Load("Warrior")) as GameObject;
        unit.transform.position = newPosition;
        unit.name = "Warrior";
        unit.GetComponent<CharacterManager>().SetUserNum(unitIndex + 1);

        players[unitIndex + 1] = unit;

        return unit;
    }
}