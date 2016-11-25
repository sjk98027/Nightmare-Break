using UnityEngine;

[System.Serializable]
public class CharacterStatus : MonoBehaviour
{
    public enum Gender
    {
        Male = 0,
        FeMale = 1,
    }

    public enum CharClass
    {
        Warrior = 0,
        Mage,
        ShildWarrior,
        Gunner
    }

    public const int skillNum = 6;
    public const int equipNum = 4;
    public const int maxLevel = 20;

    string hName;
    Gender hGender;
    int level;
    CharClass charClass;
    int maxExp;
    int exp;

    int maxHealthPoint;
    int healthPoint;
    int maxMagicPoint;
    int magicPoint;
    int hpRegeneration;
    int mpRegeneration;
    int attack;
    int defense;

    float moveSpeed;
    int skillPoint;
    int dreamStone;
    int[] skillLevel;
    int[] equipLevel;
    public Skill[] skillSet;

    public string HName { get { return hName; } }
    public Gender HGender { get { return hGender; } }
    public int Level { get { return level; } }
    public CharClass HClass { get { return charClass; } }
    public int MaxExp { get { return maxExp; } }
    public int Exp { get { return exp; } }

    public int MaxHealthPoint { get { return maxHealthPoint; } }
    public int HealthPoint { get { return healthPoint; } }
    public int MaxMagicPoint { get { return maxMagicPoint; } }
    public int MagicPoint { get { return magicPoint; } }
    public int HpRegeneration { get { return hpRegeneration; } }
    public int MpRegeneration { get { return mpRegeneration; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }

    public float MoveSpeed { get { return moveSpeed; } }
    public int SkillPoint { get { return skillPoint; } }
    public int DreamStone { get { return dreamStone; } }
    public int[] SkillLevel { get { return skillLevel; } }
    public int[] EquipLevel { get { return equipLevel; } }
    public Skill[] SkillSet { get { return skillSet; } }

    public CharacterStatus()
    {
        name = "Hero";
        level = 0;
        charClass = 0;
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
        hGender = (Gender)characterStatusData.Gender;
        charClass = (CharClass)characterStatusData.HClass;
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

    public void DecreaseHealthPoint(int amount)
    {
        healthPoint -= amount;
    }

    public void DecreaseMagicPoint(int amount)
    {
        magicPoint -= amount;
    }
}
