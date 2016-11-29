using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MonsterName{
	None=0,
	Duck,
	Frog,
	Rabbit,
	Bear
}
public class MonsterBaseData{
	int id;
	public int Id{get { return id;}}
	List<MonsterStatusData> monsterStatusData;

	public List<MonsterStatusData> MonsterStatusData { get { return monsterStatusData; } }

	public MonsterBaseData()
	{
		id = 0;
		monsterStatusData = new List<MonsterStatusData>();
	}

	public MonsterBaseData(int _id){
		id = _id;
		monsterStatusData = new List<MonsterStatusData> ();
	}

//	public MonsterBaseData(int newId, float newAttackRange, float newColliderSize)
//	{
//		Id = newId;
//		monsterStatusData = new List<MonsterStatusData>();
//	}
//
//	public MonsterBaseData(MonsterStatusData newBaseData)
//	{
//		Id = newBaseData.ID;
//		monsterStatusData = new List<MonsterStatusData>();
//	}

	public MonsterStatusData GetStatusData(int level)
	{
		if (monsterStatusData.Count >= level)
		{
			return monsterStatusData[level];
		}

		return null;
	}

	public bool AddStatusData(MonsterStatusData newHeroStatusData)
	{
		try
		{
			monsterStatusData.Add(newHeroStatusData);
			return true;
		}
		catch
		{
			return false;
		}
	}

//	public bool RemoveStatusData(int level)
//	{
//		int index = FindLevelDataIndex(level);
//
//		if (index != -1)
//		{
//			try
//			{
//				monsterStatusData.Remove(monsterStatusData[index]);
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}
//		else
//		{
//			return false;
//		}
//	}
//
//	public bool ChangeStatusData(MonsterStatusData heroStatusData)
//	{
//		int index = FindStatusDataIndex(heroStatusData.Level);
//
//		if (index != -1)
//		{
//			try
//			{
//				statusData[index] = heroStatusData;
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}
//		else
//		{
//			return false;
//		}
//	}
//
//	public int FindStatusDataIndex(int level)
//	{
//		for (int i = 0; i < StatusData.Count; i++)
//		{
//			if (StatusData[i].Level == level)
//			{
//				return i;
//			}
//		}
//
//		return -1;
//	}

}

public class MonsterDataTotal
{
	private static MonsterDataTotal instance;

	public static MonsterDataTotal Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new MonsterDataTotal();
			}

			return instance;
		}        
	}
	//public Dictionary<int, MonsterData> MonsterData;

	List<MonsterBaseData> MonsterData;

