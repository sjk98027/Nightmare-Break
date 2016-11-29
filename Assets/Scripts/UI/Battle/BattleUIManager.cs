using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleUIManager
{
	private const float checkTime = 0.1f;
	private const float mouseOverUI_xPos = 120f;
	private const float mouseOverUI_yPos = 105f;
	private Image potionUI;
	private Text mouseOverUI;
	private Image[] skillUI;
	private Image[] skillCoolTimeUI; // 0 - SKill1 // 1 - SKill2 // 2 - Skill3 // 3 - Skill4 //
    public Image hpBar;
    public Image mpBar;
    
	public Image[] SkillCoolTimeUI{ get{ return skillCoolTimeUI; }}
	public Text MouseOverUI{ get { return mouseOverUI; } set { mouseOverUI = value; } }

    #region 스킬쿨타임 제어 코루틴
    public IEnumerator SetSkillCoolTimeUI(int skillNum, float coolTime)
    {
        skillCoolTimeUI[skillNum].color += new Color(0, 0, 0, 1);

        float time = Time.smoothDeltaTime;

        while (skillCoolTimeUI[skillNum].fillAmount != 0.0f)
        {
            skillCoolTimeUI[skillNum].fillAmount -= 1 * time / coolTime;
            yield return null;

        }
        skillCoolTimeUI[skillNum].fillAmount = 1;
        skillCoolTimeUI[skillNum].color -= new Color(0, 0, 0, 1);
        time = 0;

        yield break;
    }
    #endregion

    #region 물약쿨타임제어
    public IEnumerator PotionCoolTimeUI()
    {
        potionUI.color += new Color(0, 0, 0, 1);
        float potionCoolTime = 15.0f;
        float time = Time.smoothDeltaTime;
        potionUI.gameObject.SetActive(true);
        potionUI.fillAmount = 1;
        while (potionUI.fillAmount != 0.0f)
        {
            potionUI.fillAmount -= 1 * time / potionCoolTime;
            yield return null;
        }
        time = 0;
        potionUI.color -= new Color(0, 0, 0, 0);
        yield break;
    }
    #endregion

    void hpBarCalculation(int maxHp, int currentHP)
    {
        hpBar.fillAmount = maxHp / currentHP;
    }

    void mpBarCalculation(int maxMp, int currentMP)
    {
        mpBar.fillAmount = maxMp / currentMP;
    }

    public void SetUIObject()
    {
        hpBar = GameObject.Find("HPBar").GetComponent<Image>();
        mpBar = GameObject.Find("MPBar").GetComponent<Image>();
		mouseOverUI = GameObject.Find ("MouseOverUI").GetComponent<Text> ();
		mouseOverUI.transform.parent.gameObject.SetActive (false);
		skillUI = new Image[6];
        skillCoolTimeUI = new Image[4];
		for (int i = 0; i < skillUI.Length; i++)
        {
			skillUI [i] = GameObject.Find ("Skill"+(i+1)).GetComponent<Image> ();
           
			if (i < skillCoolTimeUI.Length) {
				skillCoolTimeUI [i] = GameObject.Find ("Skill" + (i + 1) + "_CoolTime").GetComponent<Image> ();
			}
        }
    }
    
	public void SetPointEnterUI(int skillIndex, int skillLevel, int classIndex)
	{
		if (!mouseOverUI.IsActive()) {
			mouseOverUI.gameObject.transform.parent.gameObject.SetActive (true);
		}
		mouseOverUI.transform.parent.transform.localPosition = new Vector2 (skillUI [skillIndex].transform.localPosition.x + mouseOverUI_xPos, mouseOverUI_yPos);
		#region 전사 스킬 UI
		if (classIndex == 0) { 
			if (skillIndex == 0) {
				mouseOverUI.text = "스킬이름: " + SkillManager.instance.SkillData.GetSkill ("WarriorA1_L1").SkillName +"\n"+ "쿨타임: " + SkillManager.instance.SkillData.GetSkill ("WarriorA1_L1").SkillCoolTime.ToString () +"초"
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorA1_L1").SkillExplanation 
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorA1_L"+(skillLevel)).SkillDamegeExplanation; 
			} else if (skillIndex == 1) {
				mouseOverUI.text = "스킬이름: " + SkillManager.instance.SkillData.GetSkill ("WarriorA2_L1").SkillName +"\n"+ "쿨타임: " + SkillManager.instance.SkillData.GetSkill ("WarriorA2_L1").SkillCoolTime.ToString () +"초"
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorA2_L1").SkillExplanation 
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorA2_L"+(skillLevel)).SkillDamegeExplanation; 
			} else if (skillIndex == 2) {
				mouseOverUI.text = "스킬이름: " + SkillManager.instance.SkillData.GetSkill ("WarriorA3_L1").SkillName  +"\n"+ "쿨타임: " + SkillManager.instance.SkillData.GetSkill ("WarriorA3_L1").SkillCoolTime.ToString () +"초"
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorA3_L1").SkillExplanation 
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorA3_L"+(skillLevel)).SkillDamegeExplanation; 
			} else if (skillIndex == 3) {
				mouseOverUI.text = "스킬이름: " + SkillManager.instance.SkillData.GetSkill ("WarriorA4_L1").SkillName  +"\n"+ "쿨타임: "+ SkillManager.instance.SkillData.GetSkill ("WarriorA4_L1").SkillCoolTime.ToString () +"초"
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorA4_L1").SkillExplanation 
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorA4_L"+(skillLevel)).SkillDamegeExplanation; 
			} else if (skillIndex == 4) {
				mouseOverUI.text = "스킬이름: " + SkillManager.instance.SkillData.GetSkill ("WarriorP1_L1").SkillName  +"\n"+ "쿨타임: " + SkillManager.instance.SkillData.GetSkill ("WarriorP1_L1").SkillCoolTime.ToString () +"초"
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorP1_L1").SkillExplanation 
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorP1_L"+(skillLevel)).SkillDamegeExplanation; 
			} else if (skillIndex == 5) {
				mouseOverUI.text = "스킬이름: " + SkillManager.instance.SkillData.GetSkill ("WarriorP2_L1").SkillName  +"\n"+ "쿨타임: " + SkillManager.instance.SkillData.GetSkill ("WarriorP2_L1").SkillCoolTime.ToString () +"초"
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorP2_L1").SkillExplanation 
					+ "\n" + SkillManager.instance.SkillData.GetSkill ("WarriorP2_L"+(skillLevel)).SkillDamegeExplanation; 
			} else {
				return;
			}
			#endregion  
		#region 법사 스킬 UI
		} else if (classIndex == 1) {

		#endregion
		#region 거너 스킬 UI
		} else if (classIndex == 2) {
		#endregion
		#region 방패병UI
		} else if (classIndex == 3) {
		#endregion 
		}
	}
}
