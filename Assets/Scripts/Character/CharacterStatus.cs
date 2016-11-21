
using UnityEngine;

[System.Serializable]
public class CharacterStatus
{
	public enum CharClass
	{
		Warrior = 0,
		Mage,
		ShildWarrior,
		Gunner	
	}

	public CharClass charClass;
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

	public float CharSpeed {get {return this.charSpeed;}}

	public CharacterStatus(CharClass name)
	{
		if (name == CharClass.Warrior)
		{
			
			Debug.Log ("charsta");
			needExp = 1000;
			MaxHealthPoint = 1000;
			healthPoint = maxHealthPoint;

			maxManaPoint = 1000;
			magicPoint = maxManaPoint;
			charSpeed = 7;
		}
	}
}
