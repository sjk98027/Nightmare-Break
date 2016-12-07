using UnityEngine;
using System.Collections;

public class Armageddon : MonoBehaviour {

	public ParticleSystem FireBallparticleSystem;
	public CharacterStatus charStatus;
	public CharacterManager charManager;
	public GameObject character;
	public int armageddonDamage;
	public float FireBallSpeed;
	public Rigidbody FireBallRigid;
    private ParticleSystem ps;
	public SphereCollider spherecol;
	int skillLv;
	bool amaDestroy;

	float zPos;

    void Start()
    {
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = charManager.CharStatus;
        ps = GetComponent<ParticleSystem>();
		FireBallRigid = GetComponent<Rigidbody> ();
		FireBallSpeed = 5f;
		FireBallRigid.velocity =(transform.forward* FireBallSpeed);
        StartCoroutine(ArmageddonPause());

		spherecol = this.GetComponent<SphereCollider> ();
		spherecol.enabled = false;
		spherecol.radius = 0.1f;
		zPos = 0;
		StartCoroutine(ArmageddonColl());

		skillLv = charStatus.SkillLevel [4];
		armageddonDamage =(int) ((SkillManager.instance.SkillData.GetSkill ((int)charStatus.HClass, 4).GetSkillData (skillLv).SkillValue)*  charStatus.Attack);
    }

	void Update ()
	{
		if (amaDestroy)
		{
			Destroy (this.gameObject, 5f);
		}
	}
    IEnumerator ArmageddonPause()
    {
			yield return new WaitForSeconds (ps.duration - 0.2f);
			ps.Pause ();
			FireBallRigid.velocity = transform.forward * 0;
			amaDestroy = true;
    }

	IEnumerator ArmageddonColl()
	{
		
		while (true)
		{
			zPos = zPos + 0.035f;
			if (spherecol.radius < 4.0f)
			{
				spherecol.radius = spherecol.radius + 0.1f;
				//spherecol.center = new Vector3 (0, 0, 0);

				spherecol.center = new Vector3 (0, 0,zPos);
			}
			if (spherecol.enabled == false)
			{
				spherecol.enabled = true;
			}
		

			yield return new WaitForSeconds (0.1f);

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
				monsterDamage.HitDamage (armageddonDamage,character );
				armageddonDamage = 0;
			}

		}
		
	}

}
