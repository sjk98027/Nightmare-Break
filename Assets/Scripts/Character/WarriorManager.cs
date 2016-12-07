using UnityEngine;
using System.Collections;

public class WarriorManager : CharacterManager
{
	//GiganticSword
	public GameObject GiganticSword;
	public float giganticSwordSpeed;
	public GameObject giganticSwordTemp;

	public GameObject SwordDance;
	public bool rise = false;
	public CharWeapon bloodingWeapon;
	public float riseCooltime;
    private GameObject wind;
    [SerializeField]
    private TrailRenderer trailRenderer;
	int skillLv;

	public override void NormalAttack ()
	{
		base.NormalAttack ();
	}

	public override void SetClassObject ()
	{
	
	}

	public override void SetCharacterType ()
	{
		charStatus.HClass = CharacterStatus.CharClass.Warrior;
	}
	//warrior mealstrom
	public override void ProcessSkill1 ()
	{
		float maelstromSpeed = 0.5f;
		float maelstromDistance;
        skillTime += Time.deltaTime;
        if (!wind)
        {
        wind = Instantiate(Resources.Load<GameObject>("Effect/Wind"), new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity) as GameObject;
			wind.transform.parent = this.gameObject.transform;
        }

	//	transform.Translate ((Vector3.forward * testinput.vertical - Vector3.right * testinput.horizontal) * Time.deltaTime * (charStatus.MoveSpeed -6.0f), Space.World);


        if (enermy != null)
		{
			for (int i = 0; i < enermy.Length; i++)
			{
				maelstromDistance = Vector3.Distance (this.transform.position, enermy [i].transform.position);
                
				if (maelstromDistance < 10)
				{
					enermy [i].transform.Translate ((this.transform.position - enermy [i].transform.position) * maelstromSpeed * Time.deltaTime, Space.World);
				}
			}
		}

		if (skillTime >= 1.5f)
		{
			skillTime = 0;
		}

	}

	//Warrior Cutoff
	public override void ProcessSkill2 ()
	{
		skillTime += Time.deltaTime;


			animator.speed = 1;
			skillTime = 0;


	}

	public override void ProcessSkill3 ()
	{
		if (!SwordDance)
		{
			if (transform.rotation.y == 0)
			{
				SwordDance = Instantiate (Resources.Load<GameObject> ("Effect/SwordDance"), new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z + 0.5f), Quaternion.Euler (-90, 0, 0)) as GameObject;
			}
			else
			{
				SwordDance = Instantiate (Resources.Load<GameObject> ("Effect/SwordDance"), new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z - 0.5f), Quaternion.Euler (90, 0, 0)) as GameObject;
			}
		}
	}

	public override void ProcessSkill4 ()
	{

	}

	public void CutOffMove ()
	{
		Instantiate (Resources.Load<GameObject> ("Effect/SwordShadow"), new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);

		Ray cutOffDistance = new Ray (this.transform.position, transform.forward);
		RaycastHit rayHit;

		if (Physics.Raycast (cutOffDistance, out rayHit, 5f, 1 << LayerMask.NameToLayer ("Map")))
		{
			transform.Translate (0, 0, rayHit.distance - 0.5f);

		}
		else
		{
			transform.Translate (0, 0, 5);
		}
	}

	public void CutoffStop ()
	{
		//animator.speed = 0;
	}

	public void SwordDanceBoxSummon ()
	{
		Debug.Log ("DanceSummon");
		Instantiate (Resources.Load<GameObject> ("Effect/SwordDanceBox"), new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
	}

	public void GiganticSwordSummon ()
	{
		float giganticSwordPos;
		if (charDir)
		{
			giganticSwordPos = 10.0f;
		}
		else
		{
			giganticSwordPos = -10.0f;
		}

		giganticSwordTemp = Instantiate (Resources.Load<GameObject> ("GiganticSword"), transform.position + new Vector3 (0.0f, 10.0f, giganticSwordPos), Quaternion.Euler (new Vector3 (-90.0f, 90.0f, -180.0f))) as GameObject;
		giganticSwordTemp.gameObject.GetComponent<Rigidbody> ().AddForce (-Vector3.up * giganticSwordSpeed, ForceMode.Impulse);
	}

	public override void HitDamage (int _damage)
	{
		if (CharStatus.SkillLevel [5] < 4)
		{
			if (charAlive)
			{
				if (charAlive)
				{
					if (CharStatus.HealthPoint > 0)
					{
						int deFendDamage;
						deFendDamage = _damage - (CharStatus.SkillLevel [5] * 1);
						Debug.Log (deFendDamage);
						if (deFendDamage < 0)
						{
							deFendDamage = 0;
						}
						CharStatus.DecreaseHealthPoint (deFendDamage);

						if (State != CharacterState.Skill1 && State != CharacterState.Skill2 && State != CharacterState.Skill3 && State != CharacterState.Skill4)
						{
							CharState ((int)CharacterState.HitDamage);
						}
					}
					if (CharStatus.HealthPoint <= 0)
					{
						CharState ((int)CharacterState.Death);
						charAlive = false;
					}
				}
			}
		}
		else if (CharStatus.SkillLevel [5] == 4)
		{
			Debug.Log (CharStatus.HealthPoint );
			if (charAlive)
			{
				if (CharStatus.HealthPoint > 0)
				{
					int deFendDamage;
					deFendDamage = _damage - (CharStatus.SkillLevel [5] * 1);

					if (deFendDamage < 0)
					{
						deFendDamage = 0;
					}
					CharStatus.DecreaseHealthPoint (deFendDamage);
	
					CharState ((int)CharacterState.HitDamage);

					if (State != CharacterState.Skill1 && State != CharacterState.Skill2 && State != CharacterState.Skill3 && State != CharacterState.Skill4)
					{
						CharState ((int)CharacterState.HitDamage);
					}
				}
				else if (CharStatus.HealthPoint <= 0)
				{
					
					CharState ((int)CharacterState.Death);

					charAlive = false;

					if (!rise)
					{
						rise = true;
						charAlive = true;
						animator.SetBool ("Rise", false);
						StartCoroutine (colltimeCheck());
						CharStatus.DecreaseHealthPoint ((-100));
					
					}
					else if (rise)
					{
						animator.SetBool ("Rise", true);
					}
				}
			}
		}
	}

	public void NormalAttackEffect(int _attack)
	{
        if(_attack == 0)
        {
            trailRenderer.enabled = true;
        } else
        {
            trailRenderer.enabled = false;
        }
	}

	public IEnumerator colltimeCheck ()
	{
		while (rise)
		{
			riseCooltime += 1f;
			yield return new WaitForSeconds (1f);
			Debug.Log (riseCooltime);
			if (riseCooltime > 10)
			{
				riseCooltime = 0;
				rise = false;
			}
		}
	}
}
