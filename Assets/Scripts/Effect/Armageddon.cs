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

	public GameObject armageddonImpact;
	public GameObject armageddonExplosion;
	public GameObject meteorExplosion;
	public BoxCollider armageddonBox;
	int skillLv;
	public Animator armageddonAni;
	public ArmageddonPiece[] armageddonPiece;
	public bool pieceAttack;

    void Start()
    {
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = charManager.CharStatus;
        ps = GetComponent<ParticleSystem>();
		FireBallRigid = GetComponent<Rigidbody> ();
		armageddonAni = GetComponent<Animator> ();
		armageddonBox = this.GetComponent<BoxCollider> ();
		FireBallSpeed = 30f;
		pieceAttack = false;
		armageddonPiece = this.gameObject.GetComponentsInChildren<ArmageddonPiece> ();
		FireBallRigid.velocity =((transform.forward - transform.up) * FireBallSpeed);
		skillLv = charStatus.SkillLevel [4];
		//armageddonDamage =(int) ((SkillManager.instance.SkillData.GetSkill ((int)charStatus.HClass, 4).GetSkillData (skillLv).SkillValue)*  charStatus.Attack);
		armageddonDamage = 100;
	}
	public void Destroy()
	{
		for (int i = 0; i < armageddonPiece.Length; i++)
		{
			armageddonPiece [i].PieceAttack();
		}
		armageddonAni.SetBool ("Strike",true);
		armageddonImpact = Instantiate(Resources.Load<GameObject>("Effect/ArmageddonImpact"), transform.position, Quaternion.identity) as GameObject;
		armageddonExplosion =Instantiate (Resources.Load<GameObject> ("Effect/ArmageddonExplosion"), transform.position, Quaternion.identity)as GameObject;
		meteorExplosion = Instantiate (Resources.Load<GameObject> ("Effect/MeteorExplosion"), transform.position, Quaternion.identity)as GameObject;
		Destroy (armageddonImpact, 0.4f);
		Destroy (armageddonExplosion, 0.4f);
		Destroy (meteorExplosion, 0.4f);
		armageddonBox.enabled = false;

		Destroy (this.gameObject, 2f);

		
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in Armageddon");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				Debug.Log (character);
				monsterDamage.HitDamage (armageddonDamage,character );
				armageddonDamage = 0;

			}
		}
		
	}


}
