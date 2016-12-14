using UnityEngine;
using System.Collections;

public class Armageddon : MonoBehaviour 
{

	public ParticleSystem FireBallparticleSystem;
	public CharacterStatus charStatus;
	public CharacterManager charManager;
	public GameObject character;
	public int armageddonDamage;
	public float FireBallSpeed;
	public Rigidbody FireBallRigid;
    private ParticleSystem ps;
	public SphereCollider spherecol;
	public GameObject armageddonImpact;
	int skillLv;
	public Animator armageddonAni;

    void Start()
    {
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = charManager.CharStatus;
        ps = GetComponent<ParticleSystem>();
		FireBallRigid = GetComponent<Rigidbody> ();
		armageddonAni = GetComponent<Animator> ();
		FireBallSpeed = 30f;
		FireBallRigid.velocity =((transform.forward - transform.up) * FireBallSpeed);
        
		spherecol = this.GetComponent<SphereCollider> ();
		skillLv = charStatus.SkillLevel [4];
		armageddonDamage =(int) ((SkillManager.instance.SkillData.GetSkill ((int)charStatus.HClass, 4).GetSkillData (skillLv).SkillValue)*  charStatus.Attack);
    }

	void Update ()
	{
		
	}
	public void Destroy()
	{
		
		armageddonAni.SetBool ("Strike",true);
		armageddonImpact = Instantiate(Resources.Load<GameObject>("Effect/ArmageddonImpact"), transform.position, Quaternion.identity) as GameObject;
		Instantiate (Resources.Load<GameObject> ("Effect/ArmageddonExplosion"), transform.position, Quaternion.identity); //as GameObject;
		Instantiate (Resources.Load<GameObject> ("Effect/MeteorExplosion"), transform.position, Quaternion.identity); //as GameObject;
		Destroy (armageddonImpact, 0.4f);

		Destroy (this.gameObject, 1f);

		
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			
		}
		
	}


}
