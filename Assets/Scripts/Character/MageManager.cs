using UnityEngine;
using System.Collections;

public class MageManager : CharacterManager 
{
	public GameObject frameDestroy;

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

    public override void SetCharacterType()
    {
        charStatus.HClass = CharacterStatus.CharClass.Warrior;
    }
}
