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

	public Image[] SkillUI {
		get {
			return this.skillUI;
		}
	}

	
    public Image bossHp;


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

    void HpUI(int maxHp, int currentHP)
    {
        hpUI.fillAmount = maxHp / currentHP;
    }

    void MpUI(int maxMp, int currentMP)
    {
        mpUI.fillAmount = maxMp / currentMP;
    }
		


}