//	public Dictionary<int, Dungeon> DungeonData;

	public void InitializeDungeonDatabase()
	{

		MonsterData = new List<MonsterBaseData> ();
		MonsterData.Add (new MonsterBaseData ((int)MonsterName.None));
		MonsterData.Add (new MonsterBaseData ((int)MonsterName.Duck));
		MonsterData.Add (new MonsterBaseData ((int)MonsterName.Frog));
		MonsterData.Add (new MonsterBaseData ((int)MonsterName.Rabbit));
		MonsterData.Add (new MonsterBaseData ((int)MonsterName.Bear));

		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (1, "Duck", 1, 2, 900, 0, 1, 0, 1));// right 3 -> X;
		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (2, "Duck", 2, 3, 945, 0, 1, 0, 1));
		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (3, "Duck", 3, 5, 1039, 0, 1, 0, 1));
		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (4, "Duck", 4, 6, 1195, 1, 1, 0, 1));
		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (5, "Duck", 5, 8, 1434, 2, 1, 0, 1));
		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (6, "Duck", 6, 10, 1793, 2, 1, 0, 1));
		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (7, "Duck", 7, 13, 2331, 3, 1, 0, 1));
		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (8, "Duck", 8, 15, 3146, 3, 1, 0, 1));
		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (9, "Duck", 9, 19, 4405, 4, 1, 0, 1));
		MonsterData [(int)MonsterName.Duck].AddStatusData (new MonsterStatusData (10, "Duck", 10, 24, 6388, 5, 1, 0, 1));

		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (1, "Frog", 1, 2, 900, 0, 1, 0, 1));// right 3 -> X;
		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (2, "Frog", 2, 3, 945, 0, 1, 0, 1));
		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (3, "Frog", 3, 5, 1039, 0, 1, 0, 1));
		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (4, "Frog", 4, 6, 1195, 1, 1, 0, 1));
		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (5, "Frog", 5, 8, 1434, 2, 1, 0, 1));
		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (6, "Frog", 6, 10, 1793, 2, 1, 0, 1));
		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (7, "Frog", 7, 13, 2331, 3, 1, 0, 1));
		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (8, "Frog", 8, 15, 3146, 3, 1, 0, 1));
		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (9, "Frog", 9, 19, 4405, 4, 1, 0, 1));
		MonsterData [(int)MonsterName.Frog].AddStatusData (new MonsterStatusData (10, "Frog", 10, 24, 6388, 5, 1, 0, 1));

		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (1, "Rabbit", 1, 5, 900, 0, 1, 0, 1));// right 3 -> X;
		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (2, "Rabbit", 2, 7, 945, 0, 1, 0, 1));
		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (3, "Rabbit", 3, 10, 1039, 0, 1, 0, 1));
		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (4, "Rabbit", 4, 12, 1195, 1, 1, 0, 1));
		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (5, "Rabbit", 5, 16, 1434, 2, 1, 0, 1));
		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (6, "Rabbit", 6, 21, 1793, 4, 1, 0, 1));
		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (7, "Rabbit", 7, 26, 2331, 8, 1, 0, 1));
		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (8, "Rabbit", 8, 31, 3146, 9, 1, 0, 1));
		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (9, "Rabbit", 9, 38, 4405, 12, 1, 0, 1));
		MonsterData [(int)MonsterName.Rabbit].AddStatusData (new MonsterStatusData (10, "Rabbit", 10, 48, 6388, 15, 1, 0, 1));

		MonsterData [(int)MonsterName.Bear].AddStatusData (new MonsterStatusData (1, "Bear", 1, 10, 4500, 0, 1, 1, 115));// right 1 -> exp; 2-> stone ; 3-> moveSpeed;
		MonsterData[(int)MonsterName.Bear].AddStatusData(new MonsterStatusData(2,"Bear",2,15,4725,0,1,2,309));	
		MonsterData[(int)MonsterName.Bear].AddStatusData(new MonsterStatusData(3,"Bear",3,20,5198,0,1,3,425));	
		MonsterData[(int)MonsterName.Bear].AddStatusData(new MonsterStatusData(4,"Bear",4,25,5977,2,1,4,596));	
		MonsterData[(int)MonsterName.Bear].AddStatusData(new MonsterStatusData(5,"Bear",5,33,7173,4,1,5,710));
		MonsterData[(int)MonsterName.Bear].AddStatusData(new MonsterStatusData(6,"Bear",6,42,8966,8,1,6,825));
		MonsterData[(int)MonsterName.Bear].AddStatusData(new MonsterStatusData(7,"Bear",7,52,11655,12,1,8,1065));
		MonsterData[(int)MonsterName.Bear].AddStatusData(new MonsterStatusData(8,"Bear",8,63,15735,18,1,10,1456));
		MonsterData[(int)MonsterName.Bear].AddStatusData(new MonsterStatusData(9,"Bear",9,77,22029,24,1,12,1857));
		MonsterData[(int)MonsterName.Bear].AddStatusData(new MonsterStatusData(10,"Bear",10,97,31942,30,1,16,2483));
	}

	public MonsterBaseData GetHeroData(int Id)
		{
		if(MonsterData.Count > Id)
			{
			for(int i = 0; i< MonsterData.Count; i++)
				{
				if(MonsterData[i].Id == Id)
					{
					return MonsterData[i];
					}
				}
				return null;
			}
			else
			{
				return null;
			}
		}

}



public class MonsterStatusData{
	int id;
	string monsterName;
	int monsterLevel;
	int monsterDamage;
	int monsterHp;
	int monsterDefense;
	float monsterMoveSpeed;
	int dreamStoneCount;
	int monsterExp;


	public int ID { get { return id; } }
	public string MonsterName{get{ return monsterName;}}
	public int MonsterLevel{get{ return monsterLevel;}}
	public int MonsterDamage{ get { return monsterDamage; } }
	public int MonsterHp{get{ return monsterHp;}}
	public int MonsterDefense{get{ return monsterDefense;}}
	public int DreamStoneCount{get{ return dreamStoneCount;}}
	public int MonsterExp{get{ return monsterExp;}}

	public float MonsterMoveSpeed{get{ return monsterMoveSpeed;}}

