using UnityEngine;
using System.Collections;

public class MonsterSpawnPoint : MonoBehaviour {

	public DungeonManager dungeonManager;
	public GameObject[] BoomMonsterSpawn;
	public GameObject[] ShockwaveMonsterSpawn;
	public GameObject[] WarriorMonsterSpawn;

	public Transform[] spawnPoint;
	public Vector3[] spawnVector;
	public int sumMonsterCount;




	public void RespawnPointSend(){
		spawnVector = new Vector3[spawnPoint.Length];
		for (int i = 0; i < spawnPoint.Length; i++) {
			spawnVector[i] = spawnPoint [i].transform.position;
		}
	}

	// Use this for initialization
	void Awake () {
		sumMonsterCount = WarriorMonsterSpawn.Length + ShockwaveMonsterSpawn.Length + BoomMonsterSpawn.Length;
		dungeonManager = GameObject.Find ("DungeonManager").GetComponent<DungeonManager>();
		spawnVector = new Vector3[sumMonsterCount];
		for (int i = 0; i < sumMonsterCount; i++) {
			if (i < BoomMonsterSpawn.Length) {
				spawnVector [i] = BoomMonsterSpawn [i].transform.position;
			} else if (i < BoomMonsterSpawn.Length + ShockwaveMonsterSpawn.Length) {
				spawnVector [i] = ShockwaveMonsterSpawn [i - BoomMonsterSpawn.Length].transform.position;
			} else if (i >= BoomMonsterSpawn.Length + ShockwaveMonsterSpawn.Length) {
				spawnVector [i] = WarriorMonsterSpawn [i - (BoomMonsterSpawn.Length + ShockwaveMonsterSpawn.Length)].transform.position;

			}
		}
		dungeonManager.GetMonsterTransForm (spawnVector);

	}

}
