using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

  	public ParticleSystem FireBallparticleSystem;
	public CharacterStatus charStatus;
	public CharacterManager charManager;
	public GameObject character;
	public int FireBallDamage;
	public float FireBallSpeed = 180;
	public Rigidbody FireBallRigid;

    void Start()
    {
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = charManager.CharStatus;
		FireBallRigid = GetComponent<Rigidbody> ();
		FireBallRigid.velocity = transform.forward* FireBallSpeed;
		FireBallparticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
		FireBallparticleSystem.Simulate(1.5f, true);
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
				monsterDamage.HitDamage (FireBallDamage,character );
				FireBallDamage = 0;
			}

			Destroy(gameObject);
			Instantiate(Resources.Load<GameObject>("Effect/MeteorExplosion"), this.transform.position, Quaternion.identity);

		}
	}

}
