using UnityEngine;
using System.Collections;

public class MiddleBoss1 : MonoBehaviour {
	public BoomMonsterTest[] boomObject;
	public GameObject middleBoss;

	//Vector3[] pointVector = new Vector3[9];
	[SerializeField]Vector3[] pointVector1 = new Vector3[7];
	[SerializeField]Vector3[] pointVector2 = new Vector3[7];
	[SerializeField]Vector3[] pointVector3 = new Vector3[7];

	[SerializeField]
	Vector3[] boomObjectPosition;
	Vector3 garbagepointVector;

	Vector3[] garbagepointVectorArray;
	[SerializeField]
	Vector3 addedVector;

	public enum MonSterMovePosition
	{
		Up = 1,
		Down,
		Middle,
		case4,
		Comback
	};

	//




	//animation Set; move;
	public void PointVectorArraySet()
	{
		pointVector1[0] = new Vector3(1, 0, 1);
		pointVector1[1] = new Vector3(0,0,1);
		pointVector1[2] = new Vector3(-1, 0, 1);
		pointVector1[3] = new Vector3(-1, 0, 1);
		pointVector1[4] = new Vector3(0,0,1);
		pointVector1[5] = new Vector3(1, 0, 1);
		pointVector1[6] = new Vector3(0,0,1);

		pointVector3[0] = new Vector3(0,0,1);
		pointVector3[1] = new Vector3(1, 0, 1);
		pointVector3[2] = new Vector3(0,0,1);
		pointVector3[3] = new Vector3(-1, 0, 1);
		pointVector3[4] = new Vector3(-1, 0, 1);
		pointVector3[5] = new Vector3(1, 0, 1);
		pointVector3[6] = new Vector3(0,0,1);

		pointVector2[0] = new Vector3(-1, 0, 1);
		pointVector2[1] = new Vector3(0,0,1);
		pointVector2[2] = new Vector3(1, 0, 1);
		pointVector2[3] = new Vector3(1, 0, 1);
		pointVector2[4] = new Vector3(0,0,1);
		pointVector2[5] = new Vector3(-1, 0, 1);
		pointVector2[6] = new Vector3(0,0,1);
	
	}




	float moveSpeed = 0.5f;
	float limitDistanceMonsterToCenter = 6.0f;
	Vector3 centerpoint = Vector3.zero;
	float[] currentDistanceMonsterToCenter = new float[10];
	[SerializeField]
	float[] currentDistance;

	// Use this for initialization
	void Start()
	{
		


		boomObjectPosition = new Vector3[boomObject.Length];
		for (int i = 0; i < boomObjectPosition.Length; i++)
		{
			boomObjectPosition[i] = boomObject[i].transform.position;
		}
		PointVectorArraySet ();
		boomObject [0].pointVectorArrayGetting (pointVector1);
		boomObject [1].pointVectorArrayGetting (pointVector1);
		boomObject [2].pointVectorArrayGetting (pointVector1);
		boomObject [3].pointVectorArrayGetting (pointVector2);
		boomObject [4].pointVectorArrayGetting (pointVector2);
		boomObject [5].pointVectorArrayGetting (pointVector2);
		boomObject [6].pointVectorArrayGetting (pointVector3);
		boomObject [7].pointVectorArrayGetting (pointVector3);
		boomObject [8].pointVectorArrayGetting (pointVector3);
		//		InpointVector();	
		//        StartCoroutine(pointVectorchange());
		middleBoss = this.gameObject;
	}




	//	float attackcycle =0;

	// Update is called once per frame
	void Update()
	{
//		        for (int i = 0; i < boomObject.Length; i++)
//		        {
//		            //			boomObject [i].transform.Translate (Vector3.Lerp (boomObject [i].transform.position, pointVector [i] * 0.5f, 1f) * Time.deltaTime);
//		//            boomObject[i].transform.Translate(garbagepointVector * Time.deltaTime);
//		        }

		//		if (currentDistance > playerdistance) {
		//			attackcycle += Time.deltaTime;
		//moveable = false;
		//			if(attackcycle > 5){
		//				pattern(attack){
		middleBoss.transform.Translate(new Vector3(0,0,1f) *moveSpeed* Time.deltaTime);
		centerpoint += new Vector3(0,0,1)* moveSpeed * Time.deltaTime;
		for (int i = 0; i < boomObject.Length; i++) {
			boomObjectPosition[i] += new Vector3 (0, 0, 1) * moveSpeed * Time.deltaTime;
		}
		//				}
		//			}
		//		}
		//if(moveable){exampleObject.transform.Translate(Vector3.right*Time.deltaTime);}

		//Debug.Log (boomObject [0].pointVector[2]);

		for (int i = 0; i < boomObject.Length; i++) {
			boomObject [i].UpdateConduct ();
		}

	}



}
