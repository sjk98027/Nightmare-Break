using System.Collections.Generic;

public class SkillStorage {
   
	private Dictionary<string, Skill> skillDic = new Dictionary<string, Skill>();

	public Skill GetSkill(string key)
	{
		Skill dicValue = null;
		if (skillDic.ContainsKey(key))
		{
			dicValue = skillDic[key];
			return dicValue;
		} else
		{
			return null;
		}
	}

	public void Initialize()
	{
		#region 전사스킬초기화
		skillDic.Add("WarriorA1_L1", new Skill(Skill.SkillType.Active, "소용돌이", 1, 5, 1.2f, "캐릭터가 회전공격을하며 주위 적들에게 광역공격을한다."));
		skillDic.Add("WarriorA1_L2", new Skill(Skill.SkillType.Active, 2, 1.4f));
		skillDic.Add("WarriorA1_L3", new Skill(Skill.SkillType.Active, 3, 1.6f));
		skillDic.Add("WarriorA1_L4", new Skill(Skill.SkillType.Active, 4, 1.8f));

		skillDic.Add("WarriorA2_L1", new Skill(Skill.SkillType.Active, "섬단", 1, 3, 1.1f, "캐릭터가 빠른속도로 전방의 적들을 베어버린다."));
		skillDic.Add("WarriorA2_L2", new Skill(Skill.SkillType.Active, 2, 1.2f));
		skillDic.Add("WarriorA2_L3", new Skill(Skill.SkillType.Active, 3, 1.3f));
		skillDic.Add("WarriorA2_L4", new Skill(Skill.SkillType.Active, 4, 1.4f));

		skillDic.Add("WarriorA3_L1", new Skill(Skill.SkillType.Active, "검무", 1, 30, 1.2f, "캐릭터가 검으로 근접한 적들을 도륙하는 공격을한다."));
		skillDic.Add("WarriorA3_L2", new Skill(Skill.SkillType.Active, 2, 1.4f));
		skillDic.Add("WarriorA3_L3", new Skill(Skill.SkillType.Active, 3, 1.6f));
		skillDic.Add("WarriorA3_L4", new Skill(Skill.SkillType.Active, 4, 1.8f));

		skillDic.Add("WarriorA4_L1", new Skill(Skill.SkillType.Active, "기간틱소드", 1, 40, 1.2f, "캐릭터가 검으로 근접한 적들을 도륙하는 공격을한다."));
		skillDic.Add("WarriorA4_L2", new Skill(Skill.SkillType.Active, 2, 2.2f));

		skillDic.Add("WarriorP1_L1", new Skill(Skill.SkillType.Passive, "흡혈공격",1, 0.05f, "캐릭터가 일반공격시 준 데미지의 일정량을 체력으로 회복한다."));
		skillDic.Add("WarriorP1_L2", new Skill(Skill.SkillType.Passive, 2, 0.1f));
		skillDic.Add("WarriorP1_L3", new Skill(Skill.SkillType.Passive, 3, 0.15f));
		skillDic.Add("WarriorP1_L4", new Skill(Skill.SkillType.Passive, 4, 0.2f));

		skillDic.Add("WarriorP2_L1", new Skill(Skill.SkillType.Passive, "극의", 1, 0.5f, "캐릭터의 무기강화수치의 50%를 추가 방어력으로 얻는다."));
		skillDic.Add("WarriorP2_L2", new Skill(Skill.SkillType.Passive, 2, 1.0f));
		skillDic.Add("WarriorP2_L3", new Skill(Skill.SkillType.Passive, 3, 1.5f));
		skillDic.Add("WarriorP2_L4", new Skill(Skill.SkillType.Passive, 4, 2.0f));
		#endregion
	}
}

public class Skill
{
	public enum SkillType
	{
		Active,
		Passive
	}
	private string skillType;
	private string skillName;
	private string skillExplanation;
	private int skillLevel;
	private int skillCoolTime;
	private float skillDamage;
	private float passiveValue;

	public string SkillName { get { return skillName; } }
	public string SkillExplanation { get { return skillExplanation; } }
	public int SkillLevel { get { return skillLevel; } }
	public int SkillCoolTime { get { return skillCoolTime; } }
	public float SkillDamage { get { return skillDamage; } }
	public float PassiveValue { get { return passiveValue; } }    

	public Skill(SkillType _skillType, string _skillName, int _skillLevel, int _skillCoolTime, float _skillDamage, string _skillExplanation) //액티브스킬 생성자
	{ 
		skillType = _skillType.ToString();
		skillName = _skillName;
		skillLevel = _skillLevel;
		skillCoolTime = _skillCoolTime;
		skillDamage = _skillDamage;
		skillExplanation = _skillExplanation;
	}

	public Skill(SkillType _skillType, string _SkillName, int _skillLevel, float _passiveValue, string _skillExplanation) //패시브스킬 생성자
	{
		skillType = _skillType.ToString();
		skillLevel = _skillLevel;
		skillExplanation = _skillExplanation;
		passiveValue = _passiveValue;
	}

	public Skill(SkillType _skillType, int _skillLevel, float _skillAddValue) //스킬당 레벨업값 셋팅 생성자
	{
		if (_skillType == SkillType.Active)
		{
			skillDamage = _skillAddValue;
		} else
		{
			passiveValue = _skillAddValue;
		}
		skillLevel = _skillLevel;
	}
}
