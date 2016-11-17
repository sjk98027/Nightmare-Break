using UnityEngine;
using System.Collections;

public class Mealstrom : Skill 
{
	public Mealstrom()
	{
		skillId = 0;
		skillName = "Mealstrom";
		skillDamage = 20;
		skillCost = 10;
		skillLevel = 1;
		skillCoolTime = 5;
	}

	public override void UseSkill()
	{
		Debug.Log ("in Skill1");
//		float maelstromSpeed = 0.5f;
//		float maelstromDistance;
//
//		float skillTime = 0;
//		GameObject[] enermy = GameObject.FindGameObjectsWithTag ("Enermy");
//		GameObject charTranform;
//		charTranform = 
//		//charTranform //GetComponent<CharacterManager> ();
//
//
//		skillTime += Time.deltaTime;
//
//
//		for (int i = 0; i < enermy.Length; i++)
//		{
//			maelstromDistance = Vector3.Distance (charTranform.transform.position, enermy [i].transform.position);
//
//			if (maelstromDistance < 10)
//			{
//				enermy [i].transform.Translate ((charTranform.transform.position - enermy [i].transform.position) * maelstromSpeed * Time.deltaTime, Space.World);
//			}
//		}
//		if (skillTime >= 1.5f)
//		{
//			//mealstromState = false;
//			skillTime = 0;
//		}
	}

	public void Update()
	{
		
	}

	
}
