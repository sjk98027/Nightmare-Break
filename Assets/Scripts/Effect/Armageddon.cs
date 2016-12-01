using UnityEngine;
using System.Collections;

public class Armageddon : MonoBehaviour {

    private ParticleSystem ps;
    
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(ArmageddonPause());
    }

    IEnumerator ArmageddonPause()
    {
        yield return new WaitForSeconds(ps.duration - 0.2f);
        ps.Pause();
    }

}
