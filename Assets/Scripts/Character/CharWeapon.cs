using UnityEngine;

public class CharWeapon : MonoBehaviour
{
    public GameObject character;
    public CharacterManager charManager;
    public CharacterStatus charStatus;
    public int damage = 0;
    bool normalAttack;
    bool skillAttack;
    int skillLv;

    public bool NormalAttack { get { return this.normalAttack; } }
    public bool SkillAttack { get { return this.skillAttack; } }

    // Use this for initialization
    void Start()
    {
        character = GameObject.FindWithTag("Player");
        charManager = character.GetComponent<CharacterManager>();
        charStatus = GameObject.FindGameObjectWithTag("CharStatus").GetComponent<CharacterStatus>();
        charStatus.SetCharacterStatus();
        skillLv = charStatus.SkillLevel[5];
    }

    // Update is called once per frame
    void Update()
    {
        normalAttack = charManager.NormalAttackState;
        skillAttack = charManager.SkillAttackState;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enermy"))
        {
            Monster monster = coll.gameObject.GetComponent<Monster>();
            // charManager.UIManager.BattleUIManager.monsterHpBarCalculation(monster.gameObject.name, monster.MaxHP, monster.CurrentHP);
            Instantiate(Resources.Load<GameObject>("Effect/HitEffect"), new Vector3(coll.transform.position.x, coll.transform.position.y + 1.0f, coll.transform.position.z + 0.5f), Quaternion.identity);
            if (monster != null)
            {
                if (normalAttack)
                {
                    damage = charManager.charStatus.Attack;
                }
                else if (skillAttack)
                {
                    damage = charManager.charStatus.Attack;
                }

                if (damage != 0)
                {
                    if (charStatus.HClass == CharacterStatus.CharClass.Warrior)
                    {
                        if (charStatus.SkillLevel[5] < 4)
                        {
                            if (normalAttack)
                            {
                                int testPassiveHP;

                                testPassiveHP = (int)((SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 4).GetSkillData(skillLv).SkillValue) * damage);

                                if (charStatus.MaxHealthPoint > charStatus.HealthPoint)
                                {
                                    charStatus.DecreaseHealthPoint(-testPassiveHP);
                                    Debug.Log("blood");
                                }
                            }
                        }
                        else if (charStatus.SkillLevel[5] == 4)
                        {
                            Debug.Log("in Warrior");
                            int testPassiveHP;

                            testPassiveHP = (int)((SkillManager.instance.SkillData.GetSkill((int)charStatus.HClass, 4).GetSkillData(skillLv).SkillValue) * damage);
                            if (charStatus.MaxHealthPoint > charStatus.HealthPoint)
                            {
                                charStatus.DecreaseHealthPoint(-testPassiveHP);
                            }
                        }
                    }
                    monster.HitDamage(damage, this.gameObject.GetComponentInParent<CharacterManager>().gameObject);
                    damage = 0;
                }
            }
        }
    }
}
