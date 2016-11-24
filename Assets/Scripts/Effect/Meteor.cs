using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {

    void Update()
    {
        transform.Translate(0, 0, 30 * Time.smoothDeltaTime, Space.Self);    
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
