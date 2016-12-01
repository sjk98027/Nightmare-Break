using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BossMonsterKYW : Monster {
	public enum BigBearBossPatternName
	{
		BigBearBossIdle = 0,
		BigBearBossRun,
		BigBearBossAttack,
		BigBearBossOneHandAttack,
		BigBearJumpAttack,
		BigBearBossRoar,
		BigBearBossDeath}

	;
	public float searchRange = 10;
	public float moveSpeed;

	public float currentDisTance;


	public UIManager uiManager;
	public MonsterWeapon[] MonsterWeapon;

	public BigBearBossPatternName BigBearBossState;
	AnimatorStateInfo stateInfo;

	[SerializeField] Image skillInsertImage;
	public float imageSpeed = 1.0f;
	public float imageLerpTime;

	public insertImageState imageState = insertImageState.Stop;

	public enum insertImageState
	{
		Stop = 0,
		Left,
		Right
	}

	public void BossMonsterSet(int _maxlife, int _basedamage){
		RunRange = 10;
		attackRange = 8;
		MonsterSet (_maxlife, _basedamage);
		PlayerSearch ();
		uiManager = GameObject.FindWithTag ("UIManager").GetComponent<UIManager> ();
		isAttack = false;
		skillInsertImage = GameObject.Find ("InGameUICanvas").transform.Find ("BossDeadlyPatternImage").Find("BossDeadlyPattern").GetComponent<Image>();
		//skillInsertImage = transform.Find("InGameUICanvas").gameObject;
	}

	public void BossMonsterUpdate(){

		if (targetPlayer != null) {
			currentDisTance = Vector3.Distance (targetPlayer.transform.position, transform.position);		
		}


		if (IsAlive) {
			stateInfo = this.animator.GetCurrentAnimatorStateInfo (0);
			ChasePlayer ();

//			searchRange = Vector3.Distance (player [0].transform.position, transform.position);

			//			attackCyc+= Time.deltaTime;
		}
		if (!IsAlive) {
			Destroy (this.gameObject, 5);
		}

		if (stateInfo.IsName ("BigBearBossRun")) {

			transform.Translate ((player [0].transform.position - transform.position) * moveSpeed * Time.deltaTime, 0);//반대로 걸어 가서 수정
			//transform.position = Vector3.Lerp (transform.position, player [0].transform.position, Time.deltaTime * moveSpeed);
		}



	}




public void earthQuakeEffect ()
{//애니메이션 이벤트를 사용하여 지진 효과를 추가 한다.
	//		GameObject.FindGameObjectWithTag ("Floor").GetComponent<EarthQuake> ().Running = true;

}

public void changeDirection ()
{//캐릭터 이동시 보스가 보는 방향을 정한다.
	Vector3 vecLookPos = player [0].transform.position;
	vecLookPos.y = transform.position.y;
	vecLookPos.x = transform.position.x;

	transform.LookAt (vecLookPos);

}


public override void HitDamage (int _Damage, GameObject attacker)
{

	stateInfo = this.animator.GetCurrentAnimatorStateInfo (0);


	if (IsAlive)
	{
		maxLife -= _Damage;

//					uiManager.bossHp.fillAmount = maxLife / currentLife;
		if (maxLife > 0)
		{
			//hitanimation
		}
		else if (maxLife <= 0)
		{
			if (!stateInfo.IsName ("BigBearBossDeath"))
			{
				BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossDeath);
				IsAlive = false;
				return;
			}
		}
	}
}

