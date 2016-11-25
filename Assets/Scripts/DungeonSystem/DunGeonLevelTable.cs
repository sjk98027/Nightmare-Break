using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//rongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrongrong
public enum DungeonType
{
	None = 0,
	Weapon,
	Helmet,
	Armor,
	Gloves,
	Shoes,
	Ring,
	Necklace,
}

//dreamStoneCount

public class DungeonDatabase
{
	private static DungeonDatabase instance;

	public static DungeonDatabase Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new DungeonDatabase();
			}

			return instance;
		}        
	}

	public Dictionary<int, Dungeon> DungeonData;

	public void InitializeDungeonDatabase()
	{
		DungeonData = new Dictionary<int, Dungeon>();
		//DungeonData.Add ();
//		DungeonData.Add(0, new Dungeon(0, DungeonType.None, "빈칸", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(1, new Dungeon(1, DungeonType.Weapon, "작은검", 100, 2, 0, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(2, new Dungeon(2, DungeonType.Weapon, "검", 200, 3, 0, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(3, new Dungeon(3, DungeonType.Weapon, "대검", 300, 5, 0, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(4, new Dungeon(4, DungeonType.Weapon, "화염의 검", 500, 8, 0, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(5, new Dungeon(5, DungeonType.Weapon, "드래곤뼈 검", 1000, 11, 0, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(6, new Dungeon(6, DungeonType.Helmet, "가죽 모자", 100, 0, 0, 0, 3, 0, 0, 0, 0, 0));
//		DungeonData.Add(7, new Dungeon(7, DungeonType.Helmet, "녹슨 투구", 200, 0, 0, 0, 6, 0, 0, 0, 0, 0));
//		DungeonData.Add(8, new Dungeon(8, DungeonType.Helmet, "강철 투구", 300, 0, 0, 0, 10, 0, 0, 0, 0, 0));
//		DungeonData.Add(9, new Dungeon(9, DungeonType.Helmet, "미스릴 투구", 500, 0, 0, 0, 15, 0, 0, 0, 0, 0));
//		DungeonData.Add(10, new Dungeon(10, DungeonType.Helmet, "드래곤비늘 투구", 1000, 0, 0, 0, 20, 0, 0, 0, 0, 0));
//		DungeonData.Add(11, new Dungeon(11, DungeonType.Armor, "가죽 갑옷", 100, 0, 1, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(12, new Dungeon(12, DungeonType.Armor, "녹슨 갑옷", 200, 0, 2, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(13, new Dungeon(13, DungeonType.Armor, "체인 갑옷", 300, 0, 3, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(14, new Dungeon(14, DungeonType.Armor, "미스릴 갑옷", 500, 0, 4, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(15, new Dungeon(15, DungeonType.Armor, "드래곤비늘 갑옷", 1000, 0, 5, 0, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(16, new Dungeon(16, DungeonType.Gloves, "천 장갑", 100, 0, 0, 0, 0, 0, 0, 0.1f, 0, 0));
//		DungeonData.Add(17, new Dungeon(17, DungeonType.Gloves, "가죽 장갑", 200, 0, 0, 0, 0, 0, 0, 0.15f, 0, 0));
//		DungeonData.Add(18, new Dungeon(18, DungeonType.Gloves, "건틀릿", 300, 0, 0, 0, 0, 0, 0, 0.2f, 0, 0));
//		DungeonData.Add(19, new Dungeon(19, DungeonType.Gloves, "강철 건틀릿", 500, 0, 0, 0, 0, 0, 0, 0.25f, 0, 0));
//		DungeonData.Add(20, new Dungeon(20, DungeonType.Gloves, "드래곤비늘 건틀릿", 1000, 0, 0, 0, 0, 0, 0, 0.3f, 0, 0));
//		DungeonData.Add(21, new Dungeon(21, DungeonType.Shoes, "천 신발", 100, 0, 0, 0, 0, 0, 0.1f, 0, 0, 0));
//		DungeonData.Add(22, new Dungeon(22, DungeonType.Shoes, "가죽 신발", 200, 0, 0, 0, 0, 0, 0.15f, 0, 0, 0));
//		DungeonData.Add(23, new Dungeon(23, DungeonType.Shoes, "털 신발", 300, 0, 0, 0, 0, 0, 0.2f, 0, 0, 0));
//		DungeonData.Add(24, new Dungeon(24, DungeonType.Shoes, "강철 신발", 500, 0, 0, 0, 0, 0, 0.25f, 0, 0, 0));
//		DungeonData.Add(25, new Dungeon(25, DungeonType.Shoes, "드래곤비늘 신발", 1000, 0, 0, 0, 0, 0, 0.3f, 0, 0, 0));
//		DungeonData.Add(26, new Dungeon(26, DungeonType.Ring, "돌 반지", 100, 0, 0, 1, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(27, new Dungeon(27, DungeonType.Ring, "은 반지", 200, 0, 0, 2, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(28, new Dungeon(28, DungeonType.Ring, "금 반지", 300, 0, 0, 3, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(29, new Dungeon(29, DungeonType.Ring, "다이아몬드 반지", 500, 0, 0, 4, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(30, new Dungeon(30, DungeonType.Ring, "마법 깃든 반지", 1000, 0, 0, 5, 0, 0, 0, 0, 0, 0));
//		DungeonData.Add(31, new Dungeon(31, DungeonType.Necklace, "돌 목걸이", 100, 0, 0, 0, 0, 0, 0, 0, 1, 0));
//		DungeonData.Add(32, new Dungeon(32, DungeonType.Necklace, "은 목걸이", 200, 0, 0, 0, 0, 0, 0, 0, 0, 1));
//		DungeonData.Add(33, new Dungeon(33, DungeonType.Necklace, "금 목걸이", 300, 0, 0, 0, 0, 0, 0, 0, 1, 2));
//		DungeonData.Add(34, new Dungeon(34, DungeonType.Necklace, "다이아몬드 목걸이", 500, 0, 0, 0, 0, 0, 0, 0, 1, 3));
//		DungeonData.Add(35, new Dungeon(35, DungeonType.Necklace, "마법 깃든 목걸이", 1000, 0, 0, 0, 0, 0, 0, 0, 2, 3));
	}
}

