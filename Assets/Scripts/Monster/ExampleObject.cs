using UnityEngine;
using System.Collections;

public class ExampleObject : MonoBehaviour {
	public GameObject[] boomObject;
	public GameObject exampleObject;
	Vector3[] pointVector = new Vector3[10];
	Vector3[] objectPosition = new Vector3[10];
	Vector3 garbagepointVector;
	[SerializeField]Vector3 addedVector;

	[SerializeField]Vector3 centerpoint = Vector3.zero;

	float moveSpeed = 0.1f;
	float limitDistanceMonsterToCenter=6.0f;
	float[] currentDistanceMonsterToCenter = new float[10];
	//[SerializeField]float[] currentDistance;

	// Use this for initialization
	void Start () {
		boomObject = GameObject.FindGameObjectsWithTag ("Finish");
		InpointVector ();
		StartCoroutine (pointVectorchange());
		exampleObject = this.gameObject;
	}

	void InpointVector(){
		pointVector[0] = new Vector3(-1,0,0);
		pointVector[1] = new Vector3(-1,0,-1);
		pointVector[2] = new Vector3(0,0,-1);
		pointVector[3] = new Vector3(1,0,-1);
		pointVector[4] = new Vector3(1,0,0);
		pointVector[5] = new Vector3(1,0,1);
		pointVector[6] = new Vector3(0,0,1);
		pointVector[7] = new Vector3(-1,0,1);
		pointVector[8] = new Vector3(0,0,0);
	}

	void InpointVector2(){
		for (int i = 0; i < boomObject.Length; i++) {
			pointVector [i] = boomObject [i].transform.position - exampleObject.transform.position;
			currentDistanceMonsterToCenter[i] = Vector3.Distance (boomObject [i].transform.position, exampleObject.transform.position);
		}
		for (int j = 0; j < boomObject.Length; j++) {
			
		}
	}

	public IEnumerator pointVectorchange(){
		while (true) {
			for (int i = 0; i < boomObject.Length; i++) {
				if(i>0 && i<boomObject.Length-1){
					garbagepointVector = pointVector [i];
					pointVector [i] = pointVector [i+1];
					pointVector [i + 1] = garbagepointVector;
				}

				if (i == boomObject.Length -1) {
					garbagepointVector = pointVector [i];
					pointVector [i] = pointVector[0];
					pointVector [0] = garbagepointVector;
				}
//				boomObject [i].transform.Translate(garbagepointVector*moveSpeed);
			}
			addedVector = Vector3.zero;
			yield return new WaitForSeconds(0.5f);

		}

	}


	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey (KeyCode.UpArrow)) {
			exampleObject.transform.Translate (new Vector3(0,0,1)*moveSpeed);
			addedVector += new Vector3 (0, 0, 1) * moveSpeed;
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			exampleObject.transform.Translate (new Vector3(0,0,-1)*moveSpeed);
			addedVector += new Vector3 (0, 0, -1) * moveSpeed;
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			exampleObject.transform.Translate (Vector3.left*moveSpeed);
			addedVector += Vector3.left * moveSpeed;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			exampleObject.transform.Translate (Vector3.right*moveSpeed);
			addedVector += Vector3.right * moveSpeed;
		}
		for (int i = 0; i < boomObject.Length; i++) {
			//currentDistance [i] = Vector3.Distance (boomObject [i].transform.position, exampleObject.transform.position);
			boomObject [i].transform.Translate ((pointVector[i]+addedVector) * moveSpeed);
		}
		exampleObject.transform.Translate ((new Vector3 (0, 0, 1) * 0.01f));


	}


}
