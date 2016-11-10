using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class Meteor : MonoBehaviour {

    
    void Start()
    {
     
    }

    void Update()
    {
        transform.Translate(0, 0, 15 * Time.smoothDeltaTime, Space.Self);    
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            Destroy(gameObject);
            Instantiate(Resources.Load<GameObject>("Effect/MeteorExplosion"), coll.contacts[0].point, Quaternion.identity);
        }
    }
}
