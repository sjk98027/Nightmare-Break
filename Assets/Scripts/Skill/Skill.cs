using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Skill
{

	protected delegate void SetSkill();
	protected SetSkill setSkill;

    protected const int maxSkillLevel = 4;
    protected const int minSkillLevel = 1;
    protected const int maxSkillIndex = 6;
	protected int[] currentSkillLevel = new int[maxSkillIndex];  //6개의 스킬레벨을 가진다.
    protected int[] skillCoolTime = new int[maxSkillLevel]; // 1번~4번까지의 액티브 스킬 쿨타임을 가진다
    protected float[] skillDamage = new float[maxSkillLevel]; //1번~4번까지의 스킬 데미지
    protected string[] skillString = new string[maxSkillLevel];

    public int MaxSkillLevel { get { return maxSkillIndex; } }
    public float[] SkillDamage { get { return skillDamage; } }
    public int[] SkillCoolTime { get { return skillCoolTime; } }
   
	protected virtual void ActiveSkill1(){  }
	protected virtual void ActiveSkill2(){  }
	protected virtual void ActiveSkill3(){  }
	protected virtual void ActiveSkill4(){  }

	protected virtual void PassiveSkill1(){ }
	protected virtual void PassiveSkill2(){ }

	private void CallSkillMethod(){ setSkill += ActiveSkill1; setSkill += ActiveSkill2; setSkill += ActiveSkill3; setSkill += ActiveSkill4; setSkill += PassiveSkill1; setSkill += PassiveSkill2; setSkill ();}

    public void SkillSetting(int[] skillLevel)
    {
        for (int i = 0; i < maxSkillIndex; i++)
        {
            if(skillLevel[i] > maxSkillLevel)
            {
                skillLevel[i] = maxSkillLevel;
            } else if(skillLevel[i] < minSkillLevel)
                {
                skillLevel[i] = minSkillLevel;
                }
            currentSkillLevel[i] = skillLevel[i];
        }
		CallSkillMethod ();
    }
}
