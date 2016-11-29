using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


//this class manage monsterStageLevel, sumon, player sumon, player death;
public class DungeonManager : MonoBehaviour
{
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
    public WarriroMonster[] warriorMonster;

    InputManager inputManager;
    DataSender dataSender;
    GameObject m_camera;

    protected bool normalMode; //false  -> normalBattle, true -> Defence; 
	public bool NormalMode
    {
		get { return normalMode; }
		set { normalMode = value; }
    }

    public Section[] section;



    [SerializeField]int mapNumber;

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
			for (int i = 0; i < boomMonster.Length; i++)
			{
				boomMonster[i].UpdateNormalMode();
//				warriorMonster [i].UpdateNormalMode ();
			}
		}

		if (!normalMode)
		{
			for (int i = 0; i < section.Length; i++) {
				section [i].UpdateConduct ();
			}
		}
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
        //	beforeScneObject.SceneChangeObjectSet (mapNumber - 1);
        //}

    }

    public void MonsterSet()
    {

        boomMonster = gameObject.transform.GetComponentsInChildren<BoomMonster>();
        //		warriorMonster = gameObject.GetComponentsInChildren<WarriroMonster> ();
        monsterCount = (boomMonster.Length + warriorMonster.Length);

        for (int i = 0; i < boomMonster.Length; i++)
        {
            boomMonster[i].PlayerSearch();
            boomMonster[i].MonsterSet();
			boomMonster[i].NormalMode = normalMode;
            boomMonster[i].GateArrayNumber = mapNumber;
            boomMonster[i].MonsterArrayNumber = i;
			boomMonster [i].MonSterPatternUpdateConduct (normalMode);
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

        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        inputManager.InitializeManager();
        StartCoroutine(inputManager.GetKeyInput());
        StartCoroutine(dataSender.CharacterPositionSend());
		StartCoroutine(dataSender.EnqueueMessage());

        dataSender = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<DataSender>();
        dataSender.CreateUnitSend(0, player.transform.position);

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