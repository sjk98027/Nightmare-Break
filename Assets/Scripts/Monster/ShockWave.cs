using UnityEngine;
using System;
using System.Collections;

public class ShockWave : MonoBehaviour
{
	public ShockWaveMonster AttackMonster;
	public int damage;
	[SerializeField] GameObject checkTempData;
	[SerializeField] CharacterManager tempData;

	public void GetDamage (int _damage, ShockWaveMonster _AttackMonster)
	{
		AttackMonster = _AttackMonster;
		damage = _damage;
	}

	void OnParticleCollision (GameObject objectData)
	{
		checkTempData = objectData.gameObject;
		try
		{
			tempData = checkTempData.GetComponent<CharacterManager> ();
			tempData.HitDamage (damage);
		} catch (NullReferenceException e)
		{
		}
	}
}
