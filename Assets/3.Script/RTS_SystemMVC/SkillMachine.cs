using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StateSkill { ready, activate, cooldown }
public class SkillMachine : MonoBehaviour
{
    #region 변수들
    [System.Serializable]
    public class SkillSlot
    {
        public Image icon;
        public Text cooltimeText;
        public Image cooltimeImage;
        public StateSkill state = StateSkill.ready;
        public float cooltime;
    }
    Unit myUnit;
    [Header("My Unit에서 풀링")]
    [Header("My Skill View")]
    public Skill[] skills;
    [Header("Bottom UI 스킬 슬롯 설정")]
    public SkillSlot[] skillSlots;
    #endregion

    private void Start()
    {
        myUnit = GetComponent<Unit>();
        Init(myUnit);
    }

    private void Update()
    {
        ViewUpdate();
    }
    public bool isSkillReady(int index)
    {
        if (index > skillSlots.Length)
        {
            Debug.Log("인덱스 범위 초과"); return false;
        }
        return skillSlots[index].state == StateSkill.ready;
    }
    public void SkillUsed(int index)
    {
        skillSlots[index].cooltime = skills[index].cooldownTime[skills[index].level - 1];
        skillSlots[index].state = StateSkill.cooldown;
    }




    #region Init&Cashing
    public void Init(Unit unit)
    {
        myUnit = unit;
        CashingSkill();
    }
    void CashingSkill()
    {
        skills = new Skill[6];
        skills[0] = myUnit.skillQ;
        skills[1] = myUnit.skillW;
        skills[2] = myUnit.skillE;
        skills[3] = myUnit.skillR;
        skills[4] = myUnit.skillD;
        skills[5] = myUnit.skillF;

        for (int i = 0; i < skills.Length; i++)
        {
            ViewCashing(i);
        }
    }
    void ViewCashing(int index)
    {
        if (index > skillSlots.Length) return;
        skillSlots[index].icon.sprite = skills[index].icon;
        skillSlots[index].cooltimeText.text = string.Empty;
        skillSlots[index].cooltimeImage.fillAmount = 0;
        skillSlots[index].state = StateSkill.ready;
        skillSlots[index].cooltime = skills[index].cooldownTime[skills[index].level-1];
    }
    #endregion
    void ViewUpdate()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i].state == StateSkill.cooldown)
            {
                skillSlots[i].cooltime -= Time.deltaTime;
                skillSlots[i].cooltimeText.text = skillSlots[i].cooltime.ToString("F1");
                skillSlots[i].cooltimeImage.fillAmount = skillSlots[i].cooltime / skills[i].cooldownTime[skills[i].level-1];
                if (skillSlots[i].cooltime <= 0.01f)
                {
                    skillSlots[i].cooltime = 0;
                    skillSlots[i].cooltimeText.text = string.Empty;
                    skillSlots[i].state = StateSkill.ready;
                }
            }
        }        
    }
}
