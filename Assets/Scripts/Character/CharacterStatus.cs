
using UnityEngine;

[System.Serializable]
public class CharacterStatus : MonoBehaviour
{
    public enum CharClass
    {
        Warrior = 0,
        Mage,
        ShildWarrior,
        Gunner
    }
    //user name
    string name;

    // 0 = male, 1 = female
    int gender;

    //0 = warrior , 1 = mage , 2 = shildwarrior, 3 = guner
    int hClass;

    int level;

    public CharClass charClass;
    //char status
    //name
    public string characterName;
    public int characterNum;

    //exp
    public int demendExp;
    public int exp;

    //Point
    public int maxHealthPoint;
    public int maxManaPoint;

    public int healthPoint;
    public int magicPoint;

    //Regeneration
    int hpRegeneration;
    int mpRegeneration;


    //char AttackPoint and defensepoint
    public int attack;
    public int defense;

    //critical point
    public float charSpeed;

    public int dreamStone;

    public int[] skillLevel;
    public int[] equipLevel;

    //skillNum
    public Skill[] skillSet;
    public const int skillNum = 6;
    public const int equipNum = 4;
    public const int maxLevel = 20;


    public string CharacterName { get { return this.characterName; } }

    public int CharacterLevel { get { return this.Level; } }

    public int DemendExp { get { return this.demendExp; } }

    public int NeedExp { get { return this.exp; } }

    public int HealthPoint
    {
        get { return this.healthPoint; }
        set { healthPoint = value; }
    }

    public int ManaPoint { get { return this.magicPoint; } }
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

    public Skill[] SkillSet
    {
        get
        {
            return this.skillSet;
        }
        set
        {
            skillSet = value;
        }
    }

    public int Defense { get { return this.defense; } }
    public int DreamStone { get { return this.dreamStone; } }
    public float CharSpeed { get { return this.charSpeed; } }
    public string Name { get { return name; } }
    public int Gender { get { return gender; } }
    public int Level { get { return level; } }
    public int HClass { get { return hClass; } }
    public int Exp { get { return exp; } }
    public int MagicPoint { get { return magicPoint; } }
    public int HpRegeneration { get { return hpRegeneration; } }
    public int MpRegeneration { get { return mpRegeneration; } }
    public int Attack { get { return attack; } }
    public int[] SkillLevel { get { return skillLevel; } }
    public int[] EquipLevel { get { return equipLevel; } }

    public CharacterStatus(CharClass name)
    {
        if (name == CharClass.Warrior)
        {

            Debug.Log("charsta");
            exp = 1000;
            MaxHealthPoint = 1000;
            healthPoint = maxHealthPoint;
            attack = 10;
            maxManaPoint = 1000;
            magicPoint = maxManaPoint;
            charSpeed = 7;
            defense = 1;
            skillLevel = new int[skillNum];
            equipLevel = new int[equipNum];
        }
    }

    public void SetCharacterStatus()
    {
        exp = 1000;
        MaxHealthPoint = 1000;
        healthPoint = maxHealthPoint;
        attack = 10;
        maxManaPoint = 1000;
        magicPoint = maxManaPoint;
        defense = 1;
        charSpeed = 7;
        skillLevel = new int[skillNum];
        equipLevel = new int[equipNum];
    }


    public CharacterStatus()
    {
        name = "Hero";
        level = 0;
        hClass = 0;
        exp = 0;
        healthPoint = 0;
        magicPoint = 0;
        hpRegeneration = 0;
        mpRegeneration = 0;
        attack = 0;
        defense = 0;
        dreamStone = 0;
        skillLevel = new int[skillNum];
        equipLevel = new int[equipNum];
    }

    public void SetCharacterStatus(CharacterStatusData characterStatusData)
    {
        name = characterStatusData.Name;
        level = characterStatusData.Level;
        gender = characterStatusData.Gender;
        hClass = characterStatusData.HClass;
        exp = characterStatusData.Exp;
        healthPoint = characterStatusData.HealthPoint;
        magicPoint = characterStatusData.MagicPoint;
        hpRegeneration = characterStatusData.HpRegeneration;
        mpRegeneration = characterStatusData.MpRegeneration;
        attack = characterStatusData.Attack;
        defense = characterStatusData.Defense;
        dreamStone = characterStatusData.DreamStone;
        skillLevel = new int[skillNum];
        equipLevel = new int[equipNum];

        for (int i = 0; i < skillNum; i++)
        {
            skillLevel[i] = characterStatusData.SkillLevel[i];
        }

        for (int i = 0; i < equipNum; i++)
        {
            equipLevel[i] = characterStatusData.EquipLevel[i];
        }
    }
}
