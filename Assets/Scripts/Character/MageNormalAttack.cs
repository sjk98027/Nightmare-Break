﻿using UnityEngine;
using System.Collections;

public class MageNormalAttack : MonoBehaviour {

	public GameObject character;
	public CharacterManager charManager;
	public CharacterStatus charStatus;

	public int MageBallDamage;
	public float MageBallSpeed = 5;
	public Rigidbody MageBallRigid;

	void Start () 
	{
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = GameObject.FindGameObjectWithTag("CharStatus").GetComponent<CharacterStatus>();
		charStatus.SetCharacterStatus ();

		MageBallRigid = GetComponent<Rigidbody> ();
		MageBallRigid.velocity = transform.forward* MageBallSpeed;
		Destroy (this.gameObject, 1f);

	}


	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				MageBallDamage = charManager.charStatus.Attack;
				monsterDamage.HitDamage (MageBallDamage, character);
				Debug.Log (MageBallDamage);
				MageBallDamage = 0;
			}

			Destroy (gameObject);
			Instantiate (Resources.Load<GameObject> ("Effect/MeteorExplosion"), this.transform.position, Quaternion.identity);

		}
		else if (coll.gameObject.layer == LayerMask.NameToLayer ("Map"))
		{

			Destroy (gameObject);
			Instantiate (Resources.Load<GameObject> ("Effect/MeteorExplosion"), this.transform.position, Quaternion.identity);
			
		}
	}


}
