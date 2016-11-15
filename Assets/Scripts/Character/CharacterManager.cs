using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

public class CharacterManager : MonoBehaviour
{
		
	public enum CharacterState
	{
		Idle = 0,
		Run,
		Attack,
		Jump,
		CutOff,
		Maelstrom,
		GiganticSwordSummon,
		SwordDance,
		HitDamage,
		Death
	}

	Animator animator;
	public Renderer rend;
	AnimatorStateInfo runState;
	public CharacterStatus charstate;
	public float skillTime;

	public bool mealstromState;

	public bool charVer;
	public bool JumpMove;
	public Rigidbody rigdbody;
	public BoxCollider charWeapon;
	public bool normalAttackState = false;
	public bool skillAttackState = false;
	public int basicDamage;

	public InputManager inputmanager;

	public GameObject[] enemy;
	public GameObject wall;

	CharacterStatus stat= null;
	private int potionCount = 3;

	public float charSpeed;
	public float jumpPower;
	public bool charAlive = true;

	//GiganticSword
	public GameObject GiganticSword;
	public float giganticSwordSpeed;
	public float giganticSwordMatarial;
	public bool giganticSwordRendTime;
	public GameObject giganticSwordTemp;
	public GameObject giganticSwordCastSword;

	public int BasicDamage {get {return this.basicDamage;}}

	public Animator Animator { get { return animator; } }

	[SerializeField]CharacterState state;

	public CharacterStatus Charstate {get {return this.charstate;}}

	public CharacterState State { get { return state; } }

	public bool NormalAttackState {get {return this.normalAttackState;}}

	public bool SkillAttackState{get {return this.skillAttackState;}}

	void Start ()
	{
		//stat = GetComponent<CharacterStatus> ();
		animator = GetComponent<Animator> ();
		state = CharacterState.Idle;
		enemy = null;
		rigdbody = this.GetComponent<Rigidbody>();
		mealstromState = false;
		charVer = true;
		wall = GameObject.FindGameObjectWithTag ("Wall");
		JumpMove = false;
		giganticSwordCastSword = GameObject.Find("GiganticSwordSwordCast");
		//giganticSwordCastSword.SetActive(false);
		//giganticSword = Resources.Load<GameObject> ("GiganticSword");
	}

	void Update ()
	{
		if (charAlive)
		{
			enemy = GameObject.FindGameObjectsWithTag ("Enermy");
			if (mealstromState)
			{
				Maelstrom ();
			}
			if (giganticSwordRendTime && giganticSwordTemp != null)
			{
//				rend = GiganticSword.gameObject.GetComponent<Renderer> ();
				giganticSwordMatarial += Time.deltaTime;

				if (giganticSwordTemp.transform.position.y > 0.1)
				{
					
					float giganticSwordAlpha = 1;
				//	rend.material.color = new Color (0, 0, 0, giganticSwordAlpha);

					if (giganticSwordAlpha < 0.5)
					{
						giganticSwordMatarial = 0;
						Destroy (giganticSwordTemp, 0.5f);
					}
				}
			}
			if (normalAttackState || skillAttackState)
			{
				//charWeapon.center = new Vector3 (5.0f,2.1f,0.7f);
				charWeapon.size = new Vector3 (0.11f,0.11f,1.28f);
			}
			else
			{
				charWeapon.size = new Vector3 (0,0,0);
			}
		}

	}

	public void AnimationEnd ()
	{
		CharState ((int)CharacterState.Idle);
		JumpMove = false;
		//		giganticSwordCastSword.SetActive(false);
		Debug.Log ("anima End");
		normalAttackState = false;
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
						charVer = false;
					}
					else if (ver > 0)
					{
						transform.rotation = Quaternion.Euler (new Vector3 (0, 0.0f, 0));
						charVer = true;
					}
					CharState ((int)CharacterState.Run);

