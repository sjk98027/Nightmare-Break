using UnityEngine;
using System.Collections;

public class MiddleBoss : MonoBehaviour {
	public BoomMonster[] boomObject;
	public GameObject middleBoss;

	Vector3[] pointVector = new Vector3[7];

	[SerializeField]
	Vector3[] boomObjectPosition;
	[SerializeField]Vector3 addedVector = new Vector3(0,0,1f);



	float moveSpeed = 0.5f;

	Vector3 centerpoint = Vector3.zero;
	[SerializeField]float[] currentDistanceMonsterToCenter;

	public void DefenceMiddleBossSet(){
		boomObjectPosition = new Vector3[boomObject.Length];
		currentDistanceMonsterToCenter = new float[boomObject.Length];
	}

	public void UpdateConductDefenceMode(){
		middleBoss.transform.Translate(addedVector *moveSpeed* Time.deltaTime);
		centerpoint += new Vector3(0,0,1)* moveSpeed * Time.deltaTime;
		for (int i = 0; i < boomObject.Length; i++) {
			boomObjectPosition[i] += addedVector * moveSpeed * Time.deltaTime;

//			currentDistanceMonsterToCenter[i] = Vector3.Distance (boomObject [i].transform.position, middleBoss.transform.position);
		}
	}







	//	float attackcycle =0;

	// Update is called once per frame
//	void Update()
//	{
//		//		        for (int i = 0; i < boomObject.Length; i++)
//		//		        {
//		//		            //			boomObject [i].transform.Translate (Vector3.Lerp (boomObject [i].transform.position, pointVector [i] * 0.5f, 1f) * Time.deltaTime);
//		//		//            boomObject[i].transform.Translate(garbagepointVector * Time.deltaTime);
//		//		        }
//
//		//		if (currentDistance > playerdistance) {
//		//			attackcycle += Time.deltaTime;
//		//moveable = false;
//		//			if(attackcycle > 5){
//		//				pattern(attack){
//		middleBoss.transform.Translate(new Vector3(0,0,1f) *moveSpeed* Time.deltaTime);
//		centerpoint += new Vector3(0,0,1)* moveSpeed * Time.deltaTime;
//		for (int i = 0; i < boomObject.Length; i++) {
//			boomObjectPosition[i] += new Vector3 (0, 0, 1) * moveSpeed * Time.deltaTime;
//			boomObject [i].UpdateConduct ();
//			currentDistanceMonsterToCenter[i] = Vector3.Distance (boomObject [i].transform.position, middleBoss.transform.position);
//		}
//		//				}
//		//			}
//		//		}
//		//if(moveable){exampleObject.transform.Translate(Vector3.right*Time.deltaTime);}
//
//		//Debug.Log (boomObject [0].pointVector[2]);
//
//
//
//	}



}
