using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PoolManager : MonoBehaviour {
	[SerializeField]private List<GameObject> listObj;
	private Transform pool;
	[SerializeField]private GameObject obj;


	public void Start(){
		InitPoll (obj, 4);
		this.gameObject.transform.position = Vector3.zero;
	}
	void Update(){
		if(Input.GetKeyDown(KeyCode.R)){
			PopObject ();
		}
	}

	public void InitPoll(GameObject obj, int poolSize){
		listObj = new List<GameObject>();
		pool = transform;
		this.obj = obj;

		for (int i = 0; i< poolSize; i++) {
			GameObject go = Instantiate (obj) as GameObject;
			PushObject (go);
		}
	}

	public GameObject PopObject(){
		if (listObj.Count > 0) {
			GameObject obj = listObj [0];
			listObj.RemoveAt (0);
			ShowObject (obj);
			return obj;
		} else {
			return Instantiate (obj) as GameObject;
		}

	}

	public void ShowObject(GameObject obj){
		obj.SetActive (true);
		obj.transform.localPosition = Vector3.zero;

	}


	public void PushObject(GameObject obj){
		listObj.Add (obj);
		obj.transform.parent = pool;
		obj.transform.localPosition = Vector3.zero;
		obj.SetActive (false);

	}

	public void ClearPool(){
		listObj = null;
		foreach (Transform child in pool) {
			GameObject.Destroy (child.gameObject);
		}
	}

}
