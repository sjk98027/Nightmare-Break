using UnityEngine;
using System.Collections;

public class charWeapon : MonoBehaviour {

	public GameObject character;
	public CharacterManager charManager;

	public WarriorManager warriorManager;

	public CharacterStatus charstate;
	public int damage= 0;
	bool normalAttack;
	bool skillAttack;
	public bool checkHit;

	public bool NormalAttack {get {return this.normalAttack;}}

	public bool SkillAttack {get {return this.skillAttack;}}

	// Use this for initialization
	void Start () 
	{
		checkHit = false;
		character = GameObject.FindWithTag ("Player");
		warriorManager = character.GetComponent<WarriorManager> ();
		charstate = warriorManager.charstate;
	}
	
	// Update is called once per frame
	void Update () 
	{
		normalAttack = warriorManager.NormalAttackState;
		skillAttack = warriorManager.SkillAttackState;
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{

			checkHit = true;	
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();



			if (monsterDamage != null)
			{	
				if (normalAttack)
				{
					damage = warriorManager.BasicDamage;
				
					int testPassiveHP;
					charstate.skillLevel [4] = 3;
					Debug.Log ("in passive");
					testPassiveHP = (int)(damage * charstate.skillLevel [4] * 30f);

					charstate.healthPoint += testPassiveHP;

					Debug.Log (damage);
					Debug.Log (charstate.healthPoint);
					Debug.Log (testPassiveHP);
				}
				else if (skillAttack)
				{
					damage = warriorManager.BasicDamage;



				}
				if (damage != 0)
				{
					
					monsterDamage.HitDamage (damage, this.gameObject.GetComponentInParent<CharacterManager> ().gameObject);
					damage = 0;
				}
			}
		}
	}
}
