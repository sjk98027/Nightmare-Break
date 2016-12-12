using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
    public enum StateDirecion
    {
        right = 0,
        left
    };

    protected Animator animator;
    protected AnimatorStateInfo aniState;
    protected BoxCollider HittedBox;
    protected MonsterWeapon attackCollider;
    protected ShockWave shockWaveInstantiate;

    public GameObject[] player;
    protected GameObject targetPlayer;
    protected Vector3 movePoint;

    protected int monsterRunAttackAround;

	protected int randomStandby;
	//public int RandomStandby;
	//mode,gateArraynumber,monsterArraynumber
	protected bool moveAble;

    [SerializeField] protected int monsterIndex;
    [SerializeField] protected UnitId monsterId;
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

    //monster Speed variable;

    StateDirecion stateDirecion;
    [SerializeField]protected bool isAlive;
	[SerializeField]protected bool isAttack;
	[SerializeField]protected bool isHited;

    protected float[] playerToMonsterDamage;
	private float[] aggroRank; //playertoMonsterdamage/currentdistancePlayer;
    private float changeTargetTime=0;

	[SerializeField]private float[]currentDisTanceArray;
	protected Vector3 checkDirection; // monster chaseplayer and move variable;
    
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
    public UnitId MonsterId { get { return monsterId; } set { monsterId = value; } }

    public void MonsterSet(MonsterBaseData monster)
	{
        animator = this.gameObject.GetComponent<Animator>();
        HittedBox = this.gameObject.GetComponent<BoxCollider>();

        isAlive = true;
        isHited = false;
        moveAble = true;

        _name = monster.Name;
        level = monster.MonsterLevelData[0].Level;
        attack = monster.MonsterLevelData[0].Attack;
        defense = monster.MonsterLevelData[0].Defense;
        currentHP = monster.MonsterLevelData[0].HealthPoint;
        maxHP = monster.MonsterLevelData[0].HealthPoint;
        moveSpeed = monster.MonsterLevelData[0].MoveSpeed;

        currentDisTanceArray = new float[player.Length];
		aggroRank = new float[player.Length];
		playerToMonsterDamage = new float[player.Length];
        
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

	public virtual void MonsterMoveAI(bool normalMode){
		
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

    public void ChasePlayer(){
		//Debug.Log (changeTargetTime);
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

    public void GuestMonsterUpdate()
    {
        aniState = this.animator.GetCurrentAnimatorStateInfo(0);
        if (aniState.IsName("Run"))
        {
            if (moveAble)
            {
                this.transform.Translate(movePoint * moveSpeed * Time.deltaTime, 0);
            }
        }
        ChasePlayer();
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
		Debug.Log (this.gameObject);
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
