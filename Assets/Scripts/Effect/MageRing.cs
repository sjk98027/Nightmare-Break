using UnityEngine;
using System.Collections;

public class MageRing : MonoBehaviour 
{

	public ParticleSystem FireBallparticleSystem;
	public CharacterStatus charStatus;
	public CharacterManager charManager;
	public GameObject character;
	public int ringDamage = 10;

	// Use this for initialization
	void Start () 
	{
	
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = charManager.CharStatus;

	}
	
	// Update is called once per frame

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in monster");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				monsterDamage.HitDamage (ringDamage,character );
				ringDamage = 0;
			}

		}
	}

}
