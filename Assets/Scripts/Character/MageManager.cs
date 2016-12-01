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
		Instantiate (Resources.Load<GameObject> ("Effect/MageNormalAttack"), FireBallPos.transform.position, Quaternion.Euler (0, 0, 0));
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
				frameDestroy = Instantiate (Resources.Load<GameObject> ("Effect/FireBall"), FireBallPos.transform.position,Quaternion.Euler (0, 0, 0)) as GameObject;
			}
			
		}
		
	}

	public void DropMeteo()
	{
		if (!frameDestroy)
		{
			if (transform.rotation.y == 0)
			{
				frameDestroy = Instantiate (Resources.Load<GameObject> ("Effect/FireMagic"), new Vector3 (transform.position.x, transform.position.y + 10.0f, transform.position.z - 2.0f), Quaternion.Euler (-135, 0, 0)) as GameObject;
			}
			else
			{
				frameDestroy = Instantiate (Resources.Load<GameObject> ("Effect/FireMagic"), new Vector3 (transform.position.x, transform.position.y + 10.0f, transform.position.z + 2.0f),Quaternion.Euler (-45, 0, 0)) as GameObject;
			}
		}
		
	}

	public void FireHowling()
	{
		if (!mageRing)
		{
			mageRing = Instantiate (Resources.Load<GameObject> ("Effect/MagicionRing"), new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.Euler (0, 0, 0))as GameObject;

		}
			float howlingSpeed = 0.5f;
			float howlingDistance;
			skillTime += Time.deltaTime;

			if (enermy != null)
			{
				for (int i = 0; i < enermy.Length; i++)
				{
					howlingDistance = Vector3.Distance (this.transform.position, enermy [i].transform.position);

					if (howlingDistance < 10)
					{
						enermy [i].transform.Translate ((this.transform.position + enermy [i].transform.position) * howlingSpeed * Time.deltaTime, Space.World);
					}
				}
			}

			if (skillTime >= 1.5f)
			{
				skillTime = 0;
			}



	}
	public void Armageddon()
	{
		armageddon = Instantiate (Resources.Load<GameObject> ("Effect/Armageddon"), new Vector3 (transform.position.x, transform.position.y+3.0f, transform.position.z + 2.0f),Quaternion.Euler (0, 0, 0)) as GameObject;
		ArmageddonDamage = armageddon.GetComponent<Armageddon> ();

		ArmageddonDamage.armageddonDamage = 100;
		
	}

    public override void SetCharacterType()
    {
        charStatus.HClass = CharacterStatus.CharClass.Warrior;
    }
}
