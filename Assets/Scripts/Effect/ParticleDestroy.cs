using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour 
{

	private	ParticleSystem particle;
	public GameObject character;
	public int swordDamage;
	public CharacterStatus charstate;
	public CharacterManager charManager;

	// Use this for initialization
	void Start () 
	{
		particle = GetComponent<ParticleSystem> ();
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
	//	charstate = charManager.charstate;
		StartCoroutine (ParticleCheck ());

	}

	IEnumerator ParticleCheck()
	{
		yield return new WaitForSeconds (particle.duration);
		Destroy(gameObject);	
		if (gameObject.transform.parent != null) {
			Destroy (gameObject.transform.parent.gameObject);
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
		//	swordDamage = charstate.skillLevel [5] * charManager.BasicDamage;

			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				monsterDamage.HitDamage (swordDamage,character );
				swordDamage = 0;

			}
		}
	}


}
