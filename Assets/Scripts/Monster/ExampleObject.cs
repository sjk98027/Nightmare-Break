using UnityEngine;
using System.Collections;

public class ExampleObject : MonoBehaviour {
    public GameObject[] boomObject;
    public GameObject exampleObject;
    [SerializeField]
    Vector3[] pointVector = new Vector3[10];
    [SerializeField]
    Vector3[] boomObjectPosition;
    Vector3 garbagepointVector;
    [SerializeField]
    Vector3 addedVector;

    public enum MoveState
    {
        Case1 = 1,
        Case2,
        Case3,
        case4,
        Comback
    };

    //




    //animation Set; move;
    public void Pattern(MoveState state)
    {
        switch (state)
        {
            case MoveState.Case1:
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
            case MoveState.Case2:
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
            case MoveState.Case3:
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
            case MoveState.case4:
                {

                    break;
                }

            case MoveState.Comback:
                {
                    break;
                }

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
        boomObject = GameObject.FindGameObjectsWithTag("Finish");
        boomObjectPosition = new Vector3[boomObject.Length];
        for (int i = 0; i < boomObjectPosition.Length; i++)
        {
            boomObjectPosition[i] = boomObject[i].transform.position;
        }
        InpointVector();
        StartCoroutine(pointVectorchange());
        exampleObject = this.gameObject;
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
        for (int i = 0; i < boomObject.Length; i++)
        {
            //			boomObject [i].transform.Translate (Vector3.Lerp (boomObject [i].transform.position, pointVector [i] * 0.5f, 1f) * Time.deltaTime);
            boomObject[i].transform.Translate(garbagepointVector * Time.deltaTime);
        }

        //		if (currentDistance > playerdistance) {
        //			attackcycle += Time.deltaTime;
        //moveable = false;
        //			if(attackcycle > 5){
        //				pattern(attack){
        exampleObject.transform.Translate(Vector3.zero * Time.deltaTime);

        //				}
        //			}
        //		}
        //if(moveable){exampleObject.transform.Translate(Vector3.right*Time.deltaTime);}

    }



}
