using UnityEngine;
using System.Collections;

public class MonsterSpawnPoint : MonoBehaviour {

	public DungeonManager dungeonManager;
	public GameObject[] BoomMonsterSpawn;
	public GameObject[] ShockwaveMonsterSpawn;
	public GameObject[] WarriorMonsterSpawn;

	public int boomMonsterCount;
	public int shockWaveMonsterCount;
	public int warriorMonsterCount;
	public Vector3[] spawnVector;



	public int sumMonsterCount;




	public void RespawnPointSend(){
		spawnVector = new Vector3[spawnVector.Length];
		for (int i = 0; i < spawnVector.Length; i++) {
			//spawnVector[i] = spawnVector [i].transform.position;
		}
	}

	// Use this for initialization
	public void SpawnMonsterGetting () {
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
		boomMonsterCount = BoomMonsterSpawn.Length;
		shockWaveMonsterCount = ShockwaveMonsterSpawn.Length;
		warriorMonsterCount = WarriorMonsterSpawn.Length;
	}

	public void SpawnVectorGetting(){
		
	}

}
