using UnityEngine;
using System.Collections;


public class BoomMonster2Test : Monster {
	private float searchRange = 6.0f;
	private float moveSpeed = 0.5f;

	public float currentDisTance;
	float middleBossToMonsterLimitDistanceMonsterToCenter = 6.0f;
	private float middleBossToMonsterMinDistance = 1.5f;
	public AnimatorStateInfo aniState;

	[SerializeField]GameObject middleboss;
	private Vector3 boomObjectPosition;


	public override void HitDamage(int _Damage,GameObject attacker)

	{
		if (IsAlive)
		{
			IsHited=true;
			currentLife -= _Damage;
			for (int i = 0; i < player.Length; i++) {
				Debug.Log (i);

				if (player [i] == attacker) {
					Debug.Log ("in");
					playerToMonsterDamage[i] += _Damage;
					targetPlayer = player [i];
				}

			}


			//uiManager.bossHp.fillAmount = currentLife / maxLife;
			if (currentLife > 0)
			{
				Pattern (StatePosition.TakeDamage);

				//hitanimation
			}
			else if (currentLife <= 0)
			{
				Pattern (StatePosition.Death);
				IsAlive = false;

			}
		}
	}
	private Vector3 movePoint;
	private Vector3 idlePoint = new Vector3(0,0,0);

	private Vector3 boomPoint = new Vector3(100,100,100);

	public enum StatePosition
	{
		Idle=1,
		Run,
		Attack,
		Boom,
		TakeDamage,
		Death
	};

	public StatePosition monsterState;

	[SerializeField]public Vector3[] pointVector;
	[SerializeField]public Vector3 transitionVector;	
	public Vector3[] PointVector{
		get {return pointVector; }
		set{pointVector = value; }
	}

	public void pointVectorArrayGetting(Vector3[] _v3){
		pointVector = new Vector3[_v3.Length];
		for (int i=0; i < _v3.Length; i++) {
			pointVector [i] = _v3 [i];
		}
		StartCoroutine (pointVectorchange ());
	}

	public IEnumerator pointVectorchange()
	{
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


	//animation Set; move;
	public void Pattern(StatePosition state)
	{
		switch (state)
		{
		case StatePosition.Idle:
			{
				this.transform.Translate(idlePoint * Time.deltaTime, 0);
				animator.SetInteger("State", 0);
				break;
			}
		case StatePosition.Boom:
			{
				idlePoint = this.gameObject.transform.position;
				StartCoroutine("BoomCoroutine"); break;
			} // animator boom -> setintter 4
		case StatePosition.Attack:
			{
				//	StartCoroutine (AttackProcess ());
				AttackProcess1(isAttack);
				break;
			}
		case StatePosition.Run:
			{
				AnimatorReset();

				animator.SetInteger("State", 2);
				//searchRange = 10;
				break;
			}
		case StatePosition.TakeDamage:
			{
				StartCoroutine(TakeDamageCorutine());
				//				TakeDamageMethod();
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
		animator.SetTrigger ("TakeDamage");
		yield return new WaitForSeconds (0.3f);
		AnimatorReset();
		yield return new WaitForSeconds (0.05f);
		StopCoroutine (TakeDamageCorutine ());

	}

//	public void TakeDamageMethod(){
//		animator.SetTrigger ("TakeDamage");
//		IsHited = true;
//		if (stateInfo.name == "TakeDamage") {
//			isAttack = false;
//			moveAble = false;
//		} else
//			AnimatorReset ();
//
//
//	}


	IEnumerator BoomCoroutine() {
		AnimatorReset ();
		transform.position = idlePoint;
		//		animator.SetInteger ("State", 4);
		yield return new WaitForSeconds (3f);
		animator.SetTrigger("Death");
		yield return new WaitForSeconds (3f);
		transform.position = boomPoint;
		Pattern (StatePosition.Death);
		StopCoroutine (BoomCoroutine());
	}

	public void AttackProcess1(bool isAttack){
		if (isAttack) {

			if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Walk")){
				animator.SetInteger ("State", 0);
			}
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Idle")) {
				animator.SetInteger ("State", 3);
			}
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Attak2")) {
				moveAble = false;
			}


		}

	}

	IEnumerator AttackProcess(){
		AnimatorReset ();
		moveAble = false;
		yield return new WaitForSeconds (1.8f);
		animator.SetInteger ("State", 3);

		Debug.Log ("Attack");//attackaniamtion start;
		yield return new WaitForSeconds (2f);
		animator.SetInteger ("State", 0);
		isAttack= false;
		moveAble = true;
		Debug.Log ("AttackEnd");//attackanimation end;
		StopCoroutine (AttackProcess ());
	}


