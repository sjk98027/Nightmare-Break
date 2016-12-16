using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour 
{
	BoxCollider box;
	public GameObject[] enermyArray;
	// Use this for initialization
	void Start () 
	{
		box = this.GetComponent<BoxCollider> ();
		enermyArray = GameObject.FindGameObjectsWithTag ("Enermy");
		this.box.enabled = false;
		//Scene;
	}
	void Update()
	{
		if (enermyArray == null) 
		{
			this.box.enabled = true;
		}
	}
	
	// Update is called once per frame

	public void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player"))
		{
			Debug.Log ("in Change");
			//if()
			SceneManager.LoadScene("ProtoTypeCoPy1");

		}
	}

}
