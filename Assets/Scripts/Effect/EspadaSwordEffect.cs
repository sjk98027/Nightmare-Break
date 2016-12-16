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
	public AudioSource swordSound;
	public AudioClip swordSummonSound;
	public AudioClip swordFinishSound;

	public Rigidbody giganticSwordRigd;
	public float swordSpeed;

    void Start()
    {
		count = true;
       // myParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
		character = GameObject.FindWithTag ("Player");
		swordSound = this.gameObject.GetComponent<AudioSource> ();
		swordSummonSound =  Resources.Load<AudioClip> ("Sound/WarriorEffectSound/GiganticSwordSummonEffectSound");
		swordFinishSound = Resources.Load<AudioClip> ("Sound/WarriorEffectSound/GiganticSwordFinishEffectSound");
		charManager = character.GetComponent<CharacterManager> ();
		giganticSwordRigd = GetComponent<Rigidbody> ();
		swordSpeed = 40;
		giganticSwordRigd.velocity = (transform.forward* swordSpeed);

        charStatus = charManager.CharStatus;
		giganticSword = this.gameObject;
		swordSound.volume = 0.1f;
		rend = GetComponent<Renderer> ();
		StartCoroutine(EffectMove());
		swordSound.PlayOneShot (swordSummonSound);
		Destroy (this.gameObject, 3.0f);
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
			Instantiate(Resources.Load<GameObject>("Effect/SwordExplosion"), transform.position, Quaternion.identity);
			count = false;
			swordSound.PlayOneShot (swordFinishSound);
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