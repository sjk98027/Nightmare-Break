using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {

	public CharacterStatus charStatus;
	public CharacterManager charManager;
	public GameObject character;
	public int MeteorDamage;

	void Start()
	{
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = charManager.CharStatus;
	}

    void Update()
    {
        transform.Translate(0, 0, 30 * Time.smoothDeltaTime, Space.Self);    
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            Destroy(gameObject);
            Instantiate(Resources.Load<GameObject>("Effect/MeteorExplosion"), coll.contacts[0].point, Quaternion.identity);
        }

    }

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in monster");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				monsterDamage.HitDamage (MeteorDamage,character );
				MeteorDamage = 0;
			}

			Destroy(gameObject);
			Instantiate(Resources.Load<GameObject>("Effect/MeteorExplosion"), this.transform.position, Quaternion.identity);

		}
	}
}
