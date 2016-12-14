using UnityEngine;
using System.Collections;

public class EspadaSwordEffect : MonoBehaviour
{
    ParticleSystem myParticle;
	GameObject giganticSword;
	public Renderer rend;
	public bool count;
	public CharacterStatus charStatus;
	public CharacterManager charManager;
	float giganticSwordAliveTime;
	public GameObject character;
	public int swordDamage;

	public Rigidbody giganticSwordRigd;
	public float swordSpeed;

    void Start()
    {
		count = true;
       // myParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
		character = GameObject.FindWithTag ("Player");

		charManager = character.GetComponent<CharacterManager> ();
		giganticSwordRigd = GetComponent<Rigidbody> ();
		swordSpeed = 40;
		giganticSwordRigd.velocity = (transform.forward* swordSpeed);

        charStatus = charManager.CharStatus;
		giganticSword = this.gameObject;

		rend = GetComponent<Renderer> ();
		StartCoroutine(EffectMove());
	}
	void Update()
	{
       // charStatus.SkillLevel [5] = 3;
//		swordDamage = charStatus.SkillLevel [5] * charStatus.Attack;
		if (!count)
		{
			giganticSwordAliveTime += Time.deltaTime;

			float SwordAlpha = 1 - giganticSwordAliveTime ;

			rend.material.color = new Color (0, 0, 0, (SwordAlpha) );

			if(SwordAlpha <0.5)
			{
				SwordAlpha = 0;
				Destroy (this.gameObject, 0.5f);
			}
		}	
	}

    IEnumerator EffectMove()
    {
        while (myParticle.isPlaying)
        {
            myParticle.gameObject.transform.Translate(0, 0, 4 * Time.deltaTime, Space.Self);

            yield return null;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
		
        if (coll.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            Instantiate(Resources.Load<GameObject>("Effect/Explosion"), transform.position, Quaternion.identity);
			count = false;

			Debug.Log ("in Field");
        }
    }

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in monster");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				Debug.Log (character);
				monsterDamage.HitDamage (swordDamage,character );
				swordDamage = 0;

			}
		}
	}

}