public void BigBearBossPattern (int bossState)
{
	IsAttack = false;
	switch (bossState)
	{

	case 0:
		BigBearBossState = BigBearBossPatternName.BigBearBossIdle;
		animator.SetInteger ("state", 0);
		break;
	case 1:
		BigBearBossState = BigBearBossPatternName.BigBearBossRun;
		animator.SetInteger ("state", 1);
		break;
	case 2:
		BigBearBossState = BigBearBossPatternName.BigBearBossAttack;
		animator.SetInteger ("state", 2);
		IsAttack = true;
		break;

	case 3:
		BigBearBossState = BigBearBossPatternName.BigBearBossOneHandAttack;
		animator.SetInteger ("state", 3);
		IsAttack = true;
		break;

	case 4:
		BigBearBossState = BigBearBossPatternName.BigBearJumpAttack;
		animator.SetInteger ("state", 4);
		break;

	case 5:
		BigBearBossState = BigBearBossPatternName.BigBearBossRoar;
		animator.SetInteger ("state", 5);
		break;

	case 6:
		BigBearBossState = BigBearBossPatternName.BigBearBossDeath;
		animator.SetTrigger ("BigBearBossDeath");
		break;
	}
}

public void roarStart ()
{//애니메이션 이벤트를 사용하여 포효시 붉은 이펙트를 켠다.
	//GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<RedRenderImage> ().enabled = true;
	imageState = insertImageState.Left; //이미지 상태 값 저장 왼쪽
	StartCoroutine (LMoveImage ()); //코루틴 실행

}

public void roarEnd ()
{//애니메이션 이벤트를 사용하여 포효시 붉은 이펙트를 끈다.
	//GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<RedRenderImage> ().enabled = false;
	////이미지를 오른쪽이동하는 코루틴을 실행 시키는 함수 애니메이션 이벤트로 실행
	//		imageState = insertImageState.Right;//이미지 상태 값 저장 오른쪽
}

IEnumerator LMoveImage ()
{  //이미지를 왼쪽이동시키는 함수
	while (imageState == insertImageState.Left)
	{		

		yield return new WaitForSeconds (0.01f);	


		float x = skillInsertImage.GetComponent<RectTransform> ().localPosition.x;//현재 x값의 위치를 받아온다.
		if (x > 103)
		{
			skillInsertImage.GetComponent<RectTransform> ().Translate (15 * -imageSpeed, 0, 0);//10씩 왼쪽으로 이동

		}
		else
		{
			//	skillInsertImage.enabled = true;
			imageLerpTime += 0.01f;

			if (imageLerpTime >3)
			{
				float ImageAlpha = (4 - imageLerpTime);
				skillInsertImage.color = new Color (255,255,255, ImageAlpha);


				if (ImageAlpha < 0.3)
				{
					skillInsertImage.enabled = false;
					imageState = insertImageState.Stop;//x값이 0보다 작을 경우 멈춤
					ImageBackPos ();
				}

			}
		}
	}
}

public void ResetBossImage()
{
	skillInsertImage.GetComponent<RectTransform> ().localPosition = new Vector3 (613, -152 , 0);
	skillInsertImage.color = new Color (255,255,255, 255);
	imageState = insertImageState.Stop;
}

public void ImageBackPos()
{
	StopCoroutine (LMoveImage ());

	imageState = insertImageState.Right;

	ResetBossImage ();


	if (imageState == insertImageState.Stop)
	{
		skillInsertImage.enabled = true;
	}
}


	public void BossMonsterPatternUpdateConduct(){
		StartCoroutine (BossMonsterPatternChange ());
	}
	public IEnumerator BossMonsterPatternChange(){
		while (IsAlive) {
			if (attackCycle >= 1) {
				attackCycle = 0;
			}

			if (targetPlayer != null) {

				if (currentDisTance < attackRange) {
					
						//BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossAttack);
						BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossRoar);
						//애니메이션 이벤트로 효과를 넣었음
						
						BigBearBossPattern ((int)BigBearBossPatternName.BigBearJumpAttack);
						//애니메이션 이벤트로 효과를 넣었음
						

				} else if (currentDisTance > RunRange) {
					BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossIdle);
					changeDirection ();

				} else if (currentDisTance <= RunRange && currentDisTance > attackRange) {

					BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossRun);
					changeDirection ();
//				if (stateInfo.IsName ("BigBearBossRun")) {
//
//					//transform.LookAt(player[0].transform.position);
//					transform.Translate ((player [0].transform.position - transform.position) * moveSpeed * Time.deltaTime, 0);//반대로 걸어 가서 수정
//					//transform.position = Vector3.Lerp (transform.position, player [0].transform.position, Time.deltaTime * moveSpeed);
//				}
				}

				if (IsAttack) {
					for (int i = 0; i < MonsterWeapon.Length; i++) {
						MonsterWeapon [i].AttackColliderSizeChange(new Vector3 (3.6f, 1f, 1.1f));
					}
				} else if (!IsAttack) {
					for (int i = 0; i < MonsterWeapon.Length; i++) {
						MonsterWeapon [i].AttackColliderSizeChange(new Vector3(0,0,0));
					}
				}
			} else
				BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossIdle);
			yield return new WaitForSeconds (0.2f);
	}

}

	// server code;
