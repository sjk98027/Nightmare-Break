
using UnityEngine;

[System.Serializable]
public class CharacterStatus
{
	//char status
	//name
	public string characterName;
	public int characterNum;

	//Level
	public int characterLevel;
    public int maxHealthPoint;
    public int maxManaPoint;

	//exp
	public int demendExp;
	public int needExp;

	//Point
	public int healthPoint;
	public int magicPoint;

	//char AttackPoint and defensepoint
	public int defense;

	//critical point
	public float critical;
	public float charSpeed;

	public int dreamStone;


	//char stat

	//public bool Alive{get {return this.alive;}}

	public float[] skillCoolTime;

	public Skill[] activeSkillSet;

	public Skill[] passiveSkillSet;

	public bool[] onSkill;

	public string CharacterName {get {return this.characterName;}}

	public int CharacterLevel {	get {return this.characterLevel;}}

	public int DemendExp { get {return this.demendExp;}	}

	public int NeedExp {get {return this.needExp;}}

	public int HealthPoint {get {return this.healthPoint;}
		set {healthPoint = value;}}
	
	public int ManaPoint {get {return this.magicPoint;}}

    public int MaxHealthPoint
    {
        get { return this.maxHealthPoint; }
        set { maxHealthPoint = value; }
    }

    public int MaxManaPoint
    {
        get { return this.maxManaPoint; }
        set { maxManaPoint = value; }
    }

    public int Defense {get {return this.defense;}}

	public float Critical {get {return this.critical;}}

	public int DreamStone {	get {return this.dreamStone;}}

	public float[] SkillCoolTime {get {return this.skillCoolTime;}}


	public Skill[] ActiveSkillSet {get {return this.activeSkillSet;}}

	public Skill[] PassiveSkillSet {get {return this.passiveSkillSet;}}

	public bool[] OnSkill {	get {return this.onSkill;}}

	public float CharSpeed {get {return this.charSpeed;}}

	public CharacterStatus()
	{
		characterName = "default";

	}



	public CharacterStatus (CharacterStatus status)
	{
		characterName = status.characterName;
		characterNum = status.characterNum;
		characterLevel = status.characterLevel;
		needExp = 1000;
		MaxHealthPoint = 1000;
		healthPoint =maxHealthPoint;

		maxManaPoint = 1000;
		magicPoint = maxManaPoint;
		charSpeed = 7;
		SkillInitalize ();
	}

	public void SkillInitalize()
	{
		activeSkillSet = new Skill[4];

		for (int i = 0; i < activeSkillSet.Length; i++)
		{
			activeSkillSet [i] = new Skill ();
		}

		passiveSkillSet = new Skill[2];
		for (int i = 0; i < passiveSkillSet.Length; i++)
		{
			passiveSkillSet [i] = new Skill ();
		}
		onSkill = new bool[6];
		skillCoolTime = new float[6];		
	}
}
