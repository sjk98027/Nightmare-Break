using System.Collections;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public enum CharacterState
    {
        Idle = 0,
        Run,
        Attack,
        Jump,

        Skill1,
        Skill2,
        Skill3,
        Skill4,
        HitDamage,
        Death
    }

    public Animator animator;
    public Renderer rend;
    public AnimatorStateInfo runState;
    public Rigidbody rigdbody;
    public BoxCollider charWeapon;

    public InputManager inputmanager;
    public CharacterStatus charStatus;

    public GameObject[] enermy;

    public float jumpPower;

    public bool charDir;
    public bool JumpMove;
    public bool normalAttackState = false;
    public bool skillAttackState = false;
    public bool charAlive = true;
	public int charBasicDamage;

    private int potionCount = 3;

    public float skillTime;

    [SerializeField]
    CharacterState state;

    public CharacterStatus CharStatus { get { return this.charStatus; } }

    public CharacterState State { get { return state; } }

    public bool NormalAttackState { get { return this.normalAttackState; } }

    public bool SkillAttackState { get { return this.skillAttackState; } }

    private UIManager uiManager;

    public UIManager UIManager { get { return uiManager; } set { uiManager = value; } }

    void Start()
    {
        //uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        animator = GetComponent<Animator>();
        state = CharacterState.Idle;
        rigdbody = GetComponent<Rigidbody>();
		enermy = GameObject.FindGameObjectsWithTag ("Enermy");
        charDir = true;
        JumpMove = false;
		charBasicDamage = charStatus.Attack;
    }

    void Update()
    {
        if (charAlive)
        {
            // check skill state
            if (skillAttackState)
            {
                // skill index
                // skill process ProcessSkill1()

                switch (state)
                {
                    case CharacterState.Skill1:
                        ProcessSkill1();
                        break;

                    case CharacterState.Skill2:
                        ProcessSkill2();
                        break;

                    case CharacterState.Skill3:
                        ProcessSkill3();
                        break;

                    case CharacterState.Skill4:
                        ProcessSkill4();
                        break;
                }

            }



            if (normalAttackState || skillAttackState)
            {
            //    charWeapon.size = new Vector3(0.11f, 0.11f, 1.28f);
            }
            else
            {

            //    charWeapon.size = new Vector3(0, 0, 0);
            }
        }

    }

    public void SetCharacterStatus()
    {
		charStatus = GameObject.FindGameObjectWithTag("CharStatus").GetComponent<CharacterStatus>();
		charStatus.SetCharacterStatus ();
	}

    public void AnimationEnd()
    {
        CharState((int)CharacterState.Idle);
        JumpMove = false;
        normalAttackState = false;
        skillAttackState = false;

    }

    //char state Method
	public void Move (float ver, float hor)
	{
		if (state == CharacterState.Idle || state == CharacterState.Run)
		{

			runState = this.animator.GetCurrentAnimatorStateInfo (0);

			if (!animator.GetBool ("Attack"))
			{
				if (ver != 0 || hor != 0)
				{
					animator.SetFloat ("Ver", ver);
					animator.SetFloat ("Hor", hor);

					if (ver < 0)
					{
						transform.rotation = Quaternion.Euler (new Vector3 (0, 180.0f, 0));
						charDir = false;
					}
					else if (ver > 0)
					{
						transform.rotation = Quaternion.Euler (new Vector3 (0, 0.0f, 0));
						charDir = true;
					}
					CharState ((int)CharacterState.Run);

					if (runState.IsName ("Run"))
					{
						if (hor == -1.0f || hor == 1.0f)
						{
							transform.Translate ((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * (charStatus.MoveSpeed - 3.0f), Space.World);

						}
						else
						{
							transform.Translate ((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * (charStatus.MoveSpeed), Space.World);
						}
					}
				}
				else if (ver == 0 && hor == 0)
				{
					animator.SetBool ("Run", false);
					CharState ((int)CharacterState.Idle);
				}
			}
		}
		else if (state == CharacterState.Jump && JumpMove)
		{
			transform.Translate ((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * charStatus.MoveSpeed, Space.World);
		}

	}


    public void CheckGrounded()
    {
        if (state == CharacterState.Jump)
        {
            if (transform.position.y <= 0.1f)
            {
                CharState((int)CharacterState.Idle);
            }
        }
    }

    public void Jump()
    {
        runState = this.animator.GetCurrentAnimatorStateInfo(0);

        if (state != CharacterState.Jump && state != CharacterState.Attack && state != CharacterState.Skill1 && state != CharacterState.Skill2 && state != CharacterState.Skill1 && state != CharacterState.Skill4)
        {
            CharState((int)CharacterState.Jump);
        }


    }

    public void JumpForce()
    {
        rigdbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        JumpMove = true;
    }

    public virtual void NormalAttack()
    {
        //if ( state != characterState.Skill2 && state != CharacterState.Attack  && state != characterState.Skill1 && state != characterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
        if (state != CharacterState.Skill2 && state != CharacterState.Skill1 && state != CharacterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
        {
            normalAttackState = true;
            CharState((int)CharacterState.Attack);

            if (JumpMove)
            {
                rigdbody.mass = 100;
            }
            else
            {
                rigdbody.mass = 1;
            }
        }
    }

    public virtual void Skill1()
    {
			if (charStatus.MagicPoint > SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 1).ManaCost && charStatus.ActiveSkillUse[0])
			    {
				charStatus.DecreaseMagicPoint(SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 1).ManaCost);
              //  uiManager.BattleUIManager.mpBarCalculation(charStatus.MaxMagicPoint, charStatus.MagicPoint);
                StartCoroutine(charStatus.SkillCoolTimer(0, SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 1).SkillCoolTime));
         //       StartCoroutine (uiManager.BattleUIManager.SetSkillCoolTimeUI (0, SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 1).SkillCoolTime));
				if (state == CharacterState.Run || state == CharacterState.Idle || state == CharacterState.Skill1)
				{
					state = CharacterState.Skill1;
					CharState ((int)CharacterState.Skill1);
				}
		}
    }


    public void skill2()
    {
			if (charStatus.MagicPoint > SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 2).ManaCost && charStatus.ActiveSkillUse[1])
			{
                charStatus.DecreaseMagicPoint(SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 1).ManaCost);
         //       uiManager.BattleUIManager.mpBarCalculation(charStatus.MaxMagicPoint, charStatus.MagicPoint);
                StartCoroutine(charStatus.SkillCoolTimer(1, SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 2).SkillCoolTime));
          //      StartCoroutine (uiManager.BattleUIManager.SetSkillCoolTimeUI (1, SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 1).SkillCoolTime));
				if (state != CharacterState.Jump && state != CharacterState.Skill2 && state != CharacterState.Skill1 && state != CharacterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
				{
					CharState ((int)CharacterState.Skill2);
				}
			}
    }

    public void skill3()
	{
			if (charStatus.MagicPoint > SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 3).ManaCost && charStatus.ActiveSkillUse[2])
			{
                charStatus.DecreaseMagicPoint(SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 3).ManaCost);
          //      uiManager.BattleUIManager.mpBarCalculation(charStatus.MaxMagicPoint, charStatus.MagicPoint);
                StartCoroutine(charStatus.SkillCoolTimer(2, SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 3).SkillCoolTime));
             //   StartCoroutine(uiManager.BattleUIManager.SetSkillCoolTimeUI(2, SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 3).SkillCoolTime));
                if (state != CharacterState.Jump && state != CharacterState.Skill3 && state != CharacterState.Skill2 && state != CharacterState.Skill1 && state != CharacterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
				{
					CharState ((int)CharacterState.Skill3);
				}
			}
    }

    public void Skill4()
    {
			if (charStatus.MagicPoint > SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 4).ManaCost && charStatus.ActiveSkillUse[3])
			{
            charStatus.DecreaseMagicPoint(SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 4).ManaCost);
      //      uiManager.BattleUIManager.mpBarCalculation(charStatus.MaxMagicPoint, charStatus.MagicPoint);
            StartCoroutine(charStatus.SkillCoolTimer(3, SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 4).SkillCoolTime));
       //     StartCoroutine(uiManager.BattleUIManager.SetSkillCoolTimeUI(3, SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 4).SkillCoolTime));
            if (state != CharacterState.Jump && state != CharacterState.Skill1 && state != CharacterState.Skill2 && state != CharacterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
				{
					//giganticSwordCastSword.SetActive(true);
					CharState ((int)CharacterState.Skill4);
				}

		}
    }

    public virtual void ProcessSkill1()
    {

    }

    public virtual void ProcessSkill2()
    {

    }

    public virtual void ProcessSkill3()
    {

    }

    public virtual void ProcessSkill4()
    {

    }

    //using Potion
    public void UsingPotion()
    {   //Potion Effect create
        GameObject potionEffect = Instantiate(Resources.Load<GameObject>("Effect/Potion"), transform.position, Quaternion.identity) as GameObject;
        potionEffect.transform.parent = gameObject.transform;
        potionEffect.transform.position += Vector3.up;
        StartCoroutine(Potion());
    }

    IEnumerator Potion()
    {
        for (int i = 0; i < potionCount; i++)
        {
            charStatus.DecreaseHealthPoint((int)(charStatus.HealthPoint * -0.3));
            yield return new WaitForSeconds(1f);
        }
    }

    //Animation Method
    void SetStateDefault()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
    }

    public void CharState(int Inputstate)
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (charAlive)
        {
            SetStateDefault();
            //idle=0,run=1,attack=2
            switch (Inputstate)
            {
                case 0:
                    state = CharacterState.Idle;
                    animator.SetBool("Idle", true);
                    break;

                case 1:
                    state = CharacterState.Run;
                    animator.SetBool("Run", true);
                    break;

                case 2:
                    state = CharacterState.Attack;
                    animator.SetTrigger("Attack");
                    break;

                case 3:
                    state = CharacterState.Jump;
                    animator.SetTrigger("Jump");
                    break;

                case 4:
                    state = CharacterState.Skill1;
                    animator.SetTrigger("Skill1");
                    skillAttackState = true;
                    break;

                case 5:
                    state = CharacterState.Skill2;
                    animator.SetTrigger("Skill2");
                    skillAttackState = true;
                    //basicDamage = charstate.activeSkillSet [1].skillDamage
                    break;

                case 6:
                    state = CharacterState.Skill3;
                    animator.SetTrigger("Skill3");
                    skillAttackState = true;
                    //basicDamage = charstate.activeSkillSet [3].skillDamage;
                    break;
                case 7:
                    state = CharacterState.Skill4;
                    animator.SetTrigger("Skill4");
                    skillAttackState = true;
                    //basicDamage = charstate.activeSkillSet [2].skillDamage;
                    break;
                case 8:
                    state = CharacterState.HitDamage;
                    animator.SetTrigger("PlayerHitTrigger");
                    break;
                case 9:
                    state = CharacterState.Death;
                    animator.SetTrigger("PlayerDie");
                    break;
            }
            
            DataSender.Instance.CharacterActionSend(Inputstate);
        }
    }

    public virtual void HitDamage(int damage)
    {
        if (charAlive)
        {
            if (charStatus.HealthPoint > 0)
            {
                charStatus.DecreaseHealthPoint(damage);
                CharState((int)CharacterState.HitDamage);
            }
            if (charStatus.HealthPoint <= 0)
            {
                //Death Animation
                CharState((int)CharacterState.Death);
                charAlive = false;
            }
        }
    }

    public void SetPosition(CharacterPositionData newPositionData)
    {
        Debug.Log("캐릭터 위치 설정");

        if (newPositionData.dir)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0.0f, 0));
            charDir = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
            charDir = false;
        }
        transform.position = new Vector3(newPositionData.posX, newPositionData.posY, newPositionData.posZ);
    }

	public virtual void SetCharacterType() { }
}

