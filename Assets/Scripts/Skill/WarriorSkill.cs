using UnityEngine;
using System.Collections;

public class WarriorSkill : Skill {

	private int bloodDrain;

    protected override void ActiveSkill1()
    {
        skillCoolTime[0] = 5;

       if(currentSkillLevel[0]==minSkillLevel)
        {
            skillDamage[0] = 1.2f;
        } else if(currentSkillLevel[1] == 2)
        {
            skillDamage[0] = 1.4f;
        } else if(currentSkillLevel[2] == 3)
        {
            skillDamage[0] = 1.6f;
        } else if(currentSkillLevel[3] == maxSkillLevel)
        {
            skillDamage[0] = 1.8f;
        }
		base.ActiveSkill1 ();
    }

    protected override void ActiveSkill2()
    {
        skillCoolTime[1] = 3;
        if (currentSkillLevel[0] == minSkillLevel)
        {
            skillDamage[1] = 1.1f;
        }
        else if (currentSkillLevel[1] == 2)
        {
            skillDamage[1] = 1.2f;

        }
        else if (currentSkillLevel[2] == 3)
        {
            skillDamage[1] = 1.3f;
        }
        else if (currentSkillLevel[3] == maxSkillLevel)
        {
            skillDamage[1] = 1.4f;
        }
    }

    protected override void ActiveSkill3()
    {
        skillCoolTime[2] = 30;
        if (currentSkillLevel[0] == minSkillLevel)
        {
            skillDamage[2] = 1.2f;
        }
        else if (currentSkillLevel[1] == 2)
        {
            skillDamage[2] = 1.4f;
        }
        else if (currentSkillLevel[2] == 3)
        {
            skillDamage[2] = 1.6f;
        }
        else if (currentSkillLevel[3] == maxSkillLevel)
        {
            skillDamage[2] = 1.8f;
        }
    }

    protected override void ActiveSkill4()
    {
        skillCoolTime[3] = 40;
        if (currentSkillLevel[0] == minSkillLevel)
        {
            skillDamage[3] = 1.2f;
        }
        else if (currentSkillLevel[1] == 2)
        {
            skillDamage[3] = 1.4f;
        }
    }

	protected override void PassiveSkill1 ()
	{
		
	}

	protected override void PassiveSkill2 ()
	{

	}

}
