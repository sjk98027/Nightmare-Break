using UnityEngine;
using System.Collections;

public enum StatePosition
{
	Idle=1,
	Run,
	Attack,
	TakeDamage,
	Death,
	BossOneHandAttack,
	BossJumpAttack,
	BossRoar,
}



public class Monster : MonoBehaviour {
    public enum StateDirecion
    {
        right = 0,
        left
    };

    protected Animator animator;
    protected AnimatorStateInfo aniState;
	[SerializeField]protected BoxCollider HittedBox;
	[SerializeField]protected MonsterWeapon attackCollider;
	[SerializeField]protected ShockWave shockWaveInstantiate;

    public GameObject[] player;
	[SerializeField]protected GameObject targetPlayer;
    protected Vector3 movePoint;

    protected int monsterRunAttackAround;

	protected int randomStandby;
	//public int RandomStandby;
	//mode,gateArraynumber,monsterArraynumber
	protected bool moveAble;

    [SerializeField] protected int monsterIndex;
    [SerializeField] protected MonsterId monsterId;
    [SerializeField] protected string _name;
    [SerializeField] protected int level;
    [SerializeField] protected int currentHP;
	[SerializeField] protected int maxHP;
    [SerializeField] protected int attack;
    [SerializeField] protected int defense;
    [SerializeField] protected int moveSpeed;

    //monster getting variable;
    protected float RunRange;
    protected float attackRange;
    protected float attackCycle;
	[SerializeField]protected float currentDisTance;
	[SerializeField]protected float searchRange;

    //monster Speed variable;

    StateDirecion stateDirecion;
	StatePosition statePosition;
    protected bool isAlive;
	protected bool isAttack;
	protected bool isHited;

    protected float[] playerToMonsterDamage;
	private float[] aggroRank; //playertoMonsterdamage/currentdistancePlayer;
    private float changeTargetTime=0;

	[SerializeField]private float[]currentDisTanceArray;
	protected Vector3 checkDirection; // monster chaseplayer and move variable;
	protected Vector3[] pointVector;
	protected Vector3 transitionVector;
    
    public int MonsterIndex
    {
        get { return monsterIndex; }
        set { monsterIndex = value; }
    }

    public bool MoveAble
    {
        get { return moveAble; }
        set { moveAble = value; }
    }
    public bool IsAttack{
		get{ return isAttack;}
		set{ isAttack = value;}
	}
	public bool IsAlive{
		get{ return isAlive;}
		set{ isAlive = value;}
	}
	public bool IsHited{
		get{ return isHited;}
		set{ isHited = value;}
	}
    public StateDirecion _StateDirecion { get { return stateDirecion; } }
	public StatePosition _StatePosition { get { return statePosition; } }
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    
    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

    public int Attack { get { return attack; } }
    public MonsterId MonsterId { get { return monsterId; } set { monsterId = value; } }

    public void MonsterSet(MonsterBaseData monster)
	{
        animator = this.gameObject.GetComponent<Animator>();
        HittedBox = this.gameObject.GetComponent<BoxCollider>();


        isAlive = true;
        isHited = false;
        moveAble = true;

        _name = monster.Name;
		//monsterId = monster.Id; //->인식 안됨
        level = monster.MonsterLevelData[0].Level;
        attack = monster.MonsterLevelData[0].Attack;
        defense = monster.MonsterLevelData[0].Defense;
        currentHP = monster.MonsterLevelData[0].HealthPoint;
        maxHP = monster.MonsterLevelData[0].HealthPoint;
        moveSpeed = monster.MonsterLevelData[0].MoveSpeed;


		if (monster.Id == (int)MonsterId.Rabbit || monster.Id == (int)MonsterId.Frog) {
			searchRange = 8;
			attackCollider = this.transform.GetComponentInChildren<MonsterWeapon> ();
			attackCollider.MonsterWeaponSet ();
		}


		if(monster.Id == (int)MonsterId.Duck){
			//GameObject obj = Instantiate (Resources.Load("Effect/Monster_ShockWave"));
			searchRange = 12;
			//shockWaveInstantiate
		}


		//searchRange = 12;

		//test
		DungeonManager dungeonManager = GameObject.Find("DungeonManager").GetComponent<DungeonManager>();
		player = dungeonManager.Players;

        currentDisTanceArray = new float[player.Length];
		aggroRank = new float[player.Length];
		playerToMonsterDamage = new float[player.Length];
        
		//if(monsterId == UnitId.Frog || monsterId == UnitId.Rabbit){ // change
		if (attackCollider != null) {
			attackCollider = this.transform.GetComponentInChildren<MonsterWeapon> ();
			attackCollider.MonsterWeaponSet ();
		}
		if (shockWaveInstantiate != null) {
			shockWaveInstantiate.GetDamage (attack, this.gameObject.GetComponent<Duck> ());
		}
		if(shockWaveInstantiate != null|| attackCollider !=null){
//			NormalMonsterRealizePattern ();
			//bossmonsterWeapon damageGet(basedamage);
		}
        
        StartCoroutine(LookatChange());
    }

