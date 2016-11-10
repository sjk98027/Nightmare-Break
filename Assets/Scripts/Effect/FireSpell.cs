using UnityEngine;
using System.Collections;

public class FireSpell : MonoBehaviour {

	public GameObject meteorEffect;
	private float timeInterval;
    private Vector3 pos;

    void Start()
	{
		timeInterval = 0.1f;
        StartCoroutine(meteorBurst());
	}

    IEnumerator meteorBurst()
    {
        GameObject[] meteor = new GameObject[8];
        for (int i = 0; i < meteor.Length; i++)
        {
            pos = new Vector3(transform.parent.position.x + Random.Range(-3, 3), transform.parent.position.y + Random.Range(-3, 3), transform.parent.position.z);
            meteor[i] = Instantiate(meteorEffect, pos, transform.rotation) as GameObject;
            yield return new WaitForSeconds(timeInterval);
        }
        Destroy(transform.parent.gameObject);
    }
}
