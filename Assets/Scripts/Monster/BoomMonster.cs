using UnityEngine;
using System.Collections;


public class BoomMonster : Monster {
	private float searchRange = 12.0f;
	private float moveSpeed = 1f;

	public float currentDisTance;
	float middleBossToMonsterLimitDistanceMonsterToCenter = 6.0f;
	private float middleBossToMonsterMinDistance = 1.5f;
	public AnimatorStateInfo aniState;

	[SerializeField]GameObject middleboss;
	private Vector3 boomObjectPosition;


	public override void HitDamage(int _Damage,GameObject attacker)
	{
		IsHited = true;
		currentLife -= _Damage;
		if(monsterState != StatePosition.Boom){
			if (currentLife > 0) {
				for (int i = 0; i < player.Length; i++) {
					if (player [i] == attacker) {
						playerToMonsterDamage [i] += _Damage;
						targetPlayer = player [i];
					}
				}
				Pattern (StatePosition.TakeDamage);
			}
		}
		if (currentLife <= 0) {
			currentLife = 0;
			IsAlive = false;
			HittedBox.enabled = false;
			monsterState = StatePosition.Death;
			Pattern (monsterState);
		}
	}
	private Vector3 movePoint;
	private Vector3 idlePoint = new Vector3(0,0,0);

	//private Vector3 boomPoint = new Vector3(100,100,100);

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
				IsAlive = false;
				StartCoroutine("BoomCoroutine"); break;
			} // animator boom -> setintter 4
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
	IEnumerator BoomCoroutine() {
		AnimatorReset ();
		transform.position = idlePoint;
		yield return new WaitForSeconds (3f);
		IsAlive = false;
		animator.SetTrigger("Death");

		StopCoroutine (BoomCoroutine());
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

	public void middleBossPositionGetting(Vector3 _Position){
		boomObjectPosition = _Position;
	}


	public void MonSterPatternUpdateConduct(bool NormalMode){
		if (NormalMode) {
			StartCoroutine (PatternNormalChange ());
		} else if (!NormalMode) {
			StartCoroutine (PatternDefenceChange ());
		}
	}




	public IEnumerator PatternNormalChange(){
		while(IsAlive){
			if (targetPlayer != null) {	
				currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
				checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
				if (currentDisTance > searchRange) {
					monsterState = StatePosition.Idle;
					Pattern (monsterState);
					SendMonsterState (monsterState,isAttack, moveAble, movePoint, targetPlayer);
				}
				//if this object get Attackmotion pattern(stateposition.boom -> attack), and this monsterlife is 20%, boomPattern start;
				else if (currentDisTance <= searchRange) {
					movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
					if (currentDisTance > searchRange * 0.2f) {
						moveAble = true;
						isAttack = false;
						monsterState = StatePosition.Run;
						Pattern (monsterState);
						SendMonsterState (monsterState,isAttack, moveAble, movePoint, targetPlayer);
					}
					if (currentDisTance <= searchRange * 0.3f) {
						if (!isAttack) {
							isAttack = true;
							moveAble = false;
						}
						if (currentLife > maxLife * 0.3f) {
							monsterState = StatePosition.Attack;
							Pattern (monsterState);
							SendMonsterState (monsterState,isAttack, moveAble, movePoint, targetPlayer);
							yield return new WaitForSeconds (0.5f);
						} else if (currentLife < maxLife * 0.3) {
							if (Random.Range (0, 4) <= 2) {
								monsterState = StatePosition.Attack;
								Pattern (monsterState);
								SendMonsterState (monsterState,isAttack, moveAble, movePoint, targetPlayer);
								yield return new WaitForSeconds (0.5f);
							} else if(Random.Range (0, 4) > 2)
								monsterState = StatePosition.Boom;
							Pattern (monsterState);
							SendMonsterState (monsterState,isAttack, moveAble, movePoint,targetPlayer);
							yield return new WaitForSeconds (4f);
						}
					}
				}
			}
			yield return new WaitForSeconds(0.2f);
		}
	}
	public IEnumerator PatternDefenceChange(){
		while(IsAlive){
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
		yield return new WaitForSeconds (0.2f);
	}


	public void UpdateNormalMode()
	{
		aniState = this.animator.GetCurrentAnimatorStateInfo (0);

		if (aniState.IsName ("Run")) 
		{
			if (moveAble) 
			{
				this.transform.Translate (movePoint * moveSpeed * Time.deltaTime, 0);
			}
		}
		ChasePlayer ();
    }
	public void UpdateDefenceMode(){

		aniState = this.animator.GetCurrentAnimatorStateInfo (0);

		if (aniState.IsName ("Run")) {
			if (moveAble) {
				this.transform.Translate (transitionVector * moveSpeed * 0.5f * Time.deltaTime);
			}
		}

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

	// server code;
	public void SendMonsterState(StatePosition _state, bool _isAttack, bool _moveAble, Vector3 _movePoint, GameObject _Player){
		//send to server;

	}

	public void RecibeMonsterState(StatePosition _state, bool _isAttack, bool _moveAble, Vector3 _movePoint, GameObject _Player){
		if (_state == StatePosition.Run) {
			movePoint = _movePoint;
		}
		monsterState = _state;
		isAttack = _isAttack;
		moveAble = _moveAble;
		Pattern (_state);
		if (_Player != null) {
			targetPlayer = _Player;
		}
	}

	public void GuestMonsterUpdate(){
		aniState = this.animator.GetCurrentAnimatorStateInfo (0);
		if (aniState.IsName ("Run")) 
		{
			if (moveAble) 
			{
				this.transform.Translate (movePoint * moveSpeed * Time.deltaTime, 0);
			}
		}
		ChasePlayer ();
	}



	public IEnumerator GuestMonsterPatternChange(){
		while (IsAlive) {
			Pattern (monsterState);
			yield return new WaitForSeconds (0.2f);
		}
	}



}
