using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Collections.Generic;
using System.Collections;


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
    [SerializeField]
    private GameObject[] players;
    public CharacterManager[] characters;
    public GameObject[] Players { get { return players; } }
    public SceneChangeObject[] sceneChangeObject;

    public int monsterCount;
	public Frog[] frogMonster;
    public Rabbit[] rabbitMonster;
	public Duck[] duckMonster;
	public BossMonsterKYW bossMonster;

	public MonsterSpawnPoint spawnPoint;


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



	void Start()
	{

		//test
		players = GameObject.FindGameObjectsWithTag ("Player");

		//Instantiate 스폰포인트 생성조건 - > mapNumber != 2;
		mapNumber = 0;


		DungeonConstruct();//mapNumber - > inspector define
		//        modeForm = false;
//		        ModeChange(n);//client get modeform and ingame play ;
		//section = transform.GetComponentsInChildren<Section> ();



	





	}

	void Update()
	{
		if (normalMode)
		{
			for (int i = 0; i < frogMonster.Length; i++) {
				if (hostGuest != HostGuest.Host) {
					frogMonster [i].GuestMonsterUpdate ();	
				}
				if (hostGuest == HostGuest.Host) {
					frogMonster [i].UpdateNormalMode ();
				}
			}
			for (int j = 0; j < rabbitMonster.Length; j++) {
				if (hostGuest != HostGuest.Host) {
					rabbitMonster [j].GuestMonsterUpdate ();
				}
				if (hostGuest == HostGuest.Host) {
					rabbitMonster [j].UpdateNormalMode ();
				}
				
			}
			for (int k = 0; k < duckMonster.Length; k++) {
				if (hostGuest != HostGuest.Host) {
					duckMonster [k].GuestMonsterUpdate ();
						
				}
				if (hostGuest == HostGuest.Host) {
					duckMonster [k].UpdateNormalMode ();
				}
			}
		}

		if (!normalMode)
		{
			if (hostGuest == HostGuest.Host) {
				for (int i = 0; i < section.Length; i++) {
					section [i].HostUpdateConduct ();
				}
			}

			if (hostGuest != HostGuest.Guest) {
				for (int j = 0; j < section.Length; j++) {
					section [j].GuestUpdateConduct ();
				}
			}
		}


	}

    //각종 매니저 초기화
    public void Initialize(int userNum)
    {
        Debug.Log("DungeonManager 초기화 - " + userNum);
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    public void InitializePlayer(int playerNum)
    {
        players = new GameObject[playerNum];
        characters = new CharacterManager[playerNum];
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
        if (mapNumber < 4)
        {
			mapNumber++;
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
		
		if(mapNumber != 2){
			GameObject spawnpointInstantiate = (GameObject)Instantiate (Resources.Load ("Monster/MonsterSpawnPoint" + mapNumber.ToString()), this.transform.parent);
			spawnPoint = spawnpointInstantiate.GetComponent<MonsterSpawnPoint>();

			spawnPoint.SpawnMonsterGetting ();
			frogMonster = new Frog[spawnPoint.FrogCount];
			duckMonster = new Duck[spawnPoint.DuckCount];
			rabbitMonster = new Rabbit[spawnPoint.RabbitCount];
			normalMode = true;
			MonsterSet ();
		}




		if (mapNumber == 2) {
			normalMode = false;
			section = new Section[3];
			GameObject SectionInstantiate0 = (GameObject)Instantiate (Resources.Load("Monster/Section"),transform.parent);
			GameObject SectionInstantiate1 = (GameObject)Instantiate (Resources.Load("Monster/Section"),transform.parent);
			GameObject SectionInstantiate2 = (GameObject)Instantiate (Resources.Load("Monster/Section"),transform.parent);
			section [0] = SectionInstantiate0.GetComponent<Section> ();
			section [1] = SectionInstantiate1.GetComponent<Section> ();
			section [2] = SectionInstantiate2.GetComponent<Section> ();


			for (int i = 0; i < section.Length; i++) {
				section [i].MonsterSet ();
				section [i].GateNumber = i;
				section [i].SetFalse ();
			}
			//StartCoroutine (section ());
		}


        //if (mapNumber == 0) {
//        	nextSceneObject.SceneChangeObjectSet (mapNumber+1);
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
		monsterCount = 0;

		for (int i = 0; i < spawnPoint.FrogCount; i++) {
			GameObject objfrogMonster = (GameObject)Instantiate (Resources.Load ("Monster/Frog"), spawnPoint.spawnVector[i] ,this.gameObject.transform.rotation);
			objfrogMonster.transform.SetParent (this.transform);
			frogMonster [i] = objfrogMonster.GetComponent<Frog> ();
		}
		for (int i = 0; i < spawnPoint.DuckCount; i++) {
			GameObject objduckMonster= (GameObject)Instantiate (Resources.Load ("Monster/Duck"), spawnPoint.spawnVector[i+spawnPoint.FrogCount], this.transform.rotation);
			objduckMonster.transform.SetParent (this.transform);
			duckMonster [i] = objduckMonster.GetComponent<Duck> (); 
		}
		for (int i = 0; i < spawnPoint.RabbitCount; i++) {
			GameObject objRabbit= (GameObject)Instantiate (Resources.Load ("Monster/Rabbit"), spawnPoint.spawnVector[i+spawnPoint.FrogCount+spawnPoint.DuckCount], this.transform.rotation);
			objRabbit.transform.SetParent (this.transform);
			rabbitMonster[i]= objRabbit.GetComponent<Rabbit> ();
		}

		if (bossMonster != null) {
			GameObject objBossMonster = (GameObject)Instantiate (Resources.Load ("Monster/Bear"), new Vector3 (0, 0, 0), this.transform.rotation);
			Debug.Log ("in boss");
			bossMonster = objBossMonster.GetComponent<BossMonsterKYW> ();
			monsterCount++;
		}

		monsterCount += spawnPoint.sumMonsterCount;

		for (int i = 0; i < frogMonster.Length; i++) {
			frogMonster [i].player = players;
			frogMonster [i].MonsterSet (900,2);
			frogMonster [i].MonsterMoveAI (normalMode);
			frogMonster [i].MonsterArrayNumber = i;

			if (hostGuest != HostGuest.Host) {
				frogMonster [i].GuestMonsterPatternChange ();
			} else if (hostGuest == HostGuest.Host) {
				frogMonster [i].MonSterPatternUpdateConduct (normalMode);
			}
				//frogMonster [i].MonSterPatternUpdateConduct (normalMode);
		}


		for (int j = 0; j < duckMonster.Length; j++) {
			duckMonster [j].player = players;
			duckMonster [j].MonsterSet (900, 2);
			duckMonster [j].MonsterMoveAI (normalMode);
			duckMonster [j].MonsterArrayNumber = j;
			if (hostGuest == HostGuest.Host) {
				duckMonster [j].MonSterPatternUpdateConduct (normalMode);
			} else if (hostGuest != HostGuest.Host) {
				duckMonster [j].GuestMonsterPatternChange ();
			}
		}

		for (int k = 0; k < rabbitMonster.Length; k++) {
			rabbitMonster [k].player = players;
			rabbitMonster [k].MonsterSet (900, 2);
			rabbitMonster [k].MonsterMoveAI (normalMode);
			rabbitMonster [k].MonsterArrayNumber = k;
			if (hostGuest == HostGuest.Host) {
				rabbitMonster [k].MonSterPatternUpdateConduct (normalMode);
			} else if (hostGuest != HostGuest.Host) {
				rabbitMonster [k].GuestMonsterPatternChange ();
			}
		}


		if (bossMonster != null) {
			bossMonster.player = players;
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

    

	IEnumerator SectionSet(){
		for (int i = 0; i < section.Length; i++) {
			section [i].SetTrue ();
			new WaitForSeconds (10f);
		}
		return null;


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
        player.name = "Warrior";
        player.tag = "Player";
        player.transform.position = Vector3.zero;

        characters[networkManager.MyIndex] = player.GetComponent<CharacterManager>();
        characters[networkManager.MyIndex].enabled = true;
        characters[networkManager.MyIndex].SetUserNum(networkManager.MyIndex);

        players[networkManager.MyIndex] = player;

        Debug.Log("캐릭터 생성 번호 : " + networkManager.MyIndex);

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

        for (int index = 0; index < networkManager.UserIndex.Count; index++)
        {
            if (networkManager.MyIndex != networkManager.UserIndex[index].UserNum)
            {
                DataSender.Instance.CreateUnitSend(networkManager.UserIndex[index].EndPoint, (short)characterId, player.transform.position.x, player.transform.position.y, player.transform.position.z);
            }
        }

        return player;
    }

    public GameObject CreateUnit(int unitId, int unitIndex, Vector3 newPosition)
    {
        //위와 같은 생성이지만 이곳에서는 다른 플레이어의 캐릭터를 생성한다.
        //DataHandler 에서 데이타를 받아서 실행된다.
        if (players[unitIndex] == null)
        {
            GameObject unit = Instantiate(Resources.Load("Warrior")) as GameObject;
            unit.transform.position = newPosition;
            unit.name = "Warrior";
            players[unitIndex] = unit;

            characters[unitIndex] = unit.GetComponent<CharacterManager>();
            characters[unitIndex].SetUserNum(unitIndex);

            return unit;
        }
        else
        {
            Debug.Log("이미 있는 캐릭터 인덱스");
            return null;
        }        
    }
}