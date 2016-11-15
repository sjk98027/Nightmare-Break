using UnityEngine;
using System.Collections;

public class MiddleBoss : MonoBehaviour {
	public BoomMonsterTest[] boomObject;
    public GameObject middleBoss;
    [SerializeField]
    //Vector3[] pointVector = new Vector3[9];
	Vector3[] pointVector;

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
	public void Pattern(MonSterMovePosition state)
    {
        switch (state)
        {
		case MonSterMovePosition.Up:
                {
                    pointVector[0] = new Vector3(1, 0, 1);
                    pointVector[1] = Vector3.right;
                    pointVector[2] = new Vector3(1, 0, -1);
                    pointVector[3] = new Vector3(1, 0, -1);
                    pointVector[4] = Vector3.right;
                    pointVector[5] = new Vector3(1, 0, 1);
                    pointVector[6] = Vector3.right;
                    break;
                }
		case MonSterMovePosition.Down:
                {
                    pointVector[0] = Vector3.right;
                    pointVector[1] = new Vector3(1, 0, 1);
                    pointVector[2] = Vector3.right;
                    pointVector[3] = new Vector3(1, 0, -1);
                    pointVector[4] = new Vector3(1, 0, -1);
                    pointVector[5] = new Vector3(1, 0, 1);
                    pointVector[6] = Vector3.right;
                    break;
                }
		case MonSterMovePosition.Middle:
                {
                    pointVector[0] = new Vector3(1, 0, -1);
                    pointVector[1] = Vector3.right;
                    pointVector[2] = new Vector3(1, 0, 1);
                    pointVector[3] = new Vector3(1, 0, 1);
                    pointVector[4] = Vector3.right;
                    pointVector[5] = new Vector3(1, 0, -1);
                    pointVector[6] = Vector3.right;
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

	public void SendArray(MonSterMovePosition _monsterMovePosition){
		if (_monsterMovePosition== MonSterMovePosition.Up) {
			Pattern (MonSterMovePosition.Up);
		}

		if (_monsterMovePosition == MonSterMovePosition.Down) {
			Pattern (MonSterMovePosition.Down);
		}

		if (_monsterMovePosition == MonSterMovePosition.Middle) {
			Pattern (MonSterMovePosition.Middle);
		}
		
	}


    float moveSpeed = 0.1f;
    float limitDistanceMonsterToCenter = 6.0f;
    float[] currentDistanceMonsterToCenter = new float[10];
    [SerializeField]
    float[] currentDistance;

    // Use this for initialization
    void Start()
    {
		pointVector = new Vector3[7];
		boomObject = transform.GetComponentsInChildren<BoomMonsterTest>();

        boomObjectPosition = new Vector3[boomObject.Length];
        for (int i = 0; i < boomObjectPosition.Length; i++)
        {
            boomObjectPosition[i] = boomObject[i].transform.position;
        }
		Pattern (MonSterMovePosition.Up);
		boomObject [0].pointVectorArrayGetting (pointVector);
		Debug.Log (pointVector[2] + "1");
		boomObject [1].pointVectorArrayGetting (pointVector);
		Debug.Log (pointVector[2]+ "2");
		boomObject [2].pointVectorArrayGetting (pointVector);
		Debug.Log (pointVector[2] + "3");
		Pattern (MonSterMovePosition.Middle);
		boomObject [3].pointVectorArrayGetting (pointVector);
		Debug.Log (pointVector[2] + "4");
		boomObject [4].pointVectorArrayGetting (pointVector);
		Debug.Log (pointVector[2]+ "5");
		boomObject [5].pointVectorArrayGetting (pointVector);
		Debug.Log (pointVector[2]+ "6");
		Pattern (MonSterMovePosition.Down);
		boomObject [6].pointVectorArrayGetting (pointVector);
		Debug.Log (pointVector[2]+ "7");
		boomObject [7].pointVectorArrayGetting (pointVector);
		Debug.Log (pointVector[2]+ "8");
		boomObject [8].pointVectorArrayGetting (pointVector);
		Debug.Log (pointVector[2]+ "9");









//		InpointVector();	
//        StartCoroutine(pointVectorchange());
		middleBoss = this.gameObject;
    }

    void InpointVector()
    {
        pointVector[0] = new Vector3(-1, 0, 0);
        pointVector[1] = new Vector3(-1, 0, -1);
        pointVector[2] = new Vector3(0, 0, -1);
        pointVector[3] = new Vector3(1, 0, -1);
        pointVector[4] = new Vector3(1, 0, 0);
        pointVector[5] = new Vector3(1, 0, 1);
        pointVector[6] = new Vector3(0, 0, 1);
        pointVector[7] = new Vector3(-1, 0, 1);
        pointVector[8] = new Vector3(0, 0, 0);
    }


    //	public IEnumerator Monstermove(){
    //		while (true) {
    //			for(int ab =0; ab<boomObject.Length; ab++){
    //				boomObject[ab].transform.position=Vector3.Lerp(boomObject[ab].transform.position,pointVector[ab],1);
    //			}
    //			yield return new WaitForSeconds(2f);
    //		}
    //	}

    public IEnumerator pointVectorchange()
    {
        while (true)
        {
            for (int i = 0; i < boomObject.Length; i++)
            {
                if (i > 0 && i < boomObject.Length - 1)
                {
                    garbagepointVector = pointVector[i];
                    pointVector[i] = pointVector[i + 1];
					pointVector[i + 1] = garbagepointVector;
					//garbagepointVectorArray [0] = garbagepointVector;
                }

                if (i == boomObject.Length - 1)
                {
                    garbagepointVector = pointVector[i];
                    pointVector[i] = pointVector[0];
                    pointVector[0] = garbagepointVector;
                }
            }
            addedVector = Vector3.zero;
            yield return new WaitForSeconds(0.5f);
		}

    }


    //	float attackcycle =0;

    // Update is called once per frame
    void Update()
    {
//        for (int i = 0; i < boomObject.Length; i++)
//        {
//            //			boomObject [i].transform.Translate (Vector3.Lerp (boomObject [i].transform.position, pointVector [i] * 0.5f, 1f) * Time.deltaTime);
////            boomObject[i].transform.Translate(garbagepointVector * Time.deltaTime);
//        }

        //		if (currentDistance > playerdistance) {
        //			attackcycle += Time.deltaTime;
        //moveable = false;
        //			if(attackcycle > 5){
        //				pattern(attack){
		middleBoss.transform.Translate(Vector3.zero * Time.deltaTime);

        //				}
        //			}
        //		}
        //if(moveable){exampleObject.transform.Translate(Vector3.right*Time.deltaTime);}

		//Debug.Log (boomObject [0].pointVector[2]);

    }



}