	public void UpdateNormalMode()
	{
		if (IsAlive)
		{
			ChasePlayer();// playerchase;
			if (targetPlayer != null)
			{
				currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
				checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;

				if (currentDisTance > searchRange) {
					monsterState = StatePosition.Idle;
					Pattern (monsterState);
				}
				//if this object get Attackmotion pattern(stateposition.boom -> attack), and this monsterlife is 20%, boomPattern start;
				else if (currentDisTance <= searchRange)
				{
					movePoint = new Vector3(checkDirection.x, 0, checkDirection.z);
					if (currentDisTance > searchRange * 0.2f)
					{
//						if (moveAble) {
						moveAble=true;
						isAttack = false;
							monsterState = StatePosition.Run;
							Pattern (monsterState);
//						}
					}
					if (currentDisTance <= searchRange * 0.2f)
					{
//						if (!isAttack) {
							isAttack = true;
							moveAble = false;
//						}
						monsterState = StatePosition.Attack;
						Pattern (monsterState);
					}
					if (currentLife / maxLife < 0.2)
					{
						monsterState = StatePosition.Boom;
						Pattern(monsterState);
					}
				}
			}
		}
		if (!IsAlive)
		{
			monsterState = StatePosition.Death;
			Pattern(StatePosition.Death);
		}

	}
	public void UpdateDefenceMode(){
		if (!IsHited) {
			transform.Translate (transitionVector * moveSpeed * 0.5f * Time.deltaTime);
		}
		if (IsHited) {

			if (checkDirection.z > 0) {
				LookAtPattern (StateDirecion.right);
			}
			if (checkDirection.z <= 0) {
				LookAtPattern (StateDirecion.left);
			}

			currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
			checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;



			if (currentDisTance < middleBossToMonsterLimitDistanceMonsterToCenter*1.5f) {
				movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
				transform.Translate(movePoint.normalized * moveSpeed * Time.deltaTime, 0);
				if (currentDisTance >= searchRange * 0.2f)
				{
					if (moveAble) {
						Pattern (StatePosition.Run);
						Debug.Log ("Run");
					}
				}
				if (currentDisTance < searchRange * 0.2f)
				{
					attackCycle += Time.deltaTime;
					if (attackCycle > 5) {
						attackCycle = 0;
						if (!isAttack) {
							isAttack = true;
							Pattern (StatePosition.Attack);
						}
						//					}
					}
				}
			}
			if (currentDisTance >= middleBossToMonsterLimitDistanceMonsterToCenter*1.5f) {
				LookAtPattern (StateDirecion.right);
				IsHited = false;
				targetPlayer = null;
				transform.Translate (boomObjectPosition*Time.deltaTime);
			}
		}
	}

	public void middleBossPositionGetting(Vector3 _Position){
		boomObjectPosition = _Position;
	}



	public void Start(){
		PlayerSearch();
		MonsterSet();
		NormalMode = true;
		monsterState = StatePosition.Idle;
		StartCoroutine (PatternChange());

	}
	public void Update()
	{
		aniState = this.animator.GetCurrentAnimatorStateInfo (0);

		if (aniState.IsName ("Walk")) 
		{
			if (moveAble) 
			{
				this.transform.Translate (movePoint * moveSpeed * Time.deltaTime, 0);
			}
		}
		ChasePlayer ();
	}

	public IEnumerator PatternChange(){
		while(true){
			
			if (!IsAlive) {
				StopCoroutine (PatternChange ());
			}
			if (IsAlive) {
				if (targetPlayer != null) {	
					currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
					checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
				
					if (currentDisTance > searchRange) {
						monsterState = StatePosition.Idle;
						Pattern (monsterState);
					}
				//if this object get Attackmotion pattern(stateposition.boom -> attack), and this monsterlife is 20%, boomPattern start;
				else if (currentDisTance <= searchRange) {
						movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
						if (currentDisTance > searchRange * 0.2f) {
								moveAble = true;
								isAttack = false;
								monsterState = StatePosition.Run;
								Pattern (monsterState);
						}
						if (currentDisTance <= searchRange * 0.2f) {
							if (!isAttack) {
								isAttack = true;
								moveAble = false;
							}
							monsterState = StatePosition.Attack;
							Pattern (monsterState);
						}
						if (currentLife / maxLife < 0.2) {
							monsterState = StatePosition.Boom;
							Pattern (monsterState);
						}
						if (currentLife < 0) {
							monsterState = StatePosition.Death;
							Pattern (monsterState);
						}
					}
				}
			}
			yield return new WaitForSeconds(0.2f);
		
		//StatePosition statePosition
		}
	}

}
