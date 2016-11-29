using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MonsterId{
	None=0,
	Duck,
	Frog,
	Rabbit,
	Bear
}

public class MonsterDatabase
{
	private static MonsterDatabase instance;

	public static MonsterDatabase Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new MonsterDatabase();
			}

			return instance;
		}        
	}

	List<MonsterBaseData> MonsterData;

	public void InitializeMonsterDatabase()
	{
		MonsterData = new List<MonsterBaseData> ();
		MonsterData.Add (new MonsterBaseData ((int)MonsterId.None,"None"));
		MonsterData.Add (new MonsterBaseData ((int)MonsterId.Duck,"Duck"));
		MonsterData.Add (new MonsterBaseData ((int)MonsterId.Frog,"Frog"));
		MonsterData.Add (new MonsterBaseData ((int)MonsterId.Rabbit,"Rabbit"));
		MonsterData.Add (new MonsterBaseData ((int)MonsterId.Bear,"Bear"));

		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (1, 2, 900, 0, 1, 0, 1));// right 3 -> X;
		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (2, 3, 945, 0, 1, 0, 1));
		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (3, 5, 1039, 0, 1, 0, 1));
		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (4, 6, 1195, 1, 1, 0, 1));
		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (5, 8, 1434, 2, 1, 0, 1));
		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (6, 10, 1793, 2, 1, 0, 1));
		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (7, 13, 2331, 3, 1, 0, 1));
		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (8, 15, 3146, 3, 1, 0, 1));
		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (9, 19, 4405, 4, 1, 0, 1));
		MonsterData [(int)MonsterId.Duck].AddStatusData (new MonsterStatusData (10, 24, 6388, 5, 1, 0, 1));

		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (1, 2, 900, 0, 1, 0, 1));// right 3 -> X;
		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (2, 3, 945, 0, 1, 0, 1));
		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (3, 5, 1039, 0, 1, 0, 1));
		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (4, 6, 1195, 1, 1, 0, 1));
		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (5, 8, 1434, 2, 1, 0, 1));
		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (6, 10, 1793, 2, 1, 0, 1));
		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (7, 13, 2331, 3, 1, 0, 1));
		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (8, 15, 3146, 3, 1, 0, 1));
		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (9, 19, 4405, 4, 1, 0, 1));
		MonsterData [(int)MonsterId.Frog].AddStatusData (new MonsterStatusData (10, 24, 6388, 5, 1, 0, 1));

		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (1, 5, 900, 0, 1, 0, 1));// right 3 -> X;
		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (2, 7, 945, 0, 1, 0, 1));
		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (3, 10, 1039, 0, 1, 0, 1));
		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (4, 12, 1195, 1, 1, 0, 1));
		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (5, 16, 1434, 2, 1, 0, 1));
		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (6, 21, 1793, 4, 1, 0, 1));
		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (7, 26, 2331, 8, 1, 0, 1));
		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (8, 31, 3146, 9, 1, 0, 1));
		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (9, 38, 4405, 12, 1, 0, 1));
		MonsterData [(int)MonsterId.Rabbit].AddStatusData (new MonsterStatusData (10, 48, 6388, 15, 1, 0, 1));

		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(1,10,4500,0,1,1,115));// right 1 -> exp; 2-> stone ; 3-> moveSpeed;
		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(2,15,4725,0,1,2,309));	
		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(3,20,5198,0,1,3,425));	
		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(4,25,5977,2,1,4,596));	
		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(5,33,7173,4,1,5,710));
		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(6,42,8966,8,1,6,825));
		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(7,52,11655,12,1,8,1065));
		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(8,63,15735,18,1,10,1456));
		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(9,77,22029,24,1,12,1857));
		MonsterData[(int)MonsterId.Bear].AddStatusData(new MonsterStatusData(10,97,31942,30,1,16,2483));
	}

	public MonsterBaseData GetMonsterBaseData(int Id)
	{
		foreach(MonsterBaseData baseData in MonsterData)
		{
			if (baseData.Id == Id) 
			{
				return baseData;
			}
		}

		return null;
	}

	public MonsterStatusData GetMonsterStatusData(int Id, int level)
	{
		foreach(MonsterBaseData baseData in MonsterData)
		{
			if (baseData.Id == Id) 
			{
				return baseData.GetStatusData (level);
			}
		}

		return null;
	}		
}

public class MonsterBaseData{
	int id;
	string name;
	List<MonsterStatusData> monsterStatusData;

	public int Id{get { return id;}}
	public string Name{get{ return name;}}
	public List<MonsterStatusData> MonsterStatusData { get { return monsterStatusData; } }

	public MonsterBaseData()
	{
		id = 0;
		name = "";
		monsterStatusData = new List<MonsterStatusData>();
	}

	public MonsterBaseData(int _id, string _name){
		id = _id;
		name = _name;
		monsterStatusData = new List<MonsterStatusData> ();
	}

	public MonsterStatusData GetStatusData(int level)
	{
		foreach(MonsterStatusData statusData in monsterStatusData)
		{
			if (statusData.MonsterLevel == level) 
			{
				return statusData;
			}
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
}

public class MonsterStatusData{
	int monsterLevel;
	int monsterDamage;
	int monsterHp;
	int monsterDefense;
	float monsterMoveSpeed;
	int dreamStoneCount;
	int monsterExp;

	public int MonsterLevel{get{ return monsterLevel;}}
	public int MonsterDamage{ get { return monsterDamage; } }
	public int MonsterHp{get{ return monsterHp;}}
	public int MonsterDefense{get{ return monsterDefense;}}
	public int DreamStoneCount{get{ return dreamStoneCount;}}
	public int MonsterExp{get{ return monsterExp;}}

	public float MonsterMoveSpeed{get{ return monsterMoveSpeed;}}

	public MonsterStatusData(){
		
		monsterLevel = 1;
		monsterDamage = 0;
		monsterHp = 0;
		monsterDefense = 0;
		monsterMoveSpeed = 1;
		dreamStoneCount = 1;
		monsterExp = 115;
	}

	public MonsterStatusData(
		int _monsterLevel,
		int _monsterDamage,
		int _monsterHp,
		int _monsterDefense,
		float _monsterMoveSpeed,
		int _dreamStoneCount,
		int _monsterExp
	)
	{
		monsterLevel = _monsterLevel;
		monsterDamage = _monsterDamage;
		monsterHp = _monsterHp;
		monsterDefense = _monsterDefense;
		monsterMoveSpeed = _monsterMoveSpeed;
		dreamStoneCount = _dreamStoneCount;
		monsterExp = _monsterExp;
	}
}


