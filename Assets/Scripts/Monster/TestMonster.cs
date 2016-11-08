using UnityEngine;
using System.Collections;


public class TestMonster : MonoBehaviour {
	//public DungeonManager dungeonManager;

	public GameObject[] player;
	//   public Animator animator;
	//   public AnimationState aniState;

	public GameObject targetPlayer;
	//public BattleCarculationManager BCM;

	private Vector3 leftVector3 = new Vector3(0,180,0);
	private Vector3 rightVector3 = new Vector3(0,0,0);


	//mode,gateArraynumber,monsterArraynumber
	protected bool moveAble;
	public bool MoveAble{
		get{ return moveAble;}
		set{ moveAble = value;}
	}

	[SerializeField]private bool mode;
	public bool Mode{
		set { mode = value;}
	}
	[SerializeField]private int gateArrayNumber;
	public int GateArrayNumber{
		set{ gateArrayNumber = value;}
	}

	[SerializeField]private int monsterArrayNumber;
	public int MonsterArrayNumber{
		set{ monsterArrayNumber = value;}
	}

	//server send to this class monsterinfomation;
	protected int stageLevel;
	[SerializeField]protected float currentLife;
	[SerializeField]protected float maxLife;

	//monster getting variable;
	public float RunRange;// == perceive;
	public float attackRange;
	public float attackCycle;
	protected float IdleRandomTime=0;
	//monster Speed variable;

	private bool isAlive;
	[SerializeField]protected bool isAttack;
	private bool isHited;

	public float[] playerToMonsterDamage;
	public float[] aggroRank; //playertoMonsterdamage/currentdistancePlayer;
	public float changeTargetTime=0;


	[SerializeField]private float[]currentDisTanceArray;
	protected Vector3 checkDirection; // monster chaseplayer and move variable;
	//   public bool IsAttack{
	//      get{ return isAttack;}
	//   }
	public bool IsAlive{
		get{ return isAlive;}
	}
	public bool IsHited{
		get{ return isHited;}
		set{ isHited = value;}
	}

	//start monster Haves infomation setting(playersuch,monsterset)
	public void PlayerSearch(){

		player= GameObject.FindGameObjectsWithTag("Player");
		currentDisTanceArray = new float[player.Length];
		playerToMonsterDamage = new float[player.Length];
	}

	public GameObject attackCollider;
	public void Start(){
		PlayerSearch ();
		isAlive = true;
		isHited = false;
		moveAble = true;
		//      animator = this.gameObject.GetComponent<Animator> ();
		StartCoroutine(LookatChange ());
		maxLife = 100;
		currentLife = 100;
		//      attackCollider = GameObject.FindGameObjectWithTag("Finish");
		//      attackCollider.SetActive (false);
	}


	public virtual void HitDamage(float _Damage){

	}


	public void ChasePlayer(){
		//Debug.Log (changeTargetTime);
		if(!isHited)
		{
			changeTargetTime +=Time.deltaTime;
			if(changeTargetTime >=3){
				changeTargetTime = 0;
				NormalchasePlayer();
			}
		}
		if(isHited){
			changeTargetTime += Time.deltaTime;
			if(changeTargetTime >=2){
				changeTargetTime = 0;
				HitedchasePlayer();
			}
		}
	}



