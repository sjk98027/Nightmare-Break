using System.Collections.Generic;

public class SKillExpatiationStorage  {
    public enum CharacterType
    {
        Warrior,
        Mage,
        ShieldWarrior,
        Gunner
    }
   
    Dictionary<string, SkillInfo> skillDic = new Dictionary<string, SkillInfo>();
    
    void SetDictionary()
    {
        skillDic.Add("전사스킬1", new SkillInfo(SkillInfo.SkillType.Active, "소용돌이", 5, "캐릭터가 회전공격을하며 주위 적들에게 광역공격을한다."));
        skillDic.Add("전사스킬2", new SkillInfo(SkillInfo.SkillType.Active, "섬단", 3, "캐릭터가 빠른속도로 전방의 적들을 베어버린다."));
        skillDic.Add("전사스킬3", new SkillInfo(SkillInfo.SkillType.Active, "검무", 30, "캐릭터가 검으로 근접한 적들을 도륙하는 공격을한다."));
        skillDic.Add("전사스킬4", new SkillInfo(SkillInfo.SkillType.Active, "기간틱소드", 40, "전방의 기간틱소드를 소환하여 적들을 섬멸한다."));
        skillDic.Add("전사스킬5", new SkillInfo(SkillInfo.SkillType.Passive, "흡혈공격", 0, "캐릭터가 일반공격시 준 데미지의 일정량을 체력으로 회복한다."));
        skillDic.Add("전사스킬6", new SkillInfo(SkillInfo.SkillType.Passive, "방어태세", 0, "캐릭터의 현재 방어력의 추가적인 방어력을 증가시킨다."));

    }
}

public class SkillInfo
{
    public enum SkillType
    {
        Active,
        Passive
    }

    string skillType;
    string skillName;
    string skillExplanation;
    int skillCoolTime;
 
    public SkillInfo(SkillType _skillType, string _skillName, int _skillCoolTime ,string _skillExplanation)
    { 
        skillType = _skillType.ToString();
        skillName = _skillName;
        skillCoolTime = _skillCoolTime;
        skillExplanation = _skillExplanation;
    }
    


}