using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	public bool normalMode;
	public void StartWallSet(){
		
	}


	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy")) {
			coll.gameObject.GetComponent<Monster> ().MovePoint = -coll.gameObject.GetComponent<Monster> ().MovePoint;
//			if (!normalMode) {
//				coll.gameObject.GetComponent<Monster> ().MovePoint = new Vector3(-coll.gameObject.GetComponent<Monster> ().MovePoint.x,coll.gameObject.GetComponent<Monster> ().MovePoint.y,coll.gameObject.GetComponent<Monster> ().MovePoint.z);
//			}

		}
	}
}