	//coutine need this method;
	public void NormalchasePlayer(){
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
	public void HitedchasePlayer(){
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

	public void MonsterArrayEraser(GameObject thisGameObject){
		//gameObject = null;
		isAlive=false;
		thisGameObject.SetActive (false);
		if (!mode) {
			//         section.RemoveMonsterArray ();
		}

		if (mode) {

		}

		//mode = asf1.GetComponent<DungeonManager> ().modeForm;
		//dungeonManager.MonsterArrayAliveCheck(asdf)
	}

	public enum StateDirecion{
		right,
		left
	};
	public StateDirecion stateDirecion;

	IEnumerator LookatChange(){
		while (true) {
			if (!isAttack) {
				if (targetPlayer != null) {
					if ((targetPlayer.transform.position.z - transform.position.z) >= 0) {
						LookAtPattern (StateDirecion.right);
					}
					if ((targetPlayer.transform.position.z - transform.position.z) < 0) {
						LookAtPattern (StateDirecion.left);
					}
				}
			}
			yield return new WaitForSeconds (2f);
		}
	}

	public void AttackEnd(){

		StartCoroutine (LookatChange ());
		//Debug.Log ("animator.SetInteger (State, 0)");
		moveAble=true;
		isAttack = false;
		//animator.SetInteger ("State", 0);
		attackCollider.SetActive (false);
		Debug.Log ("AttackEnd");
	}
	public void AttackStart(){
		moveAble = false;
		isAttack = true;
		StopCoroutine (LookatChange ());
		Debug.Log ("AttackStart");
	}
	public void AnimatorReset(){
		//      animator.SetInteger ("State", 0);
		isAttack = false;
	}
	public void AttackBlitz()
	{
		attackCollider.SetActive (true);
	}

	public void LookAtPattern(StateDirecion state){
		switch(state){
		case StateDirecion.right: 
			{transform.rotation = Quaternion.Euler(rightVector3);break;}
		case StateDirecion.left:
			{transform.rotation = Quaternion.Euler(leftVector3);break;}
		}
	}
	private float searchRange = 6.0f;
	public float moveSpeed;

	public float currentDisTance;


	private Vector3 movePoint;
	private Vector3 idlePoint = new Vector3(0,0,0);

	private Vector3 boomPoint = new Vector3(100,100,100);
	private Vector3 attackPoint = new Vector3 (1, 0, 0);

	public enum StatePosition
	{
		Idle=1,
		Run,
		Attack,
		Boom,
		TakeDamage,
		Death
	};

	//




	//animation Set; move;
	public void Pattern(StatePosition state)
	{
		switch (state)
		{
		case StatePosition.Idle:
			{
				this.transform.Translate(idlePoint * Time.deltaTime, 0);
				//                    animator.SetInteger("State", 0);
				break;
			}
		case StatePosition.Boom:
			{
				idlePoint = this.gameObject.transform.position;
				StartCoroutine("BoomCoroutine"); break;
			} // animator boom -> setintter 4
		case StatePosition.Attack:
			{
				StartCoroutine(AttackProcess());
				break;
			}
		case StatePosition.Run:
			{
				AnimatorReset();
				this.transform.Translate(movePoint * moveSpeed * Time.deltaTime, 0);
				//                    animator.SetInteger("State", 2);
				searchRange = 10;
				break;
			}

		case StatePosition.TakeDamage:
			{
				StartCoroutine(TakeDamageCorutine());
				break;
			}
		case StatePosition.Death:
			{
				MonsterArrayEraser(this.gameObject);
				break;
			}
		}
	}

	IEnumerator TakeDamageCorutine(){
		//      animator.SetTrigger ("TakeDamage");
		yield return new WaitForSeconds (0.3f);
		AnimatorReset();
		yield return new WaitForSeconds (0.05f);
		StopCoroutine (TakeDamageCorutine ());

	}


	IEnumerator BoomCoroutine() {
		AnimatorReset ();
		transform.position = idlePoint;
		//      animator.SetInteger ("State", 4);
		yield return new WaitForSeconds (3f);
		//      animator.SetTrigger("Death");
		yield return new WaitForSeconds (3f);
		transform.position = boomPoint;
		Pattern (StatePosition.Death);
		StopCoroutine (BoomCoroutine());
	}

	IEnumerator AttackProcess(){
		AnimatorReset ();
		moveAble = false;
		yield return new WaitForSeconds (1.8f);
		//      animator.SetInteger ("State", 3);

		Debug.Log ("Attack");//attackaniamtion start;
		yield return new WaitForSeconds (0.03f);
		//      animator.SetInteger ("State", 0);
		isAttack= false;
		moveAble = true;
		Debug.Log ("AttackEnd");//attackanimation end;
		StopCoroutine (AttackProcess ());
	}
	public void Update()
	{
		if (IsAlive)
		{
			ChasePlayer();// playerchase;

			if (targetPlayer != null)
			{
				currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
				checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;

				if (currentDisTance > searchRange)
				{
					Pattern(StatePosition.Idle);
					Debug.Log ("idle");
				}
				//if this object get Attackmotion pattern(stateposition.boom -> attack), and this monsterlife is 20%, boomPattern start;
				else if (currentDisTance <= searchRange)
				{

					movePoint = new Vector3(checkDirection.x, 0, checkDirection.z);

					if (currentDisTance >= searchRange * 0.2f)
					{
						if (moveAble) {
							Pattern (StatePosition.Run);
						}
					}
					if (currentDisTance < searchRange * 0.2f)
					{
						if (!isAttack) {
							isAttack = true;
							Pattern (StatePosition.Attack);
						}

					}
					if (currentLife / maxLife < 0.2)
					{
						Pattern(StatePosition.Boom);
					}



					//Debug.Log (animator.GetCurrentAnimatorStateInfo (0));
				}


			}


		}
		if (!IsAlive)
		{
			Pattern(StatePosition.Death);
		}

	}
	void OnTriggerEnter(Collider coll){

		Debug.Log ("hit");
		//if (coll.gameObject.layer == LayerMask.NameToLayer("Weapon")) {
		Pattern (StatePosition.TakeDamage);

	}
}