using UnityEngine;
using System.Collections;

public class charWeapon : MonoBehaviour {

	public GameObject character;
	public CharacterManager charManager;
	public CharacterStatus charstate;
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
		charstate = charManager.charstate;
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
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

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

				if (damage != 0)
				{
					if (charstate.charClass == CharacterStatus.CharClass.Warrior)
					{
						if (charstate.skillLevel [4] < 4)
						{
							if (normalAttack)
							{
								int testPassiveHP;

								testPassiveHP = (int)(damage * charstate.skillLevel [4] * 0.1f);
								charstate.healthPoint += testPassiveHP;
							}
														
						}
						else if(charstate.skillLevel [4] == 4)
						{
							Debug.Log ("in Warrior");
							int testPassiveHP;

							testPassiveHP = (int)(damage * charstate.skillLevel [4] * 0.1f);
							charstate.healthPoint += testPassiveHP;
						}

					}
					monsterDamage.HitDamage (damage, this.gameObject.GetComponentInParent<CharacterManager> ().gameObject);
					damage = 0;
				}
			}
		}
	}
}
