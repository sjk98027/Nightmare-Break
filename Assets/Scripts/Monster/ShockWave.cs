using UnityEngine;
using System.Collections;

public class ShockWave : MonoBehaviour {
    public ShockWaveMonster AttackMonster;
    public int damage;

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
			Destroy (this.gameObject);
        }
    }
}