	public void MonsterMoveAI(bool _normalMode){//moveAI->StartAI , 
		_normalMode =true;
		StartCoroutine(MonsterMoveAIStart (_normalMode));
		StartCoroutine(MonsterActAI (_normalMode));

//		if (monsterId == UnitId.Frog || monsterId == UnitId.Rabbit || monsterId == UnitId.Duck ||monsterId == UnitId.ManWarrior) {
//			
//		}
//		if (!_normalMode) {
//			StartCoroutine(DefencePointVectorChange ());
//		}
	}

	IEnumerator DefencePointVectorChange(){
		while (true)
		{
			for (int i = 0; i < pointVector.Length; i++)
			{
				if (i > 0 && i < pointVector.Length - 1)
				{
					transitionVector = pointVector[i];
					pointVector[i] = pointVector[i + 1];
					pointVector[i + 1] = transitionVector;
				}

				if (i == pointVector.Length - 1)
				{
					transitionVector = pointVector[i];
					pointVector[i] = pointVector[0];
					pointVector[0] = transitionVector;
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator MonsterMoveAIStart(bool _normalMode){
		while (IsAlive) {
			//if (monsterId == UnitId.Frog || monsterId == UnitId.Rabbit || monsterId == UnitId.Duck) {
			if(_normalMode){
					if (targetPlayer != null) {
						if (Mathf.Abs(targetPlayer.transform.position.z-transform.position.z) >8 || Mathf.Abs(targetPlayer.transform.position.x-this.gameObject.transform.position.x) > 0.6f )
							if (currentDisTance > searchRange * 0.3f){
							randomStandby = Random.Range(0,3);
							if (randomStandby == 0) {
								//for 문 -> Fuck go;
								if (checkDirection.z>0) {
									movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z-3f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z-3f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z-3f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z-3f);
									yield return new WaitForSeconds (2f);
								}
								if (checkDirection.z<0) {
									movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z+3f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z+3f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z+3f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z+3f);
									yield return new WaitForSeconds (2f);
								}
							}

							if (randomStandby == 1) {

								int a = Random.Range (0, 4);
								if (a == 0) {
									movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x+1.5f, 0, checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z-1.5f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x-1.5f, 0, checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z+1.5f);
									yield return new WaitForSeconds (2f);
								}
								if (a == 1) {
									movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x+1.5f, 0, checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z+1.5f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x-1.5f, 0, checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z-1.5f);
									yield return new WaitForSeconds (2f);
								}
								if (a == 2) {
									movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x-1.5f, 0, checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z-1.5f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x+1.5f, 0, checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z+1.5f);
									yield return new WaitForSeconds (2f);
								}
								if (a == 3) {
									movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x-1.5f, 0, checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z+1.5f);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x+1.5f, 0, checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z-1.5f);
									yield return new WaitForSeconds (2f);
								}
							}
							if (randomStandby == 2) {
								int a = Random.Range (0, 4);
								if (a == 0) {
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
								}
								if (a == 1) {
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
								}
								if (a == 2) {
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
								}
								if (a == 3) {
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (1f);
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (1f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (1f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (1f);
								}
							}
							yield return new WaitForSeconds (0.2f);
						} else if(Mathf.Abs(targetPlayer.transform.position.z-transform.position.z) <8 && Mathf.Abs(targetPlayer.transform.position.x-this.gameObject.transform.position.x) <= 0.6f ) {
							aniState = this.animator.GetCurrentAnimatorStateInfo (0);
							if (!aniState.IsName ("Attack")) {
								if (moveAble) {
									if (checkDirection.z > 0) {
										if (checkDirection.x > 0) {
											movePoint = new Vector3 (-checkDirection.x, 0, 0);
										}
										if (checkDirection.x < 0) {
											movePoint = new Vector3 (checkDirection.x, 0, 0);
										}
									}
									if (checkDirection.z <= 0) {
										if (checkDirection.x > 0) {
											movePoint = new Vector3 (-checkDirection.x, 0, 0);
										}
										if (checkDirection.x < 0) {
											movePoint = new Vector3 (checkDirection.x, 0, 0);
										}
									}
								}
							}
							
						}
				yield return new WaitForSeconds (2f);}
				else
						yield return new WaitForSeconds (3f);
				}
				else if(!IsAlive){

					yield return false;
				}

			//}


			while (!_normalMode) {
				
				}
			yield return new WaitForSeconds (0.2f);
		}
	}
	
	public void Pattern(StatePosition state){
		switch (state)
		{
		case StatePosition.Idle:
			{
				//this.transform.Translate(idlePoint * Time.deltaTime, 0);
				animator.SetInteger("State", 0);
				break;
			}

		case StatePosition.Attack:
			{
				AttackProcess(isAttack);
				break;
			}
		case StatePosition.Run:
			{
				AnimatorReset();
				animator.SetInteger("State", 2);
				break;
			}
		case StatePosition.TakeDamage:
			{
				animator.SetTrigger ("TakeDamage");
				break;
			}
		case StatePosition.Death:
			{
				animator.SetTrigger ("Death");

				//				MonsterArrayEraser(this.gameObject);
				break;
			}
		}
	}



	IEnumerator MonsterActAI(bool _normalMode){
		while (_normalMode) {
			if (isAlive) {
				if (targetPlayer != null) {	
					currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
					checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;

					if (Mathf.Abs (targetPlayer.transform.position.z - transform.position.z) < 8 && Mathf.Abs (targetPlayer.transform.position.x - this.gameObject.transform.position.x) <= 0.6f) {
						if (!isAttack) {
							isAttack = true;
							moveAble = false;
						}
						statePosition = StatePosition.Attack;
						if (checkDirection.z > 0) {
							LookAtPattern (StateDirecion.right);
						}
						if (checkDirection.z < 0) {
							LookAtPattern (StateDirecion.left);
						}
						Pattern (statePosition);
						yield return new WaitForSeconds (0.5f);

					} else if (currentDisTance > searchRange) {

						moveAble = false;
						isAttack = false;
						statePosition = StatePosition.Idle;
						Pattern (statePosition);

					} else {
						moveAble = true;
						isAttack = false;
						statePosition = StatePosition.Run;
						Pattern (statePosition);
					}

				}
				yield return new WaitForSeconds (0.2f);
			} else
				yield return new WaitForSeconds (0.2f);
		}
		while (!_normalMode) {
			yield return new WaitForSeconds (0.2f);
		}
	}


	public void AttackProcess(bool isAttack){
		if (isAttack) {

			if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Run")){
				animator.SetInteger ("State", 0);
			}
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Idle")) {
				animator.SetInteger ("State", 3);
			}
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Attack")) {
				moveAble = false;
			}
		}
	}

    public void LookAtPattern(StateDirecion state)
    {
        switch (state)
        {
            case StateDirecion.right:
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    stateDirecion = StateDirecion.right; break;
                }
            case StateDirecion.left:
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    stateDirecion = StateDirecion.left; break;
                }
        }
    }

	IEnumerator LookatChange(){
		while (true) {
			if (!isAttack) {
				aniState = this.animator.GetCurrentAnimatorStateInfo (0);
				if (targetPlayer != null) {
					if (aniState.IsName ("Run")||aniState.IsName("Idle")){
						if (movePoint.z > 0) {
							LookAtPattern (StateDirecion.right);
						} else if (movePoint.z < 0) {
							LookAtPattern (StateDirecion.left);
						}
					}
				}
			}
			yield return new WaitForSeconds (0.2f);
		}
	}

    
    public void GuestMonsterUpdate()
    {
        aniState = this.animator.GetCurrentAnimatorStateInfo(0);
        if (aniState.IsName("Run"))
        {
            if (moveAble)
            {
				this.transform.Translate(movePoint.normalized * moveSpeed * Time.deltaTime, 0);
            }
        }
        ChasePlayer();
//		if (targetPlayer != null) {
//			currentDisTance= (Vector3.Distance (targetPlayer.transform.position, transform.position));
//		}

    }

	public void ChasePlayer(){
		if(player[0] != null){
			if (!isHited) {
				changeTargetTime += Time.deltaTime;
				if (changeTargetTime >= 3) {
					changeTargetTime = 0;
					NormalchasePlayer ();
				}
			}
			if (isHited) {
				changeTargetTime += Time.deltaTime;
				if (changeTargetTime >= 2) {
					changeTargetTime = 0;
					HitedchasePlayer ();
				}
			}
		}
	}

    //coutine need this method;
    public void NormalchasePlayer()
	{
		for (int i = 0; i < player.Length; i++) {
			currentDisTanceArray [i] = Vector3.Distance(player [i].transform.position, transform.position);		
		}
		for (int j = 0; j < player.Length; j++) {
			if (currentDisTanceArray [j] <= Mathf.Min (currentDisTanceArray)) {
                targetPlayer = player [j];
			}
		}

	}
	//coutine need this method;
	public void HitedchasePlayer()
	{
		for (int i = 0; i < player.Length; i++) {
			currentDisTanceArray [i] = Vector3.Distance(player [i].transform.position, transform.position);		
			if (currentDisTanceArray [i] < 2f) {
				currentDisTanceArray [i] = 2f;
			}
			aggroRank [i] = playerToMonsterDamage [i] *(1/(currentDisTanceArray [i]*0.5f));
		}

		for (int j = 0; j < player.Length; j++) {
			if (aggroRank [j] <= Mathf.Max (aggroRank)) {
                targetPlayer = player [j];
			}
		}
	}




	public void MonsterArrayEraser(GameObject thisGameObject)
	{
		//gameObject = null;
		this.gameObject.SetActive (false);
//				section.RemoveMonsterArray ();
	}

	public virtual void AttackEnd(){
		
		StartCoroutine (LookatChange ());
		moveAble=true;
		isAttack = false;
		animator.SetInteger ("State", 0);
		if (attackCollider != null) {
			attackCollider.AttackColliderOff();
		}
		else if (attackCollider == null) {
			//Instantiate ();	
		}


	}
	public virtual void AttackStart(){
		moveAble = false;
		isAttack = true;
		StopCoroutine (LookatChange ());
	}
	public virtual void AnimatorReset(){
		animator.SetInteger ("State", 0);
	}
	public virtual void AttackBlitz()
	{
		if (attackCollider != null) {
			attackCollider.AttackColliderOn ();
		} else if (attackCollider == null) {
			if (stateDirecion == StateDirecion.left) {
				Instantiate (Resources.Load ("Effect/Monster_ShockWave"), new Vector3 (this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z - 0.53f), this.transform.rotation);

			}
			if (stateDirecion == StateDirecion.right) {
				Instantiate (Resources.Load ("Effect/Monster_ShockWave"), new Vector3 (this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z + 0.53f), this.transform.rotation);	
			}
		}

	}



	public virtual void HitDamage(int _Damage, GameObject _weapon)
	{

	}

	public void GetTargetPlayer(GameObject _TargerPlayer){
		
	}

	public void SendTargetPlayer(){}


}
