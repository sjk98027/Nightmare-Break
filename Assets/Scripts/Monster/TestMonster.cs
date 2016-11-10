using UnityEngine;
using System.Collections;


public class TestMonster : Monster {
	
	private float searchRange = 6.0f;
	public float moveSpeed;

	public float currentDisTance;
	public bool monsterAlive;


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
		Death
	};

	//
	void Start(){
		MonsterSet ();
		PlayerSearch ();
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
		case StatePosition.Death:
			{
				//MonsterArrayEraser(this.gameObject);
				break;
			}
		}
	}



	IEnumerator BoomCoroutine() {
		AnimatorReset ();
		transform.position = idlePoint;
		      animator.SetInteger ("State", 4);
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

	public void HitDamage(int _Damage)
	{
		if (monsterAlive)
		{
			Debug.Log ("in");
			maxLife -= _Damage;
			if (maxLife > 0)
			{
				//hitanimation
			}
			else if (maxLife <= 0)
			{
				//DeathAni
			}
		}
	}
}