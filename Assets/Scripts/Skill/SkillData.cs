using System.Collections.Generic;

public class SkillData{

    private const int MaxSkillIndex = 6;
    private int searchIndex;
    List<SkillBasicData> skillList = new List<SkillBasicData>();

    public SkillBasicData GetSkill(int _classType, int _skillindex)
    {
        searchIndex = _classType * MaxSkillIndex + _skillindex;
        return skillList[searchIndex];
    }

    public void Initialize()
    {
        skillList.Add(new SkillBasicData(SkillBasicData.SkillTypes.None," ", 0, 0, 0, " ")); //
        #region 전사 스킬리스트
        skillList.Add(new SkillBasicData(SkillBasicData.SkillTypes.Active, "소용돌이", 1, 40, 5, "회전공격을 하며 주위 적들에게 광역공격을 한다."));
        skillList.Add(new SkillBasicData(SkillBasicData.SkillTypes.Active, "섬단", 2, 50, 3, "빠른속도로 전방의 적들을 베어버린다."));
        skillList.Add(new SkillBasicData(SkillBasicData.SkillTypes.Active, "검무", 3, 100, 30, "검으로 근접한 적들을 도륙하는 공격을한다."));
        skillList.Add(new SkillBasicData(SkillBasicData.SkillTypes.Active, "기간틱소드", 4, 150, 40, "검으로 근접한 적들을 도륙하는 공격을한다."));
        skillList.Add(new SkillBasicData(SkillBasicData.SkillTypes.Passive, "흡혈공격", 5, "일반 공격시 준 데미지의 일정량을 체력으로 회복한다."));
        skillList.Add(new SkillBasicData(SkillBasicData.SkillTypes.Passive, "극의", 6, 100, 30, "무기 공격력의 일정량의 추가 방어력을 얻는다."));

        skillList[1].AddSkillLevelData(new SkillLevelData(1, 1.2f, "무기 공격력에 120%의 공격력을 가진다."));
        skillList[1].AddSkillLevelData(new SkillLevelData(2, 1.4f, "무기 공격력에 140%의 공격력을 가진다."));
        skillList[1].AddSkillLevelData(new SkillLevelData(3, 1.2f, "무기 공격력에 160%의 공격력을 가진다."));
        skillList[1].AddSkillLevelData(new SkillLevelData(4, 1.4f, "무기 공격력에 180%의 공격력을 가진다."));

        skillList[2].AddSkillLevelData(new SkillLevelData(1, 1.1f, "무기 공격력에 110%의 공격력을 가진다."));
        skillList[2].AddSkillLevelData(new SkillLevelData(2, 1.2f, "무기 공격력에 120%의 공격력을 가진다."));
        skillList[2].AddSkillLevelData(new SkillLevelData(3, 1.3f, "무기 공격력에 130%의 공격력을 가진다."));
        skillList[2].AddSkillLevelData(new SkillLevelData(4, 1.4f, "무기 공격력에 140%의 공격력을 가진다."));

        skillList[3].AddSkillLevelData(new SkillLevelData(1, 1.2f, "무기 공격력에 120%의 공격력을 가진다."));
        skillList[3].AddSkillLevelData(new SkillLevelData(2, 1.4f, "무기 공격력에 140%의 공격력을 가진다."));
        skillList[3].AddSkillLevelData(new SkillLevelData(3, 1.6f, "무기 공격력에 160%의 공격력을 가진다."));
        skillList[3].AddSkillLevelData(new SkillLevelData(4, 1.8f, "무기 공격력에 180%의 공격력을 가진다."));

        skillList[4].AddSkillLevelData(new SkillLevelData(1, 1.2f, "무기 공격력에 120%의 공격력을 가진다."));
        skillList[4].AddSkillLevelData(new SkillLevelData(2, 2.2f, "무기 공격력에 220%의 공격력을 가진다."));

        skillList[5].AddSkillLevelData(new SkillLevelData(1, 0.05f, "평타데미지의 5%를 흡수한다."));
        skillList[5].AddSkillLevelData(new SkillLevelData(2, 0.1f, "평타데미지의 10%를 흡수한다."));
        skillList[5].AddSkillLevelData(new SkillLevelData(3, 0.15f, "평타데미지의 15%를 흡수한다."));
        skillList[5].AddSkillLevelData(new SkillLevelData(4, 0.2f, "평타데미지의 20%를 흡수한다."));

        skillList[6].AddSkillLevelData(new SkillLevelData(1, 0.5f, "무기 공격력의 50%의 추가 방어력을 얻는다."));
        skillList[6].AddSkillLevelData(new SkillLevelData(2, 1.0f, "무기 공격력의 100%의 추가 방어력을 얻는다."));
        skillList[6].AddSkillLevelData(new SkillLevelData(3, 1.5f, "무기 공격력의 150%의 추가 방어력을 얻는다."));
        skillList[6].AddSkillLevelData(new SkillLevelData(4, 2.0f, "무기 공격력의 200%의 추가 방어력을 얻는다."));

        #endregion
    }
}

public class SkillBasicData
{
    public enum SkillTypes
    {
        None,
        Active,
        Passive
    }

    private string skillName;
    private string skillTypes;
    private string basicExplanation;
    private int skillCoolTime;
    private int skillIndex;
    private int manaCost;
    List<SkillLevelData> skillLevelData = new List<SkillLevelData>();

    public string SkillName {get { return skillName; } }
    public string SkillType {get { return skillTypes; } }
    public string SkillBasicExplanation { get { return basicExplanation; } }
    public int SkillCoolTime { get { return skillCoolTime; } }
    public int SkillIndex { get { return skillIndex; } }
    public int ManaCost { get { return manaCost; } }
    public List<SkillLevelData> SkillData { get { return skillLevelData; } }

    public SkillBasicData()
    {
        skillName = " ";
        skillTypes = " ";
        basicExplanation = " ";
        skillCoolTime = 0;
        skillIndex = 0;
        manaCost = 0;
    }

    public SkillBasicData(SkillTypes _SkillTypes, string _skillName , int _skillIndex, int _manaCost, int _skillCoolTime, string _basicExplanation)
    {
        skillName = _skillName;
        skillTypes = _SkillTypes.ToString();
        skillIndex = _skillIndex;
        skillCoolTime = _skillCoolTime;
        manaCost = _manaCost;
        basicExplanation = _basicExplanation;
    }

    public SkillBasicData(SkillTypes _SkillTypes, string _skillName, int _skillIndex, string _basicExplanation)
    {
        skillName = _skillName;
        skillTypes = _SkillTypes.ToString();
        skillIndex = _skillIndex;
        basicExplanation = _basicExplanation;
    }

    public void AddSkillLevelData(SkillLevelData _data)
    {
        skillLevelData.Add(_data);
    }

    public SkillLevelData GetSkillData(int _level)
    {
        return SkillData[_level - 1];
    }

}


public class SkillLevelData
{
    private int level;
    private float skillValue;
    private string skillExplanation;

    public int Level { get { return level; } }
    public float SkillValue { get { return skillValue; } }
    public string SkillExplanation { get { return skillExplanation; } }

    public SkillLevelData(int _level, float _skillValue, string _skillExplanation)
    {
        level = _level;
        skillValue = _skillValue;
        skillExplanation = _skillExplanation;
    }

}