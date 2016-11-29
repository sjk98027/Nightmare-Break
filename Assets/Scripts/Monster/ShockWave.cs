using UnityEngine;
using System.Collections;

public class ShockWave : MonoBehaviour {
    ShockWaveMonster AttackMonster;
    int damage;

    public void GetDamage(int _damage, ShockWaveMonster _AttackMonster) {
        AttackMonster = _AttackMonster;
        damage = _damage;
    }

    void OnParticleCollision(GameObject gameObject)
    {
        if(gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Hit");
            gameObject.GetComponent<CharacterManager>().HitDamage(damage);
        }
    }
}
