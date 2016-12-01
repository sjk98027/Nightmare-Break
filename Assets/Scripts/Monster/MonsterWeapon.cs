using UnityEngine;
using System.Collections;

public class MonsterWeapon : MonoBehaviour {
	
	public Monster monster;
	public BoxCollider AttackCollider;
	public int damage;

	public void MonsterWeaponSet(){
		monster = this.GetComponentInParent<Monster> ();
		damage = monster.BaseDamage;
		AttackCollider = this.GetComponent<BoxCollider> ();
		AttackColliderOff ();
	}



	public void AttackColliderOff(){
		AttackCollider.enabled=false;
	}
	public  void AttackColliderOn(){
		AttackCollider.enabled = true;
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player"))
		{
			CharacterManager CharObject = coll.gameObject.GetComponent<CharacterManager> ();
			if (damage != 0)
			{
				CharObject.HitDamage (damage);
			}
		}
	}

	public void AttackColliderSizeChange(Vector3 _size){
		AttackCollider.size = _size;
	}

}