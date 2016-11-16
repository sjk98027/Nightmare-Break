using UnityEngine;
using System.Collections;

public class Section : MonoBehaviour {
	public DungeonManager dungeonManager;
	public BoomMonster[] boomMonster;
	public WarriroMonster[] warriorMonster;
	public MiddleBoss middleBoss;

	public Vector3[] boomMonsterPosition;
	public Vector3[] pointVector;

	[SerializeField] int monsterCount;
	[SerializeField]private int gateNumber;
	public int GateNumber{
		get { return gateNumber;}
		set{ gateNumber = value;}
	}
	//all monster objects getting;


	protected bool modeForm;
	public bool ModeForm{
		get {return modeForm;}
		set {modeForm = value;}
	}

	public enum MonSterMovePosition
	{
		Up = 1,
		Down,
		Middle,
		case4,
		Comback
	};

	public void MonsterSet(){
		dungeonManager = gameObject.transform.parent.GetComponent<DungeonManager>();
		//boomMonster = gameObject.transform.GetComponentsInChildren<BoomMonster> ();
		//monsterCount = (boomMonster.Length + warriorMonster.Length+1);
		middleBoss = gameObject.transform.GetComponentInChildren<MiddleBoss>();

//		for (int i = 0; i < boomMonster.Length; i++) {
//			boomMonster [i].PlayerSearch ();
//			boomMonster [i].MonsterSet ();
//			boomMonster [i].Mode = modeForm;
//			boomMonster [i].GateArrayNumber = gateNumber;
//			boomMonster [i].MonsterArrayNumber = i;
//		}

		boomMonster = transform.GetComponentsInChildren<BoomMonster> ();
		boomMonsterPosition = new Vector3[boomMonster.Length];
		pointVector = new Vector3[7];
		if (boomMonster == null) {
			Debug.LogError ("중간보스 없음!!" + gameObject.name);
		}

		middleBoss.DefenceMiddleBossSet ();
		for (int i = 0; i < boomMonster.Length; i++)
		{
			boomMonsterPosition[i] = boomMonster[i].transform.position;
			if (i >= 6) {
				Pattern (MonSterMovePosition.Up);
				boomMonster [i].pointVectorArrayGetting (pointVector);	
			} else if (i >= 4) {
				Pattern (MonSterMovePosition.Middle);
				boomMonster [i].pointVectorArrayGetting (pointVector);	
			} else
				Pattern (MonSterMovePosition.Up);
			boomMonster [i].pointVectorArrayGetting (pointVector);
		}
	}

	public void UpdateConduct(){
		for (int i = 0; i < boomMonster.Length; i++) {
			//boomMonster [i].UpdateConduct ();
			boomMonster [i].UpdateConductDefenceMode ();
		}
		middleBoss.UpdateConductDefenceMode();



	}




	public void Pattern(MonSterMovePosition state)
	{
		switch (state)
		{
		case MonSterMovePosition.Up:
			{
				pointVector[0] = new Vector3(1, 0, 1);
				pointVector[1] = new Vector3(0,0,1);
				pointVector[2] = new Vector3(-1, 0, 1);
				pointVector[3] = new Vector3(-1, 0, 1);
				pointVector[4] = new Vector3(0,0,1);
				pointVector[5] = new Vector3(1, 0, 1);
				pointVector[6] = new Vector3(0,0,1);
				break;
			}
		case MonSterMovePosition.Down:
			{
				pointVector[0] = new Vector3(0,0,1);
				pointVector[1] = new Vector3(1, 0, 1);
				pointVector[2] = new Vector3(1,0,0);
				pointVector[3] = new Vector3(-1, 0, 1);
				pointVector[4] = new Vector3(-1, 0, 1);
				pointVector[5] = new Vector3(1, 0, 1);
				pointVector[6] = new Vector3(0,0,1);
				break;
			}
		case MonSterMovePosition.Middle:
			{
				pointVector[0] = new Vector3(-1, 0, 1);
				pointVector[1] = new Vector3(0,0,1);
				pointVector[2] = new Vector3(1, 0, 1);
				pointVector[3] = new Vector3(1, 0, 1);
				pointVector[4] = new Vector3(0,0,1);
				pointVector[5] = new Vector3(-1, 0, 1);
				pointVector[6] = new Vector3(0,0,1);
				break;
			}
		case MonSterMovePosition.case4:
			{

				break;
			}

		case MonSterMovePosition.Comback:
			{
				break;
			}

		}
	}


	public void SetFalse(){
		this.gameObject.SetActive (false);
	}



	public void RemoveMonsterArray(){
		monsterCount -= 1;
		if (monsterCount == 0) {
			dungeonManager.SceneChange ();
		}


	}


}
