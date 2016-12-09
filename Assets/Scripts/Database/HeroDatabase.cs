using System;
using System.Collections.Generic;
using UnityEngine;

public enum HeroId
{
    None = 0,
    Warrior,
    Mage,
}

public class HeroDatabase
{
    private static HeroDatabase instance;

    public static HeroDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HeroDatabase();
            }

            return instance;
        }
    }

    List<HeroBaseData> heroData;

    public void InitializeHeroDatabase()
    {
        heroData = new List<HeroBaseData>();

        AddBaseData(new HeroBaseData((int)HeroId.None, "None"));
        AddBaseData(new HeroBaseData((int)HeroId.Warrior, "Warrior"));
        AddBaseData(new HeroBaseData((int)HeroId.Mage, "Mage"));

        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(1, 10, 0, 150, 100, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(2, 7, 1, 220, 60, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(3, 9, 2, 300, 70, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(4, 12, 3, 390, 110, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(5, 15, 5, 490, 150, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(6, 18, 7, 600, 190, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(7, 21, 9, 720, 230, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(8, 25, 11, 850, 270, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(9, 29, 13, 990, 310, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Warrior).AddLevelData(new HeroLevelData(10, 33, 15, 1240, 350, 1, 1, 7, 100));

        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(1, 3, 0, 35, 4, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(2, 4, 0, 45, 4, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(3, 5, 0, 55, 4, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(4, 6, 0, 75, 5, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(5, 7, 0, 95, 5, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(6, 8, 0, 115, 5, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(7, 10, 0, 145, 5, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(8, 12, 0, 175, 5, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(9, 14, 0, 205, 6, 1, 1, 7, 100));
        GetBaseData((int)HeroId.Mage).AddLevelData(new HeroLevelData(10, 16, 0, 235, 6, 1, 1, 7, 100));
    }

    public bool AddBaseData(HeroBaseData newHeroData)
    {
        try
        {
            heroData.Add(newHeroData);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("HeroDatabase::AddBaseData 에러 - " + e.Message);
            return false;
        }

    }

    public HeroBaseData GetBaseData(int Id)
    {
        foreach (HeroBaseData baseData in heroData)
        {
            if (baseData.Id == Id)
            {
                return baseData;
            }
        }

        return null;
    }
}

public class HeroBaseData
{
    int id;
    string name;
    List<HeroLevelData> heroLevelData;

    public int Id { get { return id; } }
    public string Name { get { return name; } }
    public List<HeroLevelData> HeroLevelData { get { return heroLevelData; } }

    public HeroBaseData()
    {
        id = 0;
        name = "";
        heroLevelData = new List<HeroLevelData>();
    }

    public HeroBaseData(int _id, string _name)
    {
        id = _id;
        name = _name;
        heroLevelData = new List<HeroLevelData>();
    }

    public HeroLevelData GetLevelDataData(int level)
    {
        for (int index = 0; index < heroLevelData.Count; index++)
        {
            if (heroLevelData[index].Level == level)
            {
                return heroLevelData[index];
            }
        }

        return null;
    }

    public bool AddLevelData(HeroLevelData newHeroLevelData)
    {
        try
        {
            heroLevelData.Add(newHeroLevelData);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class HeroLevelData
{
    int level;
    int attack;
    int defense;
    int healthPoint;
    int magicPoint;
    int hpRegeneration;
    int mpRegeneration;
    int moveSpeed;
    int maxExp;

    public int Level { get { return level; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int HealthPoint { get { return healthPoint; } }
    public int MagicPoint { get { return magicPoint; } }
    public int HpRegeneration { get { return hpRegeneration; } }
    public int MpRegeneration { get { return mpRegeneration; } }
    public int MoveSpeed { get { return moveSpeed; } }
    public int MaxExp { get { return maxExp; } }

    public HeroLevelData()
    {
        level = 1;
        attack = 0;
        defense = 0;
        healthPoint = 0;
        magicPoint = 0;
        hpRegeneration = 0;
        mpRegeneration = 0;
        moveSpeed = 1;
        maxExp = 0;
    }

    public HeroLevelData(int newLevel, int newAttack, int newDefense, int newHealthPoint, int newMagicPoint, int newHpRegeneration, int newMpRegeneration, int newMoveSpeed, int newMaxExp)
    {
        level = newLevel;
        attack = newAttack;
        defense = newDefense;
        healthPoint = newHealthPoint;
        magicPoint = newMagicPoint;
        hpRegeneration = newHpRegeneration;
        mpRegeneration = newMpRegeneration;
        moveSpeed = newMoveSpeed;
        maxExp = newMaxExp;
    }
}


