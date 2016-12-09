using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
public class Monster : MonoBehaviour {
	//public DungeonManager dungeonManager;

	public GameObject[] player;//
	public Animator animator;
	public AnimatorStateInfo aniState;

	public GameObject targetPlayer;
	private Vector3 leftVector3 = new Vector3(0,180,0);
	private Vector3 rightVector3 = new Vector3(0,0,0);

	public BoxCollider HittedBox;
	private int monsterRunAttackAround;
	public int MonsterRunAttackAround;

	protected int randomStandby;
	//public int RandomStandby;
	//mode,gateArraynumber,monsterArraynumber
	protected bool moveAble;
	public bool MoveAble{
		get{ return moveAble;}
		set{ moveAble = value;}
	}

    [SerializeField]private int monsterArrayNumber;
	public int MonsterArrayNumber{
		set{ monsterArrayNumber = value;}
	}

	public Vector3 movePoint;

	//server send to this class monsterinfomation;
	protected int stageLevel;
	[SerializeField]protected float currentLife;
	[SerializeField]protected float maxLife;

	//monster getting variable;
	public float RunRange;// == perceive;
	public float attackRange;
	public float attackCycle;
	protected float IdleRandomTime=0;

	[SerializeField]private int baseDamage;
	public int BaseDamage{
		get{ return baseDamage;}
		set{ baseDamage = value;}
	}
	//monster Speed variable;

	[SerializeField]private bool isAlive;
	[SerializeField]protected bool isAttack;
	private bool isHited;

	public float[] playerToMonsterDamage;
	public float[] aggroRank; //playertoMonsterdamage/currentdistancePlayer;
	public float changeTargetTime=0;
	 
	public MonsterWeapon attackCollider;
	public ShockWave shockWaveInstantiate;
	//public CottomExplosion boomInstantiate;

	[SerializeField]private float[]currentDisTanceArray;
	protected Vector3 checkDirection; // monster chaseplayer and move variable;
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

    public float MaxHP
    {
        get { return maxLife; }
        set { maxLife = value; }
    }
    
    public float CurrentHP
    {
        get { return currentLife; }
        set { currentLife = value; }
    }
	public enum StateDirecion{
		right,
		left
	};
	public StateDirecion stateDirecion;

	public void MonsterSet(int _maxlife, int _baseDamage)
	{
		isAlive = true;
		isHited = false;
		moveAble = true;
		maxLife = _maxlife;
		currentLife = maxLife;
		baseDamage = _baseDamage;

		currentDisTanceArray = new float[player.Length];
		aggroRank = new float[player.Length];
		playerToMonsterDamage = new float[player.Length];

		animator = this.gameObject.GetComponent<Animator> ();
		HittedBox = this.gameObject.GetComponent<BoxCollider> ();

		StartCoroutine(LookatChange ());

		if (attackCollider != null) {
			attackCollider = this.transform.GetComponentInChildren<MonsterWeapon> ();
			attackCollider.MonsterWeaponSet ();
		}
		if (shockWaveInstantiate != null) {
			shockWaveInstantiate.GetDamage (baseDamage, this.gameObject.GetComponent<ShockWaveMonster> ());
		}
		if(shockWaveInstantiate != null|| attackCollider !=null){
//			NormalMonsterRealizePattern ();
			//bossmonsterWeapon damageGet(basedamage);
		}

//		if(boomInstaniate != null){
//			
//		}
	}

	public virtual void MonsterMoveAI(bool normalMode){
		
	}



	public void LookAtPattern(StateDirecion state)
	{
		switch(state){
		case StateDirecion.right: 
			{transform.rotation = Quaternion.Euler(rightVector3);
				stateDirecion = StateDirecion.right;break;}
		case StateDirecion.left:
			{transform.rotation = Quaternion.Euler(leftVector3);
				stateDirecion = StateDirecion.left;break;}
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
