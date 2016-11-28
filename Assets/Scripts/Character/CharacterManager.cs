using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

public class CharacterManager : MonoBehaviour
{
    public enum Gender
    {
        Male = 0,
        FeMale = 1,
    }

    public enum CharacterState
	{
		Idle = 0,
		Run,
		Attack,
		Jump,

		Skill1,
		Skill2,
		Skill3,
		Skill4,
		HitDamage,
		Death
	}
	public Animator animator;
	public Renderer rend;
	public AnimatorStateInfo runState;
	public CharacterStatus charstate;
	public float skillTime;

	public float[] skillCoolTime;

	public bool charDir;
	public bool JumpMove;
	public Rigidbody rigdbody;
	public BoxCollider charWeapon;
	public BoxCollider charBox;
	public bool normalAttackState = false;
	public bool skillAttackState = false;
	public InputManager inputmanager;

	public GameObject[] enermy;
	public GameObject wall;

	private int potionCount = 3;
	public float jumpPower;
	public bool charAlive = true;
	public int basicDamage;

	public GameObject[] Enermy {get {return this.enermy;}}

	public Animator Animator { get { return animator; } }

	public CharacterState state;

	public CharacterStatus Charstate {get {return this.charstate;}}

	public CharacterState State { get { return state; } }

	public bool NormalAttackState {get {return this.normalAttackState;}}

	public bool SkillAttackState{get {return this.skillAttackState;}}

	public int BasicDamage{ get { return this.basicDamage; } }

	public UIManager uiManager;

	void Awake ()
	{
		charstate = GameObject.FindGameObjectWithTag ("CharStatus").GetComponent<CharacterStatus>();
		charstate.SetCharacterStatus ();
		basicDamage = Charstate.Attack;
		uiManager = GameObject.FindWithTag ("UI").GetComponent<UIManager> ();
		animator = GetComponent<Animator> ();
		charBox = this.gameObject.GetComponent<BoxCollider> ();
		state = CharacterState.Idle;
		enermy = null;
		rigdbody = this.GetComponent<Rigidbody>();
		charDir = true;
		wall = GameObject.FindGameObjectWithTag ("Wall");
		JumpMove = false;
		enermy = GameObject.FindGameObjectsWithTag("Enermy");
		skillCoolTime = new float[4];
		CharCoolTime (0);
	}

	void Update ()
	{
		if (charAlive)
		{
			// check skill state
			if (skillAttackState )
			{
				// skill index
				// skill process ProcessSkill1()

				switch (state)
				{
				case CharacterState.Skill1:
					ProcessSkill1 ();
					break;
				
				case CharacterState.Skill2:
					ProcessSkill2 ();
					break;

				case CharacterState.Skill3:
					ProcessSkill3 ();
					break;

				case CharacterState.Skill4:
					ProcessSkill4 ();
					break;
				}
					
			}



			if (normalAttackState)
			{
				charWeapon.enabled = true;
			}
			else
			{
				charWeapon.enabled = false;
			}
		}

	}

	public void AnimationEnd ()
	{
		CharState ((int)CharacterState.Idle);
		JumpMove = false;
		normalAttackState = false;
		skillAttackState = false;

	}

