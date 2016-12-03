using UnityEngine;
using System.Collections;

public class Armageddon : MonoBehaviour {

	public ParticleSystem FireBallparticleSystem;
	public CharacterStatus charStatus;
	public CharacterManager charManager;
	public GameObject character;
	public int armageddonDamage;
	public float FireBallSpeed = 0.001f;
	public Rigidbody FireBallRigid;
    private ParticleSystem ps;
	public SphereCollider spherecol;
    
    void Start()
    {
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = charManager.CharStatus;
        ps = GetComponent<ParticleSystem>();
		FireBallRigid = GetComponent<Rigidbody> ();
		FireBallRigid.velocity = transform.forward* FireBallSpeed;
        StartCoroutine(ArmageddonPause());

		spherecol = this.GetComponent<SphereCollider> ();
		spherecol.enabled = false;
		StartCoroutine(ArmageddonColl());


    }
    IEnumerator ArmageddonPause()
    {
		
        yield return new WaitForSeconds(ps.duration - 0.2f);
        ps.Pause();
		FireBallRigid.velocity = transform.forward* 0;

    }
	IEnumerator ArmageddonColl()
	{
		while (true)
		{
			if (spherecol.enabled == false)
			{
				spherecol.enabled = true;
			}
		

			yield return new WaitForSeconds (0.3f);

			if (spherecol.enabled == true)
			{
				spherecol.enabled = false;
			}
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
				armageddonDamage = charStatus.Attack;
				monsterDamage.HitDamage (armageddonDamage,character );
				armageddonDamage = 0;
			}

		}
		
	}

}