					if (runState.IsName ("Run"))
					{
						transform.Translate ((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * charSpeed, Space.World);
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
			transform.Translate ((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * charSpeed, Space.World);
		}

	}

	public void NormalAttack ()
	{
		//if ( state != CharacterState.CutOff && state != CharacterState.Attack  && state != CharacterState.Maelstrom && state != CharacterState.GiganticSwordSummon && state != CharacterState.HitDamage && state != CharacterState.Death)
		if ( state != CharacterState.CutOff  && state != CharacterState.Maelstrom && state != CharacterState.GiganticSwordSummon && state != CharacterState.HitDamage && state != CharacterState.Death)
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

		if (state != CharacterState.Jump  && state != CharacterState.Attack && state != CharacterState.Maelstrom && state != CharacterState.CutOff && state != CharacterState.Maelstrom && state != CharacterState.GiganticSwordSummon)
		{
			CharState ((int)CharacterState.Jump);
		}


	}

	public void JumpForce ()
	{
			rigdbody.AddForce (Vector3.up * jumpPower, ForceMode.Impulse);
			JumpMove = true;
	}


	//swordmaster Skill
	public void Espada ()
	{
		
		if (state != CharacterState.Jump && state != CharacterState.Maelstrom && state != CharacterState.CutOff && state != CharacterState.GiganticSwordSummon && state != CharacterState.HitDamage && state != CharacterState.Death)
		{
			Debug.Log ("sk3");
			//giganticSwordCastSword.SetActive(true);
			CharState ((int)CharacterState.GiganticSwordSummon);
		}
	}

	public void GiganticSwordSummon ()
	{
		float giganticSwordPos;
		if (charVer)
		{
			giganticSwordPos = 10.0f;

		}
		else
		{
			giganticSwordPos = -10.0f;
		}

		giganticSwordTemp = Instantiate (Resources.Load<GameObject> ("GiganticSword"), transform.position + new Vector3 (0.0f, 10.0f, giganticSwordPos), Quaternion.Euler (new Vector3 (0.0f, -90, 0.0f))) as GameObject;
		giganticSwordTemp.gameObject.GetComponent<Rigidbody> ().AddForce (-Vector3.up *giganticSwordSpeed, ForceMode.Impulse);
		giganticSwordRendTime = true;
	}

	public void Maelstrom ()
	{

		if (state == CharacterState.Run || state == CharacterState.Idle)
		{

			state = CharacterState.Maelstrom;
			CharState ((int)CharacterState.Maelstrom);
		}	
			
			
			float maelstromSpeed = 0.5f;
			float maelstromDistance;
			
			skillTime += Time.deltaTime;

		
			for (int i = 0; i < enemy.Length; i++)
			{
				maelstromDistance = Vector3.Distance (this.transform.position, enemy [i].transform.position);

				if (maelstromDistance < 10)
				{
					enemy [i].transform.Translate ((this.transform.position - enemy [i].transform.position) * maelstromSpeed * Time.deltaTime, Space.World);
				}
			}
			if (skillTime >= 1.5f)
			{
				mealstromState = false;
				skillTime = 0;
			}

	}

	public void CutOff ()
	{
		if (state != CharacterState.Jump && state != CharacterState.CutOff && state != CharacterState.Maelstrom && state != CharacterState.GiganticSwordSummon && state != CharacterState.HitDamage && state != CharacterState.Death)
		{
			CharState ((int)CharacterState.CutOff);
		}
	}

	public void CutOffMove ()
	{
		Instantiate (Resources.Load<GameObject> ("Effect/SwordShadow"), new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);

		Ray cutOffDistance = new Ray (this.transform.position, transform.forward); //(this.transform.position);
		RaycastHit rayHit;

		if (Physics.Raycast (cutOffDistance, out rayHit, 5f, 1 << LayerMask.NameToLayer ("Map")))
		{
			transform.Translate (0, 0, rayHit.distance - 0.5f);
		}
		else
		{
			transform.Translate (0, 0, 5);
		}
		//animation stop and keyboardinput Lock
	}

	public void SwordDance()
	{
		if (state != CharacterState.Jump && state != CharacterState.SwordDance && state != CharacterState.CutOff && state != CharacterState.Maelstrom && state != CharacterState.GiganticSwordSummon && state != CharacterState.HitDamage && state != CharacterState.Death)
		{
			CharState ((int)CharacterState.SwordDance);
		}
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
			stat.healthPoint += (int)(stat.healthPoint * 0.3);
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
				state = CharacterState.CutOff;
				animator.SetTrigger ("CutOff");
				skillAttackState = true;
				break;

			case 5:
				state = CharacterState.Maelstrom;
				animator.SetTrigger ("Maelstrom");
				skillAttackState = true;
				break;

			case 6:
				state = CharacterState.GiganticSwordSummon;
				animator.SetTrigger ("GiganticSwordSummon");
				skillAttackState = true;
				break;

			case 7:
				state = CharacterState.SwordDance;
				animator.SetTrigger ("SwordDance");
				skillAttackState = true;
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

	public void HitDamage(int _damage)
	{
		if(charAlive)
		{
			
			if (charstate.HealthPoint > 0)
			{
				this.charstate.HealthPoint -= _damage;
				CharState ((int)CharacterState.HitDamage);
				Debug.Log("Hit Char"+ this.charstate.HealthPoint);
			}
			if (charstate.HealthPoint <= 0)
			{
				//Death Animation
				CharState ((int)CharacterState.Death);
				charAlive= false;
				Debug.Log ("death");
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
	public void SetState (CharacterStateData newStateData)
	{
		Debug.Log ("상태 설정");

        animator.SetFloat ("Ver", newStateData.ver);
        animator.SetFloat ("Hor", newStateData.hor);

        if (newStateData.ver < 0)
        {
        	transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
        	charVer = false;
        }
        else if (newStateData.ver >= 0)
        {
        	transform.rotation = Quaternion.Euler(new Vector3(0, 0.0f, 0));
        	charVer = true;
        }
		transform.position = new Vector3 ((float)Math.Round((double)newStateData.posX, 3), (float)Math.Round((double)newStateData.posY, 3), (float)Math.Round((double)newStateData.posZ, 3));

		CharState(newStateData.state);
	}
}

