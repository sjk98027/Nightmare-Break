using UnityEngine;
using System.Collections;

public class MageNormalAttack : MonoBehaviour {

	public GameObject character;
	public CharacterManager charManager;
	public CharacterStatus charStatus;

	public int MageBallDamage= 100;
	public float MageBallSpeed = 50;
	public Rigidbody MageBallRigid;

	void Start () 
	{
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = GameObject.FindGameObjectWithTag("CharStatus").GetComponent<CharacterStatus>();
		charStatus.SetCharacterStatus ();
		MageBallDamage = 100;
		MageBallRigid = GetComponent<Rigidbody> ();
		MageBallRigid.velocity = transform.forward* MageBallSpeed;

	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in monster");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				monsterDamage.HitDamage (MageBallDamage,character );
				MageBallDamage = 0;
			}

			Destroy(gameObject);
			Instantiate(Resources.Load<GameObject>("Effect/MeteorExplosion"), this.transform.position, Quaternion.identity);

		}
	}


}
