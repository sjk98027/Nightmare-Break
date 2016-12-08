using UnityEngine;
using System.Collections;

public class MageManager : CharacterManager 
{
	public GameObject frameDestroy;
	public GameObject fireBall;
	public GameObject FireBallPos;
	public GameObject armageddon;
	public GameObject mageRing;
	public Armageddon ArmageddonDamage;
	public bool howling;

	public override void ProcessSkill1 ()
	{
		
	}
	public override void ProcessSkill2 ()
	{
		
	}
	public override void ProcessSkill3 ()
	{
		FireHowling ();
	}
	public override void ProcessSkill4 ()
	{

	}

	public void StrikeBall()
	{
		if (transform.rotation.y == 0)
		{
			Instantiate (Resources.Load<GameObject> ("Effect/MageNormalAttack"), FireBallPos.transform.position, Quaternion.Euler (0, 0, 0));
		}
		else
		{
			Instantiate (Resources.Load<GameObject> ("Effect/MageNormalAttack"), FireBallPos.transform.position, Quaternion.Euler (0, 180, 0));
		}
	}

	public void SummonFireBall()
	{
		if(!fireBall)
		{
			if (transform.rotation.y == 0)
			{
				frameDestroy = Instantiate (Resources.Load<GameObject> ("Effect/FireBall"),FireBallPos.transform.position, Quaternion.Euler (0, 0, 0)) as GameObject;
			}
			else
			{
				frameDestroy = Instantiate (Resources.Load<GameObject> ("Effect/FireBall"), FireBallPos.transform.position,Quaternion.Euler (0, 180, 0)) as GameObject;
			}
			
		}
		
	}

	public void DropMeteo()
	{
		if (!frameDestroy)
		{
			if (transform.rotation.y == 0)
			{
				frameDestroy = Instantiate (Resources.Load<GameObject> ("Effect/FireMagic"), new Vector3 (transform.position.x, transform.position.y + 10.0f, transform.position.z -3.0f), Quaternion.Euler (-135, 0, 0)) as GameObject;
			}
			else
			{
				frameDestroy = Instantiate (Resources.Load<GameObject> ("Effect/FireMagic"), new Vector3 (transform.position.x, transform.position.y + 10.0f, transform.position.z + 3.0f),Quaternion.Euler (-45, 0, 0)) as GameObject;
			}
		}
		
	}

	public void FireHowling()
	{
		if (howling)
		{
			if (!mageRing)
			{
				mageRing = Instantiate (Resources.Load<GameObject> ("Effect/FlameImpact"), new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.Euler (0, 0, 0))as GameObject;
			}
			float howlingSpeed = 5f;
			float howlingDistance;
			skillTime += Time.deltaTime;

			if (enermy != null)
			{
				for (int i = 0; i < enermy.Length; i++)
				{
					howlingDistance = Vector3.Distance (this.transform.position, enermy [i].transform.position);

					if (howlingDistance < 10)
					{
						enermy [i].transform.Translate ((enermy [i].transform.position - this.transform.position) * howlingSpeed * Time.deltaTime, Space.World);
					}
				}
			}
			if (skillTime >= 0.23f)
			{
				skillTime = 0;
				howling = false;
				Destroy (mageRing, 0);
			}
		}
	}
	public void Armageddon()
	{
		if (transform.rotation.y == 0) 
		{
			armageddon = Instantiate (Resources.Load<GameObject> ("Effect/Armageddon"), new Vector3 (transform.position.x, transform.position.y+3.0f, transform.position.z + 2.0f), Quaternion.Euler (0, 0, 0)) as GameObject;
		}
		else 
		{
			armageddon = Instantiate (Resources.Load<GameObject> ("Effect/Armageddon"), new Vector3 (transform.position.x, transform.position.y+3.0f, transform.position.z - 2.0f), Quaternion.Euler (0, 180, 0)) as GameObject;
		}
			ArmageddonDamage = armageddon.GetComponent<Armageddon> ();

		ArmageddonDamage.armageddonDamage = 100;
		
	}
	public void HowlingForce()
	{
		howling = true;
	}

	public override void HitDamage (int _damage)
	{
		if (CharStatus.SkillLevel [5] < 4)
		{
			if (charAlive)
			{
				int chance = (CharStatus.SkillLevel [4]) * 25;
				int superArmor;
				superArmor = Random.Range (0, 100);

				if (CharStatus.HealthPoint > 0)
				{
					CharStatus.DecreaseHealthPoint (_damage);

					if (chance > superArmor)
					{
						if (State != CharacterState.Skill1 && State != CharacterState.Skill2 && State != CharacterState.Skill3 && State != CharacterState.Skill4)
						{
							CharState ((int)CharacterState.HitDamage);
						}
						else
						{
							//ArmorEffect
						}
					}
				}
				else if (CharStatus.HealthPoint <= 0)
				{
					CharState ((int)CharacterState.Death);
					charAlive = false;
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
					CharStatus.DecreaseHealthPoint (_damage);

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
				}
			}
		}
	}


    public override void SetCharacterType()
    {
        charStatus.HClass = CharacterStatus.CharClass.Warrior;
    }



	public override void UsingMagicPoint(int SkillArray)
	{
		float manaFury =SkillManager.instance.SkillData.GetSkill ((int)charStatus.HClass, 6).GetSkillData (CharStatus.SkillLevel [5]).SkillValue;
		charStatus.DecreaseMagicPoint ((int)((float)(SkillManager.instance.SkillData.GetSkill ((int)charStatus.HClass, SkillArray).ManaCost)* manaFury));
	}

}
