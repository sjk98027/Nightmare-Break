using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        particleSystem.Simulate(1.5f, true);
    }

}
