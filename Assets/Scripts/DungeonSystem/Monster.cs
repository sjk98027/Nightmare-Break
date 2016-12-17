﻿using UnityEngine;
using System.Collections;

public enum StatePosition
{
	Idle=0,
	Run,
	Attack,
	BossOneHandAttack,
	BossJumpAttack,
	TakeDamage,
	Death,
	BossRoar,
}

public enum DefenseMoveDirectionArray
{
	Up = 1,
	Down,
	Middle,
	case4,
	Comback
}

public class Monster : MonoBehaviour {
	public void targetPlayerPosition(TargetPlayerPosition targetposition){
		switch (targetposition) {
		case TargetPlayerPosition.Zero:
			movePoint = (targetPlayer.transform.position);
			break;
		case TargetPlayerPosition.Up:
			movePoint = new Vector3 (targetPlayer.transform.position.x - transform.position.x + (searchRange*0.5f), 0, checkDirection.z);
			break;
		case TargetPlayerPosition.Down:
			movePoint = new Vector3 (targetPlayer.transform.position.x - transform.position.x - (searchRange*0.5f), 0, checkDirection.z);
			break;
		case TargetPlayerPosition.Left:
			movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z - transform.position.z - (searchRange*0.5f));
			break;
		case TargetPlayerPosition.Right:
			movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z - transform.position.z + (searchRange*0.5f));
			break;
		}
	}
	public enum TargetPlayerPosition{
		Zero = 0,
		Up,
		Down,
		Left,
		Right
	}
    public const bool right = true;
    public const bool left = false;

    protected Animator animator;
	protected AnimatorStateInfo aniState;
	protected BoxCollider HittedBox;
	protected MonsterWeapon[] attackCollider;
	protected GameObject shockWaveInstantiate;

    public GameObject[] player;
	protected GameObject[] wall;
	protected GameObject nearWall;
	[SerializeField]protected GameObject targetPlayer;
	[SerializeField]protected Vector3 movePoint;

    protected int monsterRunAttackAround;

	protected int randomStandby;
	//public int RandomStandby;
	//mode,gateArraynumber,monsterArraynumber
	protected bool moveAble;

	[SerializeField]protected int monsterIndex;
	protected MonsterId monsterId;
    protected string _name;
    protected int level;
    protected int currentHP;
	protected int maxHP;
    protected int attack;
    protected int defense;
	[SerializeField]protected int moveSpeed;


    //monster getting variable;
	[SerializeField]protected float RunRange;
	[SerializeField]protected float attackRange;
	[SerializeField]protected float attackCycle;
	[SerializeField]protected float currentDisTance;
	[SerializeField]protected float searchRange;
	StatePosition statePosition;
    bool direction;
	public  bool isAlive;
	protected bool isAttack;
	protected bool isHited;

	//boss skill 
	protected int bossPatternCount;
	protected bool bossNormalAttackCycle;
	protected bool bossSkill;
	protected int bossRandomPattern;



    protected float[] playerToMonsterDamage;
	private float[] aggroRank; //playertoMonsterdamage/currentdistancePlayer;
    private float changeTargetTime=0;

	[SerializeField]private float[]currentDisTanceArray;
	[SerializeField]private float[] currentDisTanceWall;
	protected Vector3 checkDirection; // monster chaseplayer and move variable;
	[SerializeField]protected Vector3[] pointVector;
    
    public int MonsterIndex
    {
        get { return monsterIndex; }
        set { monsterIndex = value; }
    }

    public bool MoveAble
    {
        get { return moveAble; }
        set { moveAble = value; }
    }
    public bool IsAttack{
		get{ return isAttack;}
		set{ isAttack = value;}
	}
	public bool IsAlive{
		get{ return isAlive;}
		set{ isAlive = value;}
	}
	public bool IsHited{
		get{ return isHited;}
		set{ isHited = value;}
	}
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    
    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

	public Vector3 MovePoint{
		get{ return movePoint;}
		set{ movePoint = value;}
	}

    public StatePosition _StatePosition { get { return statePosition; } }
    public bool Direction { get { return direction; } }
    public int Attack { get { return attack; } }
    public MonsterId MonsterId { get { return monsterId; } set { monsterId = value; } }

    public void MonsterSet(MonsterBaseData monster)
	{
		wall = GameObject.FindGameObjectsWithTag("Wall");
		currentDisTanceWall = new float[wall.Length];

        animator = this.gameObject.GetComponent<Animator>();
        HittedBox = this.gameObject.GetComponent<BoxCollider>();

        isAlive = true;
        isHited = false;
        moveAble = true;

        _name = monster.Name;
        level = monster.MonsterLevelData[0].Level;
        attack = monster.MonsterLevelData[0].Attack;
        defense = monster.MonsterLevelData[0].Defense;
        currentHP = monster.MonsterLevelData[0].HealthPoint;
        maxHP = monster.MonsterLevelData[0].HealthPoint;
        moveSpeed = monster.MonsterLevelData[0].MoveSpeed;


		if (monster.Id == (int)MonsterId.Rabbit || monster.Id == (int)MonsterId.Frog) {
			searchRange = 12;
			attackRange = 4;
			attackCollider = this.transform.GetComponentsInChildren<MonsterWeapon> ();
			attackCollider[0].MonsterWeaponSet ();
		}


		if(monster.Id == (int)MonsterId.Duck){
			shockWaveInstantiate = Resources.Load<GameObject>("Effect/Monster_ShockWave");
			searchRange = 8;

		}

		if (monster.Id == (int)MonsterId.Bear || monster.Id == (int)MonsterId.BlackBear) {
			attackCollider = this.transform.GetComponentsInChildren<MonsterWeapon> ();
			attackCollider[0].MonsterWeaponSet ();
			attackCollider[1].MonsterWeaponSet ();
			searchRange = 12;
			attackRange = 3;
			RunRange = 30;
		}

        currentDisTanceArray = new float[player.Length];
		aggroRank = new float[player.Length];
		playerToMonsterDamage = new float[player.Length];

        StartCoroutine(LookatChange());
    }

	IEnumerator BossNormalAttackCycleSet()
	{ 
		while (IsAlive) 
		{
			bossNormalAttackCycle = true;

			yield return new WaitForSeconds (3.0f);
		}
	}

	public void MonsterAIStart(bool _normalMode){//moveAI->StartAI , 
		if (!_normalMode) {
			if (monsterIndex > 18) {
				DefenseMoveSet (DefenseMoveDirectionArray.Up);
			} else if (monsterIndex > 9) {
				DefenseMoveSet (DefenseMoveDirectionArray.Middle);
			} else if (monsterIndex >= 0) {
				DefenseMoveSet (DefenseMoveDirectionArray.Down);
			}
		}
		if(monsterId == MonsterId.Rabbit|| monsterId == MonsterId.Frog ){
			StartCoroutine(MonsterMoveAI (_normalMode));
			StartCoroutine(MonsterActAI (_normalMode));
		}

		if (monsterId == MonsterId.Duck) {
			StartCoroutine(MonsterMoveAI (_normalMode));
			StartCoroutine (MonsterActAIADC(_normalMode));
		}

		if(monsterId == MonsterId.Bear || monsterId == MonsterId.BlackBear){
			bossSkill = false;
			StartCoroutine (BossActAI());
			StartCoroutine (BossSkillAI ());
			StartCoroutine(BossNormalAttackCycleSet ());

		}
	}

	public void MonsterUpdate()
	{
		aniState = this.animator.GetCurrentAnimatorStateInfo(0);
		if (aniState.IsName("Run"))
		{
			if (moveAble)
			{
				if (monsterId != MonsterId.Bear && monsterId != MonsterId.BlackBear) {
					this.transform.Translate (movePoint.normalized * moveSpeed * Time.deltaTime);
				}
				else
				{
					this.transform.Translate((targetPlayer.transform.position-this.transform.position )* moveSpeed * Time.deltaTime, 0);
				}
			}
		}
		ChasePlayer();
		NearWallCheck ();
	}

	IEnumerator MonsterMoveAI(bool _normalMode)
	{
		while (IsAlive)
		{
			//if (monsterId == UnitId.Frog || monsterId == UnitId.Rabbit || monsterId == UnitId.Duck) {
			if (_normalMode) {
				if (targetPlayer != null) {
					if (Mathf.Abs (targetPlayer.transform.position.z - transform.position.z) > 8 || Mathf.Abs (targetPlayer.transform.position.x - this.gameObject.transform.position.x) > 0.6f) {
						if (currentDisTance < searchRange) {
							randomStandby = Random.Range (0, 3);
							if (randomStandby == 0) {
								//for 문 -> Fuck go;
								if (checkDirection.z > 0) {
									for (int i = 0; i < 4; i++) {
										movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z - 3f);
										yield return new WaitForSeconds (2f);
									}
								}
								if (checkDirection.z < 0) {
									for (int i = 0; i < 4; i++) {
										movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z + 3f);
										yield return new WaitForSeconds (2f);
									}									
								}
							}

							if (randomStandby == 1) {

								int a = Random.Range (0, 4);
								if (a == 0) {
									targetPlayerPosition (TargetPlayerPosition.Up);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Left);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Down);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Right);
									yield return new WaitForSeconds (2f);
								}
								if (a == 1) {
									targetPlayerPosition (TargetPlayerPosition.Up);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Right);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Down);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Left);
									yield return new WaitForSeconds (2f);
								}
								if (a == 2) {
									targetPlayerPosition (TargetPlayerPosition.Down);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Left);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Up);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Right);
									yield return new WaitForSeconds (2f);
								}
								if (a == 3) {
									targetPlayerPosition (TargetPlayerPosition.Down);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Right);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Up);
									yield return new WaitForSeconds (2f);
									targetPlayerPosition (TargetPlayerPosition.Left);
									yield return new WaitForSeconds (2f);
								}
							}
							if (randomStandby == 2) {
								int a = Random.Range (0, 4);
								if (a == 0) {
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
								}
								if (a == 1) {
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
								}
								if (a == 2) {
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (2f);
								}
								if (a == 3) {
									movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (1f);
									movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
									yield return new WaitForSeconds (1f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (1f);
									movePoint = new Vector3 (0, 0, -checkDirection.z);
									yield return new WaitForSeconds (1f);
								}
							}
							yield return new WaitForSeconds (0.2f);
						} else if (Mathf.Abs (targetPlayer.transform.position.z - transform.position.z) < 8 && Mathf.Abs (targetPlayer.transform.position.x - this.gameObject.transform.position.x) <= 0.6f) {
							aniState = this.animator.GetCurrentAnimatorStateInfo (0);
							if (!aniState.IsName ("Attack")) {
								if (moveAble) {
									if (checkDirection.z > 0) {
										if (checkDirection.x > 0) {
											movePoint = new Vector3 (-checkDirection.x, 0, 0);
										}
										if (checkDirection.x < 0) {
											movePoint = new Vector3 (checkDirection.x, 0, 0);
										}
									}
									if (checkDirection.z <= 0) {
										if (checkDirection.x > 0) {
											movePoint = new Vector3 (-checkDirection.x, 0, 0);
										}
										if (checkDirection.x < 0) {
											movePoint = new Vector3 (checkDirection.x, 0, 0);
										}
									}
								}
							}
						}
						yield return new WaitForSeconds (2f);
					} else
						yield return new WaitForSeconds (3f);
				} 
				else if (!IsAlive)
				{
					StopAllCoroutines();
					//yield return false;
				}
				yield return new WaitForSeconds (0.2f);
			}
			while (!_normalMode) {
				if (isAlive) {
					if(!isHited){
						for (int i = 0; i < pointVector.Length; i++) {
							if (i > 0 && i < pointVector.Length - 1) {
								movePoint = pointVector [i];
								pointVector [i] = pointVector [i + 1];
								pointVector [i + 1] = movePoint;
							}

							if (i == pointVector.Length - 1) {
								movePoint = pointVector [i];
								pointVector [i] = pointVector [0];
								pointVector [0] = movePoint;
							}
						}
					}
					if (isHited) {
						if (targetPlayer.transform.position.z > transform.position.z) {
							movePoint = (targetPlayer.transform.position-transform.position);
						} else
							isHited = false;
					}
					yield return new WaitForSeconds (2f);
				} 
				else {
					
					break;
				}
			}
		}

	}

	IEnumerator MonsterActAI(bool _normal){
		while (_normal) {
			if (isAlive) {
				if (targetPlayer != null) {	
					currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
					checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
					if (currentDisTance > searchRange) {
						statePosition = StatePosition.Idle; 
						Pattern (statePosition);

					}
					//if this object get Attackmotion pattern(stateposition.boom -> attack), and this monsterlife is 20%, boomPattern start;
					else if (currentDisTance <= searchRange) {
						{
							if (currentDisTance > searchRange * 0.2f) {
								moveAble = true;
								isAttack = false;
								statePosition = StatePosition.Run;
								Pattern (statePosition);
							}
							if (currentDisTance <= searchRange * 0.3f) {
								if (!isAttack) {
									isAttack = true;
									moveAble = false;
								}
								statePosition = StatePosition.Attack;
								Pattern (statePosition);
								yield return new WaitForSeconds (0.5f);
							}
						}
					}
				}
				yield return new WaitForSeconds (0.2f);
			} else
				break;
		}
		while (!_normal) {
			if (isAlive) {
				if (targetPlayer != null) {
					currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
					checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
				}
				attackCycle += 0.2f;
				//int 
				if (attackCycle > 3) {
					statePosition = StatePosition.Idle;
					Pattern (statePosition);
					yield return new WaitForSeconds (1.3f);
					if (checkDirection.z > 0) {
						LookAtPattern (right);
					}
					if (checkDirection.z <= 0) {
						LookAtPattern (left);
					}
					moveAble = false;
					isAttack = true;
					statePosition = StatePosition.Attack;
					Pattern (statePosition);
					yield return new WaitForSeconds (2f);
					attackCycle = 0;

				}
				if (attackCycle <= 3) {
					LookAtPattern (left);
					moveAble = true;
					isAttack = false;
					statePosition = StatePosition.Run;
					Pattern (statePosition);
				}

				yield return new WaitForSeconds (0.2f);
			}else break;
		}
	}

	IEnumerator MonsterActAIADC(bool _normalMode){
		while (_normalMode) {
			if (isAlive) {
				if (targetPlayer != null) {	
					currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
					checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;

					if (Mathf.Abs (targetPlayer.transform.position.z - transform.position.z) < 8 && Mathf.Abs (targetPlayer.transform.position.x - this.gameObject.transform.position.x) <= 0.6f) {
						if (!isAttack) {
							isAttack = true;
							moveAble = false;
						}
						statePosition = StatePosition.Attack;
						if (checkDirection.z > 0) {
							LookAtPattern (right);
						}
						if (checkDirection.z < 0) {
							LookAtPattern (left);
						}
						Pattern (statePosition);
						yield return new WaitForSeconds (0.5f);

					} 

					else if (currentDisTance > searchRange) {

						moveAble = false;
						isAttack = false;
						statePosition = StatePosition.Idle;
						Pattern (statePosition);

					} else {
						moveAble = true;
						isAttack = false;
						statePosition = StatePosition.Run;
						Pattern (statePosition);
					}

				}
				yield return new WaitForSeconds (0.2f);
			} else
				yield return new WaitForSeconds (0.2f);
		}
		while (!_normalMode) {
			if (targetPlayer != null) {
				currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
				checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
			}
			attackCycle += 0.2f;

			if (attackCycle > 3) {
				statePosition = StatePosition.Idle;
				Pattern (statePosition);
				yield return new WaitForSeconds (1.3f);
				if (checkDirection.z > 0) {
					LookAtPattern (right);
				}
				if (checkDirection.z <= 0) {
					LookAtPattern (left);
				}
				moveAble = false;
				isAttack = true;
				statePosition = StatePosition.Attack;
				Pattern (statePosition);
				yield return new WaitForSeconds (2f);
				attackCycle = 0;

			}
			if (attackCycle <= 3) {
				LookAtPattern (right);
				moveAble = true;
				isAttack = false;
				statePosition = StatePosition.Run;
				Pattern (statePosition);
			}

			yield return new WaitForSeconds (0.2f);
		}
	}

	IEnumerator BossActAI(){
		while (true) {
			if (isAlive) {
				if (!bossSkill) {
					aniState = this.animator.GetCurrentAnimatorStateInfo (0);
					if(targetPlayer != null){
						currentDisTance = Vector3.Distance (targetPlayer.transform.position, transform.position);
						checkDirection = targetPlayer.transform.position - transform.position;
						if(!aniState.IsName ("Attack")) {
							currentDisTance = Vector3.Distance (targetPlayer.transform.position, transform.position);
							checkDirection = targetPlayer.transform.position - transform.position;
							if (checkDirection.z > 0) {
								LookAtPattern (right);
							}
							if (checkDirection.z < 0) {
								LookAtPattern (left);								
							}
						}

						if (currentDisTance < attackRange && bossNormalAttackCycle) 
						{
							statePosition = StatePosition.Attack;
							bossNormalAttackCycle = false;
							Pattern (statePosition);
							yield return new WaitForSeconds (1);

						}
						if (currentDisTance > searchRange) {
							statePosition = StatePosition.Idle;
							Pattern (statePosition);
							if (aniState.IsName ("Idle")) {
								//changedirection
							}
						} 
						
						else if (currentDisTance <= searchRange && currentDisTance > attackRange && moveAble) {
							statePosition = StatePosition.Run;
							Pattern (statePosition);
							movePoint = checkDirection;


						}

						if (aniState.IsName ("Attack")) {
							for (int i = 0; i < attackCollider.Length; i++) {
								attackCollider [i].enabled = true;
							}
							
						} else if (aniState.IsName ("Attack")) {
							for (int i = 0; i < attackCollider.Length; i++) {
								attackCollider [i].enabled = false;
							}
						}
				}

				}
				yield return new WaitForSeconds(0.2f);
			}

			if (!isAlive) {
				break;
			}
		}
	}


	IEnumerator BossSkillAI()
	{
		aniState = this.animator.GetCurrentAnimatorStateInfo (0);

		while (IsAlive)
		{
			yield return new WaitForSeconds (10f);
			bossRandomPattern = Random.Range (0, 3);		// pattern = 1;
			bossSkill = true;
			animator.SetBool ("BossSkill", true);

				checkDirection = targetPlayer.transform.position - transform.position;
				if (checkDirection.z > 0) {
						movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z - 3f);
				}
				if (checkDirection.z < 0) {
						movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z + 3f);
				}

			if (bossRandomPattern == 0)
			{
				statePosition = StatePosition.BossJumpAttack;
				Pattern(statePosition);

			}
			else if (bossRandomPattern == 1)
			{
				statePosition = StatePosition.BossRoar;
				Pattern(statePosition);
			}
			else if (bossRandomPattern == 2)
			{
				statePosition = StatePosition.BossOneHandAttack;
				Pattern(statePosition);
			}


			yield return new WaitForSeconds (1.0f);

			bossSkill = false;
			animator.SetBool ("BossSkill", false);
		}
	}
	IEnumerator SetTargetPlayer()
	{
		while (IsAlive)
		{
			int chaseIndex = Random.Range (0, player.Length);

			targetPlayer = player[chaseIndex];

			yield return new WaitForSeconds (15.0f);
		}
	}

    public void Pattern(StatePosition state)
    {
        switch (state)
        {
            case StatePosition.Idle:
                {
                    animator.SetInteger("State", 0);
                    break;
                }

            case StatePosition.Attack:
                {
                    AttackProcess(isAttack);
                    break;
                }

            case StatePosition.Run:
                {
                    if (!aniState.IsName("Attack"))
                    {
                        AnimatorReset();
                    }
                    animator.SetInteger("State", 1);
                    break;
                }

            case StatePosition.TakeDamage:
                {
                    animator.SetTrigger("TakeDamage");
                    break;
                }

            case StatePosition.Death:
                {
                    animator.SetTrigger("Death");

                    //MonsterArrayEraser(this.gameObject);
                    break;
                }

            case StatePosition.BossOneHandAttack:
                {
                    animator.SetInteger("State", 3);
                    break;
                }

            case StatePosition.BossJumpAttack:
                {
                    animator.SetInteger("State", 4);
                    break;
                }
        }
    }

	public void AttackProcess(bool isAttack){
		if (monsterId == MonsterId.Bear || monsterId == MonsterId.BlackBear) {
			animator.SetInteger ("State", 2);
		}
		else
			if (isAttack) {

			if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Run")){
				animator.SetInteger ("State", 0);
			}
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Idle")) {
				animator.SetInteger ("State", 2);
			}
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Attack")) {
				moveAble = false;
			}
		}
	}

    public void LookAtPattern(bool dir)
    {
        switch (dir)
        {
            case true:
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                }
            case false:
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
                }
        }
    }

	IEnumerator LookatChange(){
		while (true) {
			if (!isAttack) {
				aniState = this.animator.GetCurrentAnimatorStateInfo (0);
				if (targetPlayer != null) {
					if (aniState.IsName ("Run")||aniState.IsName("Idle")){
						if (movePoint.z > 0) {
							LookAtPattern (right);
						} else if (movePoint.z < 0) {
							LookAtPattern (left);
						}
					}
				}
			}
			yield return new WaitForSeconds (0.2f);
		}
	}

    
	public void NearWallCheck(){
		/*if(wall[0] !=null){
			for (int i = 0; i < wall.Length; i++) {
				currentDisTanceWall [i] = Vector3.Distance (wall [i].transform.position, transform.position);
			}
			for (int j = 0; j < wall.Length; j++) {
				if (currentDisTanceWall [j] <= Mathf.Min (currentDisTanceWall)) {
					nearWall = wall [j];
				}
			}
		}*/
	}


	//targetplayer change;
	public void ChasePlayer(){
		if(player[0] != null){
			if (!isHited) {
				changeTargetTime += Time.deltaTime;
				if (changeTargetTime >= 3) {
					changeTargetTime = 0;
					NormalchasePlayer ();
				}
			}
			if (isHited) {
				changeTargetTime += Time.deltaTime;
				if (changeTargetTime >= 2) {
					changeTargetTime = 0;
					HitedchasePlayer ();
				}
			}
		}
	}
    public void NormalchasePlayer()
	{
		for (int i = 0; i < player.Length; i++) {
			currentDisTanceArray [i] = Vector3.Distance(player [i].transform.position, transform.position);		
		}
		for (int j = 0; j < player.Length; j++) {
			if (currentDisTanceArray [j] <= Mathf.Min (currentDisTanceArray)) {
                targetPlayer = player [j];
			}
		}

	}
	public void HitedchasePlayer()
	{
		for (int i = 0; i < player.Length; i++) {
			currentDisTanceArray [i] = Vector3.Distance(player [i].transform.position, transform.position);		
			if (currentDisTanceArray [i] < 2f) {
				currentDisTanceArray [i] = 2f;
			}
			aggroRank [i] = playerToMonsterDamage [i] *(1/(currentDisTanceArray [i]*0.5f));
		}

		for (int j = 0; j < player.Length; j++) {
			if (aggroRank [j] <= Mathf.Max (aggroRank)) {
                targetPlayer = player [j];
			}
		}
	}

	public void DefenseMoveSet(DefenseMoveDirectionArray defenseMoveDirectionArray)
	{
		pointVector = new Vector3[7];
		switch (defenseMoveDirectionArray) {
		case DefenseMoveDirectionArray.Up:
			{
				pointVector [0] = new Vector3 (1, 0, 1);
				pointVector [1] = new Vector3 (0, 0, 1);
				pointVector [2] = new Vector3 (-1, 0, 1);
				pointVector [3] = new Vector3 (-1, 0, 1);
				pointVector [4] = new Vector3 (0, 0, 1);
				pointVector [5] = new Vector3 (1, 0, 1);
				pointVector [6] = new Vector3 (0, 0, 1);
				break;
			}
		case DefenseMoveDirectionArray.Down:
			{
				pointVector [0] = new Vector3 (0, 0, 1);
				pointVector [1] = new Vector3 (1, 0, 1);
				pointVector [2] = new Vector3 (1, 0, 1);
				pointVector [3] = new Vector3 (-1, 0, 1);
				pointVector [4] = new Vector3 (-1, 0, 1);
				pointVector [5] = new Vector3 (1, 0, 1);
				pointVector [6] = new Vector3 (0, 0, 1);
				break;
			}
		case DefenseMoveDirectionArray.Middle:
			{
				pointVector [0] = new Vector3 (-1, 0, 1);
				pointVector [1] = new Vector3 (0, 0, 1);
				pointVector [2] = new Vector3 (1, 0, 1);
				pointVector [3] = new Vector3 (1, 0, 1);
				pointVector [4] = new Vector3 (0, 0, 1);
				pointVector [5] = new Vector3 (-1, 0, 1);
				pointVector [6] = new Vector3 (0, 0, 1);
				break;
			}

		}
	}

	//monster animation event;
	public void AttackStart(){
		moveAble = false;
		isAttack = true;
		StopCoroutine (LookatChange ());
	}
	public void AnimatorReset(){
		//animator.SetInteger ("State", 0);
	}
	public void AttackBlitz()
	{
		if (monsterId != MonsterId.Duck) {
			for (int i = 0; i < attackCollider.Length; i++) {
				attackCollider [i].AttackColliderOn ();
			}
		} 
		if (monsterId == MonsterId.Duck) {
			if (direction == left) {
				GameObject objShockWave = (GameObject)Instantiate (shockWaveInstantiate, new Vector3 (this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z - 0.53f), this.transform.rotation);
				objShockWave.GetComponent<ShockWave>().SetDamage(attack,this);
			}
			if (direction == right) {
				GameObject objShockWave = (GameObject)Instantiate (shockWaveInstantiate, new Vector3 (this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z + 0.53f), this.transform.rotation);	
				objShockWave.GetComponent<ShockWave>().SetDamage(attack,this);
			}
		}
	}
	public void AttackEnd(){

		StartCoroutine (LookatChange ());
		moveAble=true;
		isAttack = false;
		animator.SetInteger ("State", 0);

		if (monsterId != MonsterId.Duck) {
			for (int i = 0; i < attackCollider.Length; i++) {
				attackCollider [i].AttackColliderOff ();
			}
		} 

	}
	//bossAnimation event
	public void RoarHit()
	{
		Debug.Log (direction);
		if (direction == right)
		{
			Instantiate (Resources.Load<GameObject> ("Effect/WarningEffect"), new Vector3 (-3.55f, 0.15f, this.transform.position.z + 10f), Quaternion.Euler (-90, 0, 0));
		}
		else if (direction == left)
		{
			Instantiate (Resources.Load<GameObject> ("Effect/WarningEffect"), new Vector3 (3.55f, 0.15f, this.transform.position.z - 10f), Quaternion.Euler (-90, 0, 0));
		} 

		//GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<RedRenderImage> ().enabled = true;
	}

	IEnumerator Shooting()
	{
		int shootNum = 0;

		//		while (shootNum < shootNumber)
		//		{
		//			yield return new WaitForSeconds (0.2f);
		//
		//			int xPos = Random.Range (-2, 2);
		//			int zPos = Random.Range (-2, 2);
		//
		//			Instantiate (Resources.Load<GameObject> ("Effect/BossDarkSphere"), handPos.transform.position + (Vector3.right * xPos) + (Vector3.forward * zPos), Quaternion.Euler (0, 0, 0));
		//
		//			shootNum++;
		//		}
		yield return new WaitForSeconds (0.2f);
	}

	//monsterdie event;
	public void MonsterArrayEraser(GameObject thisGameObject)
	{
		if (monsterId == MonsterId.Frog) {
		
		}

		this.gameObject.SetActive (false);
		//				section.RemoveMonsterArray ();
	}

	public void HitDamage(int _Damage, GameObject _weapon)
	{
		
//		
		currentHP -= _Damage;

		if (currentHP > 0)
		{
			for (int i = 0; i < player.Length; i++)
			{
				if (player [i] == _weapon)
				{
					playerToMonsterDamage [i] += _Damage;
				}
			}
			statePosition = StatePosition.TakeDamage;
			Pattern (statePosition);
		}
		else
		{
			currentHP = 0;
			IsAlive = false;
			HittedBox.enabled = false;
			statePosition = StatePosition.Death;
			Pattern (statePosition);
		}
	}

	public void GetTargetPlayer(GameObject _TargerPlayer){
		
	}

	public void SendTargetPlayer(){
		
	}

}