public class Dungeon
{
	int Id;
	DungeonType type;
	string name;
	int dreamStoneCount;
	int attack;
	int defense;
	int magicDefense;
	int health;
	int mana;
	float moveSpeed;
	float attackSpeed;
	int healthRegeneration;
	int manaRegeneration;

	public int ID { get { return Id; } }
	public DungeonType Type { get { return type; } }
	public string Name { get { return name; } }
	public int DreamStoneCount { get { return dreamStoneCount; } }
	public int Attack { get { return attack; } }
	public int Defense { get { return defense; } }
	public int MagicDefense { get { return magicDefense; } }
	public int Health { get { return health; } }
	public int Mana { get { return mana; } }
	public float MoveSpeed { get { return moveSpeed; } }
	public float AttackSpeed { get { return attackSpeed; } }
	public int HealthRegeneration { get { return healthRegeneration; } }
	public int ManaRegeneration { get { return manaRegeneration; } }

//	public Dungeon()
//	{
//		Id = 0;
//		type = DungeonType.None;
//		gold = 0;
//		attack = 0;
//		defense = 0;
//		magicDefense = 0;
//		health = 0;
//		mana = 0;
//		moveSpeed = 0;
//		attackSpeed = 0;
//		healthRegeneration = 0;
//		manaRegeneration = 0;
//	}
//
//	public Dungeon(int newId, DungeonType newType, string newName, int newGold, int newAttack, int newDefense, int newMagicDefense, int newHealth, int newMana,
//		float newMoveSpeed, float newAttackSpeed, int newHealthRegeneration, int newManaRegeneration)
//	{
//		Id = newId;
//		type = newType;
//		name = newName;
//		gold = newGold;
//		attack = newAttack;
//		defense = newDefense;
//		magicDefense = newMagicDefense;
//		health = newHealth;
//		mana = newMana;
//		moveSpeed = newMoveSpeed;
//		attackSpeed = newAttackSpeed;
//		healthRegeneration = newHealthRegeneration;
//		manaRegeneration = newManaRegeneration;
//	}
}