	//char state Method
	public void Move (float ver, float hor)
	{
		if (state == CharacterState.Idle || state == CharacterState.Run)
		{
			
			runState = this.animator.GetCurrentAnimatorStateInfo (0);

			if (!animator.GetBool ("Attack"))
			{
				if (ver != 0 || hor != 0)
				{
					animator.SetFloat ("Ver", ver);
					animator.SetFloat ("Hor", hor);

					if (ver < 0)
					{
						transform.rotation = Quaternion.Euler (new Vector3 (0, 180.0f, 0));
						charDir = false;
					}
					else if (ver > 0)
					{
						transform.rotation = Quaternion.Euler (new Vector3 (0, 0.0f, 0));
						charDir = true;
					}
					CharState ((int)CharacterState.Run);

					if (runState.IsName ("Run"))
					{
						if (hor == -1.0f || hor == 1.0f)
						{
							transform.Translate ((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * (charstate.CharSpeed - 3.0f), Space.World);

						}
						else
						{
							transform.Translate ((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * (charstate.CharSpeed), Space.World);
						}
					}
				}
				else if (ver == 0 && hor == 0)
				{
					animator.SetBool ("Run", false);
					CharState ((int)CharacterState.Idle);
				}
			}
		}
		else if (state == CharacterState.Jump && JumpMove)
		{
			transform.Translate ((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * charstate.CharSpeed, Space.World);
		}

	}
		
	public void CheckGrounded ()
	{
		if (state == CharacterState.Jump)
		{
			if (transform.position.y <= 0.1f)
			{
				CharState ((int)CharacterState.Idle);
			}
		}
	}

	public void Jump ()
	{
		runState = this.animator.GetCurrentAnimatorStateInfo (0);

		if (state != CharacterState.Jump  && state != CharacterState.Attack && state != CharacterState.Skill1 && state != CharacterState.Skill2 && state != CharacterState.Skill1 && state != CharacterState.Skill4)
		{
			CharState ((int)CharacterState.Jump);
		}


	}

	public void JumpForce ()
	{
			rigdbody.AddForce (Vector3.up * jumpPower, ForceMode.Impulse);
			JumpMove = true;
	}

	public virtual void NormalAttack ()
	{
		//if ( state != characterState.Skill2 && state != CharacterState.Attack  && state != characterState.Skill1 && state != characterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
		if ( state != CharacterState.Skill2  && state != CharacterState.Skill1 && state != CharacterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
		{
			normalAttackState = true;
			CharState ((int)CharacterState.Attack);

			if (JumpMove)
			{
				rigdbody.mass = 100;
			}
			else
			{
				rigdbody.mass = 1;
			}


		}
	}


	public virtual void Skill1 ()
	{
		if (state == CharacterState.Run || state == CharacterState.Idle || state == CharacterState.Skill1)
		{
			if (uiManager.BattleUIManager.SkillUI [0].fillAmount == 1)
			{
				StartCoroutine (uiManager.BattleUIManager.SkillCoolTimeUI (0, skillCoolTime [0]));

				state = CharacterState.Skill1;
				CharState ((int)CharacterState.Skill1);

			}
		}
	}


	public void skill2 ()
	{
		if (state != CharacterState.Jump && state != CharacterState.Skill2 && state != CharacterState.Skill1 && state != CharacterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
		{
			if (uiManager.BattleUIManager.SkillUI [1].fillAmount == 1)
			{
				StartCoroutine (uiManager.BattleUIManager.SkillCoolTimeUI (1, skillCoolTime [1]));

				CharState ((int)CharacterState.Skill2);
			}
		}

	}

	public void skill3()
	{
		if (state != CharacterState.Jump && state != CharacterState.HitDamage && state != CharacterState.Skill3 && state != CharacterState.Skill2 && state != CharacterState.Skill1 && state != CharacterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
		{
			if (uiManager.BattleUIManager.SkillUI [2].fillAmount == 1)
			{
			StartCoroutine (uiManager.BattleUIManager.SkillCoolTimeUI (2, skillCoolTime [2]));

				CharState ((int)CharacterState.Skill3);
			}
		}
	}

	public void Skill4 ()
	{
		if (state != CharacterState.Jump && state != CharacterState.Skill1 && state != CharacterState.Skill2 && state != CharacterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
		{
			if (uiManager.BattleUIManager.SkillUI [3].fillAmount == 1)
			{
				StartCoroutine (uiManager.BattleUIManager.SkillCoolTimeUI (3, skillCoolTime [3]));

				//giganticSwordCastSword.SetActive(true);
				CharState ((int)CharacterState.Skill4);
			}
		}

	}





	public virtual void ProcessSkill1()
	{	
		
	}

	public virtual void ProcessSkill2()
	{		

	}

	public virtual void ProcessSkill3()
	{		

	}

	public virtual void ProcessSkill4()
	{		

	}
	//using Potion
	public void UsingPotion ()
	{   //Potion Effect create
		GameObject potionEffect = Instantiate (Resources.Load<GameObject> ("Effect/Potion"), transform.position, Quaternion.identity) as GameObject;
		potionEffect.transform.parent = gameObject.transform;
		potionEffect.transform.position += Vector3.up;
		StartCoroutine (Potion ());
	}

	IEnumerator Potion ()
	{
		for (int i = 0; i < potionCount; i++)
		{
			charstate.healthPoint += (int)(charstate.healthPoint * 0.3);
			yield return new WaitForSeconds (1f);
		}
	}


	//Animation Method
	void SetStateDefault ()
	{
		animator.SetBool ("Idle", false);
		animator.SetBool ("Run", false);
	}

	public void CharState (int Inputstate)
	{
		if (charAlive)
		{
			SetStateDefault ();
			//idle=0,run=1,attack=2
			switch (Inputstate)
			{
			case 0:
				state = CharacterState.Idle;
				animator.SetBool ("Idle",true);
				break;

			case 1:
				state = CharacterState.Run;
				animator.SetBool ("Run", true);
				break;

			case 2:
				state = CharacterState.Attack;
				animator.SetTrigger ("Attack");
				break;

			case 3:
				state = CharacterState.Jump;
				animator.SetTrigger ("Jump");
				break;

			case 4:
					state = CharacterState.Skill1;
					animator.SetTrigger ("Skill1");
					skillAttackState = true;
					break;

			case 5:
					state = CharacterState.Skill2;
					animator.SetTrigger ("Skill2");
					skillAttackState = true;
					//basicDamage = charstate.activeSkillSet [1].skillDamage
					break;

			case 6:
					state = CharacterState.Skill3;
					animator.SetTrigger ("Skill3");
					skillAttackState = true;
					//basicDamage = charstate.activeSkillSet [3].skillDamage;
					break;
			case 7:
					state = CharacterState.Skill4;
					animator.SetTrigger ("Skill4");
					skillAttackState = true;
					//basicDamage = charstate.activeSkillSet [2].skillDamage;
					break;
			case 8:
					state = CharacterState.HitDamage;
					animator.SetTrigger ("PlayerHitTrigger");
					break;
			case 9:
					state = CharacterState.Death;
					animator.SetTrigger ("PlayerDie");
					break;
			}
		}
	}
	public void CharCoolTime(int _name)
	{		
		if (_name == 0)
		{
			skillCoolTime [0] = 5;
			skillCoolTime [1] = 3;
			skillCoolTime [2] = 3;
			skillCoolTime [3] = 4;
		}


	}


	public virtual void HitDamage(int _damage)
	{
		if(charAlive)
		{
			if (charstate.HealthPoint > 0)
			{
				
				this.charstate.HealthPoint -= _damage;
				CharState ((int)CharacterState.HitDamage);
			}
			if (charstate.HealthPoint <= 0)
			{
				//Death Animation
				CharState ((int)CharacterState.Death);
				charAlive= false;
			}
		}
	}
	//NetWork
	public CharacterState GetCharacterState (int state)
	{
		switch (state)
		{
		case 0:
			return CharacterState.Idle;

		case 1:
			return CharacterState.Run;

		case 2:
			return CharacterState.Attack;

		default:
			return CharacterState.Idle;
		}
	}

    public void SetPosition(CharacterPositionData newPositionData)
    {
        Debug.Log("캐릭터 위치 설정");

        if (newPositionData.dir)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0.0f, 0));
            charDir = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
            charDir = false;
        }
        transform.position = new Vector3(newPositionData.posX, newPositionData.posY, newPositionData.posZ);
    }
}

