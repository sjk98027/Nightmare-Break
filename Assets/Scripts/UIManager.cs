using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    public Image hpUI;
    [SerializeField]
    public Image mpUI;
    [SerializeField]
    private Image potionUI;
    private const float checkTime = 0.1f;
    [SerializeField]
    private Image[] skillUI; // 0 - SKill1 // 1 - SKill2 // 2 - Skill3 // 3 - Skill4 //
    public Image bossHp;


    public IEnumerator SkillCoolTimeUI(int skillNum, float coolTime)
    {
        skillUI[skillNum].gameObject.SetActive(true);
        skillUI[skillNum].fillAmount = 1;

        float time = Time.smoothDeltaTime;

        while (skillUI[skillNum].fillAmount != 0.0f)
        {
            skillUI[skillNum].fillAmount -= 1 * time / coolTime;
            yield return null;
           
        }
        time = 0;
        
        yield break;
    }

    public IEnumerator PotionCoolTimeUI()
    {
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
        yield break;
    }

    void HpUI(float currentHP)
    {

    }

    void MpUI(float currentMP)
    {

    }



    /*
    IEnumerator HealthPointUI()
    {
        while (true)
        {
            hpUI.fillAmount = (float)(1 / characterInfo.healthPoint);
            yield return new WaitForSeconds(checkTime);
        }
    }

    IEnumerator ManaPointUI()
    {
        while (true)
        {
            mpUI.fillAmount = (float)(1 / characterInfo.manaPoint);
            yield return new WaitForSeconds(checkTime);
        }
    }

    IEnumerator SkillCoolTimeUI()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                skillUI[i].fillAmount = 1 / characterSkill.skillCoolTime[i];
            }

            yield return new WaitForSeconds(checkTime);
        }
    }

    IEnumerator PotionCoolTimeUI()
    {
        while (true)
        {
            potionUI.fillAmount = (float)(characterSkill.skillCoolTime[4]);
            yield return new WaitForSeconds(checkTime);
        }
    }
    */


}
