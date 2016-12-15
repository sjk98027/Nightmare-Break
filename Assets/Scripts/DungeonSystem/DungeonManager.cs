using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Collections.Generic;
using System.Collections;

public enum UnitId
{
    ManWarrior = 0,
    WomanWarrior,
    ManMage,
    WomanMage,
    Frog,
    Duck,
    Rabbit,
    BlackBear,
    Bear,
}

//this class manage monsterStageLevel, sumon, player sumon, player death;
public class DungeonManager : MonoBehaviour
{
    GameObject[] playerSpawnPoints;
	[SerializeField]GameObject[] players;
    [SerializeField]GameObject[] monsterSpawnPoints;
    GameObject[] monsters;
    CharacterManager[] characterData;
	[SerializeField]Monster[] monsterData;

    MonsterSpawnList monsterSpawnList;
    MonsterStatusData dungeonData;

    public SceneChangeObject[] sceneChangeObject;
	public BossMonsterKYW bossMonster;
    //public Section[] section;

    InputManager inputManager;
    UIManager uiManager;
    NetworkManager networkManager;
    GameObject m_camera;

    int mapNumber;

    bool normalMode; //false  -> normalBattle, true -> Defence; 

    public GameObject[] Players { get { return players; } }
    public CharacterManager[] CharacterData { get { return characterData; } }

    public bool NormalMode
    {
		get { return normalMode; }
		set { normalMode = value; }
    }   

	void Start()
	{
        //test
        if (GameObject.FindGameObjectWithTag("GameManager") == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }		

		//Instantiate 스폰포인트 생성조건 - > mapNumber != 2;
		mapNumber = 0;
		normalMode = true;

		//DungeonConstruct();

//        if (GameObject.FindGameObjectWithTag("GameManager") == null)
//        {
//            InitializeMonsterSpawnPoint();
//
//            MonsterSpawnData[] monsterSpawnData = new MonsterSpawnData[3];
//            monsterSpawnData[0] = new MonsterSpawnData((int)MonsterId.Frog, 3);
//            monsterSpawnData[1] = new MonsterSpawnData((int)MonsterId.Duck, 4);
//            monsterSpawnData[2] = new MonsterSpawnData((int)MonsterId.Rabbit, 2);
//            monsterSpawnList = new MonsterSpawnList(3, monsterSpawnData);
//
//            MonsterBaseData[] monsterBaseData = new MonsterBaseData[3];
//            monsterBaseData[0] = new MonsterBaseData((int)MonsterId.Frog, "Frog");
//            monsterBaseData[0].AddLevelData(new MonsterLevelData(1, 2, 0, 30, 5));
//            monsterBaseData[1] = new MonsterBaseData((int)MonsterId.Duck, "Duck");
//            monsterBaseData[1].AddLevelData(new MonsterLevelData(1, 3, 0, 35, 4));
//            monsterBaseData[2] = new MonsterBaseData((int)MonsterId.Rabbit, "Rabbit");
//            monsterBaseData[2].AddLevelData(new MonsterLevelData(1, 5, 0, 75, 4));
//            dungeonData = new MonsterStatusData(3, monsterBaseData);
//
//            SpawnMonster();
//            SetMonsterStatus();
//        }        
		if (GameObject.FindGameObjectWithTag("GameManager") == null)
		{
			//스폰 포인트 찾기
			InitializeMonsterSpawnPoint();

			//서버에서 온 데이터 : monsterSpawnData
			//여기서는 직접 데이터를 설정
			MonsterSpawnData[] monsterSpawnData = new MonsterSpawnData[1];
			monsterSpawnData[0] = new MonsterSpawnData((int)MonsterId.Bear, 1);
			//monsterSpawnData[1] = new MonsterSpawnData((int)MonsterId.BlackBear, 1);

			//던전 데이터에서 몬스터 스폰 리스트를 monsterSpawnData로 설정
			//그러면 몬스터 생성 숫자가 설정됨(종류, 개수)
			monsterSpawnList = new MonsterSpawnList(1, monsterSpawnData);

			//서버에서 온 데이터 : monsterBaseData
			MonsterBaseData[] monsterBaseData = new MonsterBaseData[1];
			monsterBaseData[0] = new MonsterBaseData((int)MonsterId.Bear, "Bear");
			monsterBaseData[0].AddLevelData(new MonsterLevelData(1, 2, 0, 30,1));

			//던전 데이터에서 몬스터 스탯테이블을 monsterBaseData로 설정
			//그러면 위의 몬스터 생성할때 스탯이 이렇게 설정됨
			dungeonData = new MonsterStatusData(1, monsterBaseData);

			SpawnMonster();
			SetMonsterStatus();
		}
	}

