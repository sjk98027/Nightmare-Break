using UnityEngine;
using System.Collections;


public class TestMonster : Monster 
{
	public enum BigBearBossPatternName
	{
		BigBearBossIdle =0,
		BigBearBossRun,
		BigBearBossAttack,
		BigBearBossOneHandAttack,
		BigBearJumpAttack,
		BigBearBossRoar,
		BigBearBossDeath
	};
	
	public float searchRange;
	public float moveSpeed;
	bool secondAttack;
	float AttackTime;

	public BoxCollider[] MonsterWeapon;
	public bool monsterAlive;
	public BigBearBossPatternName BigBearBossState;
	AnimatorStateInfo stateInfo;
	bool monsterAttack;
	//public float searchRange;





	void Start()
	{
		MonsterSet ();
		PlayerSearch ();
		secondAttack = false;
		BoxCollider[] MonsterWeapon = new BoxCollider[2];
	
	}



	//animation Set; move;

	void Update()
	{

		if (monsterAlive)
		{
			stateInfo = this.animator.GetCurrentAnimatorStateInfo (0);
			searchRange = Vector3.Distance (player [0].transform.position, transform.position);
			AttackTime += Time.deltaTime;

			if(AttackTime >= 1)
			{
				if(secondAttack)
				{
					secondAttack = false;
				}
				else
				{
					secondAttack = true;	
				}


				AttackTime = 0;
			}

		

			if (searchRange < attackRange)
			{
				
				if (secondAttack)
				{

					BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossAttack);

				}
				else if(!secondAttack)
				{
					BigBearBossPattern ((int)BigBearBossPatternName.BigBearJumpAttack);

				}
			}
			else if(searchRange > RunRange)
			{
				BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossIdle);
			}
			else if (searchRange <= RunRange)
			{
				BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossRun);

				if(stateInfo.IsName("BigBearBossRun"))
				{

					//transform.LookAt(player[0].transform.position);
					transform.Translate ((transform.position-player [0].transform.position )*moveSpeed * Time.deltaTime);
					//transform.position = Vector3.Lerp (transform.position, player [0].transform.position, Time.deltaTime * moveSpeed);
				}
			}

			if (monsterAttack)
			{
				for (int i = 0; i < MonsterWeapon.Length; i++)
				{
					MonsterWeapon [i].size = new Vector3 (3.6f, 1f, 1.1f);
				}
			}
			else if (!monsterAttack)
			{
				for (int i = 0; i < MonsterWeapon.Length; i++)
				{
					MonsterWeapon [i].size = new Vector3 (0,0,0);
				}
			}


		}
		if (!monsterAlive)
		{
			Destroy (this.gameObject, 5);
		}

	}





	public void HitDamage(int _Damage)
	{
		stateInfo = this.animator.GetCurrentAnimatorStateInfo (0);

		if (monsterAlive)
		{
			Debug.Log ("in damage");
			maxLife -= _Damage;
			if (maxLife > 0)
			{
				//hitanimation
			}
			else if (maxLife <= 0)
			{
				if (!stateInfo.IsName ("BigBearBossDeath"))
				{
					BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossDeath);
					monsterAlive = false;
					return;
				}
			}
		}
	}

	public void BigBearBossPattern(int bossState)
	{
		monsterAttack = false;
		switch (bossState)
		{
			
		case 0:
			BigBearBossState = BigBearBossPatternName.BigBearBossIdle;
			animator.SetInteger ("state", 0);
			break;
		case 1:
			BigBearBossState = BigBearBossPatternName.BigBearBossRun;
			animator.SetInteger ("state", 1);
			break;
		case 2:
			BigBearBossState = BigBearBossPatternName.BigBearBossAttack;
			animator.SetInteger ("state", 2);
			monsterAttack = true;
			break;

		case 3:
			BigBearBossState = BigBearBossPatternName.BigBearBossOneHandAttack;
			animator.SetInteger ("state", 3);
			monsterAttack = true;
			break;

		case 4:
			BigBearBossState = BigBearBossPatternName.BigBearJumpAttack;
			animator.SetInteger ("state", 4);
			break;

		case 5:
			BigBearBossState = BigBearBossPatternName.BigBearBossRoar;
			animator.SetInteger ("state", 5);
			break;

		case 6:
			BigBearBossState = BigBearBossPatternName.BigBearBossDeath;
			animator.SetTrigger ("BigBearBossDeath");
			break;
		}
	}


}