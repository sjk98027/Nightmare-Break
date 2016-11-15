using UnityEngine;
using System.Collections;

public class charWeapon : MonoBehaviour {

	public GameObject character;
	public CharacterManager charManager;
//	public CharacterStatus charStatus

	public int damage= 0;
	bool normalAttack;
	bool skillAttack;

	public bool NormalAttack {get {return this.normalAttack;}}

	public bool SkillAttack {get {return this.skillAttack;}}

	// Use this for initialization
	void Start () 
	{
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		normalAttack = charManager.NormalAttackState;
		skillAttack = charManager.SkillAttackState;
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			
			TestMonster monsterDamage = coll.gameObject.GetComponent<TestMonster> ();

			if (monsterDamage != null)
			{
				if (normalAttack)
				{
					
					damage = charManager.BasicDamage;

				
				}
				else if (skillAttack)
				{
					damage = charManager.BasicDamage;
				}
				if(damage !=0)
				{

					monsterDamage.HitDamage(damage);
					damage = 0;
				}
			}
		}
	}
}