	void Update()
	{
        for (int i = 0; i < monsters.Length; i++)
        {
			monsterData[i].MonsterUpdate();
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
        characterData = new CharacterManager[playerNum];
    }

    public void InitializeMonsterSpawnPoint()
    {
        monsterSpawnPoints = GameObject.FindGameObjectsWithTag("MonsterSpawnPoint");
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
			modeForm = true;
        }
    }

    public void SceneChange()
    {
        if (mapNumber < 4)
        {
			mapNumber++;
            SceneManager.LoadScene(mapNumber + 1);// loadScene;
        }
     
    }

    void DungeonConstruct()
    {
		
		//if(mapNumber != 2){
			GameObject spawnpointInstantiate = (GameObject)Instantiate (Resources.Load ("Monster/MonsterSpawnPoint" + mapNumber.ToString()), this.transform.parent);
            //요고의 차일드를 받으세요
			normalMode = true;
			//MonsterSet ();
		//}



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

    public void SetMonsterSpawnList(MonsterSpawnList newMonsterSpawnList)
    {
        monsterSpawnList = newMonsterSpawnList;
    }

    public void SpawnMonster()
    {
        monsters = new GameObject[monsterSpawnList.MonsterNum];
        monsterData = new Monster[monsterSpawnList.MonsterNum];

        int monsterIndex = 0;

        for (int i = 0; i < monsterSpawnList.MonsterKind; i++)
        {
            for (int j = 0; j < monsterSpawnList.MonsterSpawnData[i].MonsterNum; j++)
            {
               monsters[monsterIndex] = CreateMonster(monsterSpawnList.MonsterSpawnData[i].MonsterId, monsterIndex, monsterSpawnPoints[monsterIndex].transform.position);
                monsterData[monsterIndex].MonsterIndex = monsterIndex++;

            }
        }
	}

    public void SetMonsterData(MonsterStatusData newDungeonData)
    {
        dungeonData = newDungeonData;
    }

    public void SetMonsterStatus()
    {
        for (int monsterIndex = 0; monsterIndex < monsterData.Length; monsterIndex++)
        {
            monsterData[monsterIndex].player = players;

            for (int dataIndex = 0; dataIndex < dungeonData.MonsterNum; dataIndex++)
            {
                if ((int) monsterData[monsterIndex].MonsterId == dungeonData.MonsterData[dataIndex].Id)
                {
                    monsterData[monsterIndex].MonsterSet(dungeonData.MonsterData[dataIndex]);
                    break;
                }
			}

			monsterData[monsterIndex].MonsterAIStart(normalMode);
        }

        //for (int i = 0; i < frogMonster.Length; i++)
        //{
        //    frogMonster[i].player = players;
        //    frogMonster[i].MonsterSet(900, 2);
        //    frogMonster[i].MonsterMoveAI(normalMode);
        //    frogMonster[i].MonsterArrayNumber = i;

        //    if (hostGuest != HostGuest.Host)
        //    {
        //        frogMonster[i].GuestMonsterPatternChange();
        //    }
        //    else if (hostGuest == HostGuest.Host)
        //    {
        //        frogMonster[i].MonSterPatternUpdateConduct(normalMode);
        //    }
        //    //frogMonster [i].MonSterPatternUpdateConduct (normalMode);
        //}


        //for (int j = 0; j < duckMonster.Length; j++)
        //{
        //    duckMonster[j].player = players;
        //    duckMonster[j].MonsterSet(900, 2);
        //    duckMonster[j].MonsterMoveAI(normalMode);
        //    duckMonster[j].MonsterArrayNumber = j;
        //    if (hostGuest == HostGuest.Host)
        //    {
        //        duckMonster[j].MonSterPatternUpdateConduct(normalMode);
        //    }
        //    else if (hostGuest != HostGuest.Host)
        //    {
        //        duckMonster[j].GuestMonsterPatternChange();
        //    }
        //}

        //for (int k = 0; k < rabbitMonster.Length; k++)
        //{
        //    rabbitMonster[k].player = players;
        //    rabbitMonster[k].MonsterSet(900, 2);
        //    rabbitMonster[k].MonsterMoveAI(normalMode);
        //    rabbitMonster[k].MonsterArrayNumber = k;
        //    if (hostGuest == HostGuest.Host)
        //    {
        //        rabbitMonster[k].MonSterPatternUpdateConduct(normalMode);
        //    }
        //    else if (hostGuest != HostGuest.Host)
        //    {
        //        rabbitMonster[k].GuestMonsterPatternChange();
        //    }
        //}


        //if (bossMonster != null)
        //{
        //    bossMonster.player = players;
        //    bossMonster.BossMonsterSet(900, 2);
        //    if (hostGuest == HostGuest.Host)
        //    {
        //        bossMonster.BossMonsterPatternUpdateConduct();
        //    }
        //    else if (hostGuest != HostGuest.Host)
        //    {
        //        //bossMonster.
        //        bossMonster.BossMonsterPatternUpdateConduct();
        //    }

        //}
    }

    public GameObject CreateMonster(int unitId, int unitIndex, Vector3 createPoint)
    {
        GameObject monster = null;

        if (unitId == (int)MonsterId.Frog)
        {
            monster = (GameObject)Instantiate(Resources.Load("Monster/Frog"), createPoint, gameObject.transform.rotation);
        }
        else if (unitId == (int)MonsterId.Duck)
        {
            monster = (GameObject)Instantiate(Resources.Load("Monster/Duck"), createPoint, gameObject.transform.rotation);
        }
        else if (unitId == (int)MonsterId.Rabbit)
        {
            monster = (GameObject)Instantiate(Resources.Load("Monster/Rabbit"), createPoint, gameObject.transform.rotation);
        }
        else if(unitId == (int)MonsterId.BlackBear)
        {
            monster = (GameObject)Instantiate(Resources.Load("Monster/BlackBear"), createPoint, gameObject.transform.rotation);

        }
        else if (unitId == (int)MonsterId.Bear)
        {
            monster = (GameObject)Instantiate(Resources.Load("Monster/Bear"), createPoint, gameObject.transform.rotation);

        }

        monster.transform.SetParent(transform);
        monsterData[unitIndex] = monster.GetComponent<Monster>();
        monsterData[unitIndex].MonsterId = (MonsterId)unitId;

        return monster;
    }



    public IEnumerator CheckMapClear()
    {
        while (true)
        {
            yield return null;

            int count = 0;

            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] == null)
                {
                    count++;
                }
            }

