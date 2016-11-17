using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Skill
{
	static Skill defaultSkill;

	[SerializeField] protected int skillId;
	[SerializeField] protected string skillName;
	[SerializeField] protected int skillCost;
	[SerializeField] protected int skillLevel;
	[SerializeField] protected int skillDamage;
	[SerializeField] protected int skillCoolTime;
	[SerializeField] protected Sprite skillIcon;






	static Skill()
	{
		defaultSkill = new Skill ();
	}

	public Skill()
	{
		skillId = 0;
		skillName = "Default";
		skillLevel = 0;
		skillCoolTime = 0;
	}

	public virtual void UseSkill()
	{
		
	}


}
