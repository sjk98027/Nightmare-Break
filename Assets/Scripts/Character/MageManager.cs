using UnityEngine;
using System.Collections;

public class MageManager : CharacterManager 
{
	public GameObject frameDestroy;
	public GameObject fireBall;
	public GameObject FireBallPos;

	public override void ProcessSkill1 ()
	{
		
	}
	public override void ProcessSkill2 ()
	{
		
	}
	public override void ProcessSkill3 ()
	{
		
	}
	public override void ProcessSkill4 ()
	{

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
		float HowlingSpeed = 5f;
		float HowlingDistance;
		skillTime += Time.deltaTime;


		if (enermy != null)
		{
			for (int i = 0; i < enermy.Length; i++)
			{
				HowlingDistance = Vector3.Distance (this.transform.position, enermy [i].transform.position);

				if (HowlingDistance < 10)
				{
					
					enermy [i].transform.Translate ((this.transform.position + enermy [i].transform.position) * HowlingSpeed * Time.deltaTime, Space.World);
				}
			}
		}

		if (skillTime >= 1.5f)
		{
			skillTime = 0;
		}

		
	}

    public override void SetCharacterType()
    {
        charStatus.HClass = CharacterStatus.CharClass.Warrior;
    }
}