            if (count >= monsters.Length)
            {
                break;
            }
        }

        SceneChange();
    }

	//monsterspawnPoint getting
	//public void GetMonsterTransForm(Vector3[] _monsterTransForm){
	//	monsterTransForm = _monsterTransForm;
	//}

    public GameObject CreatePlayer(int characterId)
    {
        //여기서는 플레이어 캐릭터 딕셔너리 -> 각 직업에 따른 플레이어 스탯과 능력치, 스킬, 이름을 가지고 있음
        //딕셔너리를 사용하여 그에 맞는 캐릭터를 소환해야 하지만 Prototype 진행 시에는 고정된 플레이어를 소환하도록 함.

        GameObject player = Instantiate(Resources.Load("ManWarrior")) as GameObject;
        player.name = "ManWarrior";
        player.tag = "Player";
        player.transform.position = Vector3.zero;

        characterData[networkManager.MyIndex] = player.GetComponent<CharacterManager>();
        characterData[networkManager.MyIndex].enabled = true;
        characterData[networkManager.MyIndex].SetUserNum(networkManager.MyIndex);

        players[networkManager.MyIndex] = player;

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

    public void CreateUnit(int unitId, int unitIndex, Vector3 newPosition)
    {
        if (unitId <= (int)UnitId.WomanMage)
        {
            if (players[unitIndex] == null)
            {
                GameObject unit = Instantiate(Resources.Load("ManWarrior")) as GameObject;
                unit.transform.position = newPosition;
                unit.name = "Warrior";
                players[unitIndex] = unit;

                characterData[unitIndex] = unit.GetComponent<CharacterManager>();
                characterData[unitIndex].SetUserNum(unitIndex);
            }
            else
            {
                Debug.Log("이미 있는 캐릭터 인덱스");
            }
        }
        else
        {
            CreateMonster(unitId, unitIndex, monsterSpawnPoints[unitIndex].transform.position);
        }
    }
}