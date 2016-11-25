using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleUIManager {

    public Image hpBar;
    public Image mpBar;
    private Image potionUI;
    private const float checkTime = 0.1f;
    private Image[] skillUI; // 0 - SKill1 // 1 - SKill2 // 2 - Skill3 // 3 - Skill4 //
    public Image[] SkillUI{ get{ return skillUI; }}

    #region 스킬쿨타임 제어 코루틴
    public IEnumerator SkillCoolTimeUI(int skillNum, float coolTime)
    {
        skillUI[skillNum].color += new Color(0, 0, 0, 1);

        float time = Time.smoothDeltaTime;

        while (skillUI[skillNum].fillAmount != 0.0f)
        {
            skillUI[skillNum].fillAmount -= 1 * time / coolTime;
            yield return null;

        }
        skillUI[skillNum].fillAmount = 1;
        skillUI[skillNum].color -= new Color(0, 0, 0, 1);
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
        skillUI = new Image[4];
        for (int i = 0; i < skillUI.Length; i++)
        {
            skillUI[i] = GameObject.Find("Skill" +(i+1)+ "_CoolTime").GetComponent<Image>();
        }
        //  Debug.Log(skillUI[i]);

    }
    

}
