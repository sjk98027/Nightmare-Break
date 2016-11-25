using UnityEngine;
using System.Collections;

public class SkillStorage : MonoBehaviour {
    
    SkillManager skill= new SkillManager();

    public SkillManager SetSkill { get { return skill; } }
    public static SkillStorage instance;
    
    void Start()
    {
        instance = this;
        skill.Initialize();
    } 

}