	public MonsterStatusData(){
		id = 0;
		monsterName = "니가가라화와이";
		monsterLevel = 1;
		monsterDamage = 0;
		monsterHp = 0;
		monsterDefense = 0;
		monsterMoveSpeed = 1;
		dreamStoneCount = 1;
		monsterExp = 115;
	}

	public MonsterStatusData(int _id,
		string _monsterName,
		int _monsterLevel,
		int _monsterDamage,
		int _monsterHp,
		int _monsterDefense,
		float _monsterMoveSpeed,
		int _dreamStoneCount,
		int _monsterExp

	)
	{
		id = _id;
		monsterName = _monsterName;
		monsterLevel = _monsterLevel;
		monsterDamage = _monsterDamage;
		monsterHp = _monsterHp;
		monsterDefense = _monsterDefense;
		monsterMoveSpeed = _monsterMoveSpeed;
		dreamStoneCount = _dreamStoneCount;
		monsterExp = _monsterExp;
	}
}



public enum UnitID
{
	None,
	Unity,

}
//
//public enum State
//{
//	Idle = 1,
//	Move,
//	Attack,
//	Die,
//	Skill1,
//	Skill2,
//	Skill3,
//}

//public class HeroDatabase
//{
//	private static HeroDatabase instance;
//
//	public static HeroDatabase Instance
//	{
//		get
//		{
//			if(instance == null)
//			{
//				instance = new HeroDatabase();
//			}
//
//			return instance;
//		}
//	}
//
//	List<HeroBaseData> HeroData;
//
//	public void InitializeHeroDatabase()
//	{
//		HeroData = new List<HeroBaseData>();
//
//		//이름, 공격범위, 충돌범위 
//		HeroData.Add(new HeroBaseData((int)UnitID.None, 1.0f, 1.0f));
//		HeroData.Add(new HeroBaseData((int)UnitID.Unity, 1.0f, 1.0f));
//
//		//레벨, 경험치, 공격력, 방어력, 마법방어력, 체력, 마나, 이동속도, 공격속도, 체력리젠, 마나리젠
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(1, 100, 5, 0, 0, 40, 5, 5, 0.8f, 1, 0, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(2, 200, 7, 0, 0, 45, 7, 5, 0.85f, 1, 0, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(3, 400, 9, 0, 0, 50, 9, 5, 0.9f, 1, 0, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(4, 750, 11, 0, 0, 55, 11, 5, 0.95f, 1, 0, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(5, 1300, 13, 0, 0, 60, 13, 5, 1f, 1, 1, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(6, 2100, 15, 0, 0, 65, 15, 5, 1.05f, 1, 1, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(7, 3200, 17, 1, 0, 71, 17, 5, 1.1f, 1, 1, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(8, 4650, 19, 2, 0, 77, 19, 5, 1.15f, 1, 1, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(9, 6500, 21, 3, 0, 83, 21, 5, 1.2f, 1, 2, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(10, 8800, 23, 4, 0, 89, 23, 5, 1.25f, 1, 2, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(11, 11600, 25, 5, 0, 95, 26, 5, 1.3f, 1, 2, 0));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(12, 14950, 27, 6, 1, 102, 29, 5, 1.35f, 1, 2, 1));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(13, 18900, 29, 7, 2, 109, 32, 5, 1.4f, 1, 3, 1));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(14, 23500, 31, 8, 3, 116, 35, 5, 1.45f, 1, 3, 1));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(15, 28800, 33, 9, 4, 123, 38, 5, 1.5f, 1, 3, 1));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(16, 34850, 35, 10, 5, 130, 41, 5, 1.55f, 1, 3, 1));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(17, 41700, 37, 12, 6, 138, 44, 5, 1.6f, 1, 4, 1));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(18, 49400, 39, 14, 7, 146, 47, 5, 1.65f, 1, 4, 1));
//		HeroData[(int)UnitID.Unity].AddLevelData(new HeroLevelData(19, 58000, 41, 16, 9, 154, 50, 5, 1.7f, 1, 4, 1));
//	}
//
//	public HeroBaseData GetHeroData(int Id)
//	{
//		if(HeroData.Count > Id)
//		{
//			for(int i = 0; i< HeroData.Count; i++)
//			{
//				if(HeroData[i].ID == Id)
//				{
//					return HeroData[i];
//				}
//			}
//			return null;
//		}
//		else
//		{
//			return null;
//		}
//	}
//}
//
//
