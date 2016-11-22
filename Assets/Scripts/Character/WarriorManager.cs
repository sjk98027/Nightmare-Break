using UnityEngine;
using System.Collections;

public class WarriorManager : CharacterManager 
{


	//GiganticSword
	public GameObject GiganticSword;
	public float giganticSwordSpeed;
	public float giganticSwordMatarial;
	public bool giganticSwordRendTime;
	public GameObject giganticSwordTemp;
	public GameObject giganticSwordCastSword;
    public GameObject SwordDance;

	public charWeapon bloodingWeapon;



	//giganticSwordCastSword.SetActive(false);
	//giganticSword = Resources.Load<GameObject> ("GiganticSword");

	public override void NormalAttack()
	{

		int testPassiveHP;

		bool checkHit = charWeapon.GetComponent<charWeapon> ().checkHit;

//
//		if (checkHit)
//		{
//			charstate.skillLevel [4] = 3;
//			Debug.Log ("in passive");
//			float attackHeal = (charstate.skillLevel [4] * 0.1f);
//			testPassiveHP = (int)(basicDamage * attackHeal);
//
//			charstate.healthPoint += testPassiveHP;
//
//			Debug.Log (testPassiveHP);

//
//			if (charstate.skillLevel [4] == 4)
//			{
//
//				//mealstrom heal
//				//			if (charstate.healthPoint <= 0)
//				//			{
//				//				Debug.Log ("in Special");
//				//				charstate.healthPoint = charstate.maxHealthPoint;
//				//			}
//			}
//			charWeapon.GetComponent<charWeapon> ().checkHit = false;
//		}
//		
		base.NormalAttack();

	}
	//warrior mealstrom
	public override void ProcessSkill1 ()
	{
		
			float maelstromSpeed = 0.5f;
			float maelstromDistance;
			skillTime += Time.deltaTime;

			for (int i = 0; i < enermy.Length; i++)
			{
				maelstromDistance = Vector3.Distance (this.transform.position, enermy [i].transform.position);

				if (maelstromDistance < 10)
				{
					enermy [i].transform.Translate ((this.transform.position - enermy [i].transform.position) * maelstromSpeed * Time.deltaTime, Space.World);
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

		if (skillTime >= 2)
		{
			Animator.speed = 1;
			skillTime = 0;
		}
	}

	public override void ProcessSkill3 ()
	{
        if (!SwordDance)
        {
            if (transform.rotation.y == 0)
            {
              SwordDance = Instantiate(Resources.Load<GameObject>("Effect/SwordDance"), new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z + 0.5f), Quaternion.Euler(-90, 0, 0)) as GameObject;
            }
            else
            {
                SwordDance =  Instantiate(Resources.Load<GameObject>("Effect/SwordDance"), new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z - 0.5f), Quaternion.Euler(90, 0, 0)) as GameObject;
            }
        }
    }

	public override void ProcessSkill4 ()
	{
		giganticSwordCastSword = GameObject.Find("GiganticSwordSwordCast");

		if (giganticSwordRendTime && giganticSwordTemp != null)
		{
			//rend = GiganticSword.gameObject.GetComponent<Renderer> ();
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

	public void CutoffStop()
	{
		Animator.speed = 0;			
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

		giganticSwordTemp = Instantiate (Resources.Load<GameObject> ("GiganticSword"), transform.position + new Vector3 (0.0f, 10.0f, giganticSwordPos), Quaternion.Euler (new Vector3 (0.0f, -90, 0.0f))) as GameObject;
		giganticSwordTemp.gameObject.GetComponent<Rigidbody> ().AddForce (-Vector3.up *giganticSwordSpeed, ForceMode.Impulse);
		giganticSwordRendTime = true;
	}

}