//	public void SendMonsterState(StatePosition _state, bool _isAttack, bool _moveAble, Vector3 _movePoint, GameObject _Player){
//		//send to server;
//
//	}
//
//	public void RecibeMonsterState(StatePosition _state, bool _isAttack, bool _moveAble, Vector3 _movePoint, GameObject _Player){
//		if (_state == StatePosition.Run) {
//			movePoint = _movePoint;
//		}
//		monsterState = _state;
//		isAttack = _isAttack;
//		moveAble = _moveAble;
//		Pattern (_state);
//		if (_Player != null) {
//			targetPlayer = _Player;
//		}
//	}
//
//	public void GuestMonsterUpdate(){
//		aniState = this.animator.GetCurrentAnimatorStateInfo (0);
//		if (aniState.IsName ("Run")) 
//		{
//			if (moveAble) 
//			{
//				this.transform.Translate (movePoint * moveSpeed * Time.deltaTime, 0);
//			}
//		}
//		ChasePlayer ();
//	}
//
//
//
//	public IEnumerator GuestMonsterPatternChange(){
//		while (IsAlive) {
//			Pattern (monsterState);
//			yield return new WaitForSeconds (0.2f);
//		}
//	}
//






}



//public class TestMonster : Monster
//{
//	
//	//animation Set; move;
//
//	void Update ()
//	{
//
//		if (IsAlive) {
//			stateInfo = this.animator.GetCurrentAnimatorStateInfo (0);
//			searchRange = Vector3.Distance (player [0].transform.position, transform.position);
//			AttackTime += Time.deltaTime;
//
//			if (AttackTime >= 1) {
//				AttackTime = 0;
//			}
//			if (searchRange < attackRange) {
//				if (!secondAttack) {
//					//BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossAttack);
//					BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossRoar);
//					//애니메이션 이벤트로 효과를 넣었음
//					secondAttack = true;
//				} else if (!secondAttack) {
//					BigBearBossPattern ((int)BigBearBossPatternName.BigBearJumpAttack);
//					//애니메이션 이벤트로 효과를 넣었음
//					secondAttack = true;
//				}
//			} else if (searchRange > RunRange) {
//				BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossIdle);
//				changeDirection ();
//
//			} else if (searchRange <= RunRange && searchRange > attackRange) {
//
//				BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossRun);
//				changeDirection ();
//				if (stateInfo.IsName ("BigBearBossRun")) {
//
//					//transform.LookAt(player[0].transform.position);
//					transform.Translate ((player [0].transform.position - transform.position) * moveSpeed * Time.deltaTime, 0);//반대로 걸어 가서 수정
//					//transform.position = Vector3.Lerp (transform.position, player [0].transform.position, Time.deltaTime * moveSpeed);
//				}
//			}
//
//			if (IsAttack) {
//				for (int i = 0; i < MonsterWeapon.Length; i++) {
//					//				MonsterWeapon [i].size = new Vector3 (3.6f, 1f, 1.1f);
//					MonsterWeapon [i].size = new Vector3 (0, 0, 0);
//				}
//			} else if (!IsAttack) {
//				for (int i = 0; i < MonsterWeapon.Length; i++) {
//					MonsterWeapon [i].size = new Vector3 (0, 0, 0);
//				}
//			}
//
//
//		}
//		if (!IsAlive) {
//			Destroy (this.gameObject, 5);
//		}
//
//	}
//}