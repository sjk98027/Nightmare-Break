using UnityEngine;
using System.Collections;

public class BoomMonsterTest : MonoBehaviour {
	
		private float searchRange = 6.0f;
		private float moveSpeed = 1f;

		public float currentDisTance;


		private Vector3 movePoint;
		private Vector3 idlePoint = new Vector3(0,0,0);

		private Vector3 boomPoint = new Vector3(100,100,100);


	[SerializeField]public Vector3[] pointVector;
	[SerializeField]public Vector3 garbagepointVector;	
	public Vector3[] PointVector{
			get {return pointVector; }
			set{pointVector = value; }
		}
		
	public void pointVectorArrayGetting(Vector3[] _v3){
		pointVector = new Vector3[_v3.Length];
		pointVector = _v3;

		StartCoroutine (pointVectorchange ());
	}

		public enum StatePosition
		{
			Idle=1,
			Run,
			Attack,
			Boom,
			TakeDamage,
			Death
		};

	public IEnumerator pointVectorchange()
		{
			while (true)
			{
			for (int i = 0; i < pointVector.Length; i++)
				{
				if (i > 0 && i < pointVector.Length - 1)
					{
						garbagepointVector = pointVector[i];
						pointVector[i] = pointVector[i + 1];
						pointVector[i + 1] = garbagepointVector;
						
					}
	
				if (i == pointVector.Length - 1)
					{
						garbagepointVector = pointVector[i];
						pointVector[i] = pointVector[0];
						pointVector[0] = garbagepointVector;
					}
				}
				
				yield return new WaitForSeconds(0.5f);
			}
	
		}

	public void UpdateConduct(){
		transform.Translate (garbagepointVector*moveSpeed*0.5f*Time.deltaTime);
	}

}
