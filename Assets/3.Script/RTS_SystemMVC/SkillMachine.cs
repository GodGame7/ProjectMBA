using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMachine : MonoBehaviour
{
    //인풋시스템에서 KeyCode를 key로 스킬 데이터를 가져갈 수 있어야 함.
    //인풋을 어디에서 받을 것인가?
    //SkillView에서 어차피 쿨타임을 관리해야함
    //커맨드패턴으로 어떻게 정리할 것인가?
    public enum SkillState { ready, activate, cooldown }
    SkillState q_state = SkillState.ready;
    SkillState w_state = SkillState.ready;
    SkillState e_state = SkillState.ready;
    SkillState r_state = SkillState.ready;
    SkillState d_state = SkillState.ready;
    SkillState f_state = SkillState.ready;
    [Header("Skill View")]
    [SerializeField] Image q_icon;
    [SerializeField] Image w_icon;
    [SerializeField] Image e_icon;
    [SerializeField] Image r_icon;
    [SerializeField] Image d_icon;
    [SerializeField] Image f_icon;

    [SerializeField] Image q_blue;
    [SerializeField] Image w_blue;
    [SerializeField] Image e_blue;
    [SerializeField] Image r_blue;
    [SerializeField] Image d_blue;
    [SerializeField] Image f_blue;

    [SerializeField] Text q_cooltime;
    [SerializeField] Text w_cooltime;
    [SerializeField] Text e_cooltime;
    [SerializeField] Text r_cooltime;
    [SerializeField] Text d_cooltime;
    [SerializeField] Text f_cooltime;
    [Space]
    [Header("Skill Data")]
    public Skill q_skill;
    public Skill w_skill;
    public Skill e_skill;
    public Skill r_skill;
    public Skill d_skill;
    public Skill f_skill;
    float q_skilltime;
    float w_skilltime;
    float e_skilltime;
    float r_skilltime;
    float d_skilltime;
    float f_skilltime;
    [Header("KeyCode")]
    public KeyCode qKey;
    public KeyCode wKey;
    public KeyCode eKey;
    public KeyCode rKey;
    public KeyCode dKey;
    public KeyCode fKey;


    Unit myUnit;

    private void Start()
    {

    }

    private void Update()
    {
        UpdateSkillCooldownTime();
        UpdateSkillCooldownFillImage();
    }

    #region UseSkill외부접근커맨드
    public void UseQ()
    {
        q_skilltime = q_skill.cooldownTime;
        q_state = SkillState.cooldown;
    }
    public void UseW()
    {
        w_skilltime = w_skill.cooldownTime;
        w_state = SkillState.cooldown;
    }
    public void UseE()
    {
        e_skilltime = e_skill.cooldownTime;
        e_state = SkillState.cooldown;
    }
    public void UseR()
    {
        r_skilltime = r_skill.cooldownTime;
        r_state = SkillState.cooldown;
    }
    public void UseD()
    {
        d_skilltime = d_skill.cooldownTime;
        d_state = SkillState.cooldown;
    }
    public void UseF()
    {
        f_skilltime = f_skill.cooldownTime;
        f_state = SkillState.cooldown;
    }
    #endregion

    #region stateCheck외부커맨드
    public bool isSkillReady(KeyCode keyCode)
    {
        if (keyCode == qKey)
        {
            return q_state == SkillState.ready;
        }
        else if (keyCode == wKey)
        {
            return w_state == SkillState.ready;
        }
        else if (keyCode == eKey)
        {
            return e_state == SkillState.ready;
        }
        else if (keyCode == rKey)
        {
            return r_state == SkillState.ready;
        }
        else if (keyCode == dKey)
        {
            return d_state == SkillState.ready;
        }
        else if (keyCode == fKey)
        {
            return f_state == SkillState.ready;
        }
        else Debug.Log("연결 된 키가 없습니다"); return false;
    }
    #endregion


    void UpdateSkillCooldownTime()
    {
        if (q_state==SkillState.cooldown)
        {
            q_skilltime -= Time.deltaTime; q_cooltime.text = q_skilltime.ToString("F1");
            if (q_skilltime <= 0.01f) { q_skilltime = 0; q_cooltime.text = string.Empty; q_state = SkillState.ready; }
        }
        if (w_state == SkillState.cooldown)
        {
            w_skilltime -= Time.deltaTime; w_cooltime.text = w_skilltime.ToString("F1");
            if (w_skilltime <= 0.01f) { w_skilltime = 0; w_cooltime.text = string.Empty; w_state = SkillState.ready; }
        }
        if (e_state == SkillState.cooldown)
        {
            e_skilltime -= Time.deltaTime; e_cooltime.text = e_skilltime.ToString("F1");
            if (e_skilltime <= 0.01f) { e_skilltime = 0; e_cooltime.text = string.Empty; e_state = SkillState.ready; }
        }
        if (r_state == SkillState.cooldown)
        {
            r_skilltime -= Time.deltaTime; r_cooltime.text = r_skilltime.ToString("F1");
            if (r_skilltime <= 0.01f) { r_skilltime = 0; r_cooltime.text = string.Empty; r_state = SkillState.ready; }
        }
        if (d_state == SkillState.cooldown)
        {
            d_skilltime -= Time.deltaTime; d_cooltime.text = d_skilltime.ToString("F1");
            if (d_skilltime <= 0.01f) { d_skilltime = 0; d_cooltime.text = string.Empty; d_state = SkillState.ready; }
        }
        if (f_state == SkillState.cooldown)
        {
            f_skilltime -= Time.deltaTime; f_cooltime.text = f_skilltime.ToString("F1");
            if (f_skilltime <= 0.01f) { f_skilltime = 0; f_cooltime.text = string.Empty; f_state = SkillState.ready; }
        }
    }
    void UpdateSkillCooldownFillImage()
    {
        q_blue.fillAmount = q_skilltime / q_skill.cooldownTime;
        w_blue.fillAmount = w_skilltime / w_skill.cooldownTime;
        e_blue.fillAmount = e_skilltime / e_skill.cooldownTime;
        r_blue.fillAmount = r_skilltime / r_skill.cooldownTime;
        d_blue.fillAmount = d_skilltime / d_skill.cooldownTime;
        f_blue.fillAmount = f_skilltime / f_skill.cooldownTime;
    }
    public void Init(Unit unit)
    {
        myUnit = unit;

        q_skill = myUnit.skillQ;
        w_skill = myUnit.skillW;
        e_skill = myUnit.skillE;
        r_skill = myUnit.skillR;
        d_skill = myUnit.skillD;
        f_skill = myUnit.skillF;

        q_icon.sprite = q_skill.icon;
        w_icon.sprite = w_skill.icon;
        e_icon.sprite = e_skill.icon;
        r_icon.sprite = r_skill.icon;
        d_icon.sprite = d_skill.icon;
        f_icon.sprite = f_skill.icon;

        q_blue.fillAmount = 0;
        w_blue.fillAmount = 0;
        e_blue.fillAmount = 0;
        r_blue.fillAmount = 0;
        d_blue.fillAmount = 0;
        f_blue.fillAmount = 0;

        q_cooltime.text = string.Empty;
        w_cooltime.text = string.Empty;
        e_cooltime.text = string.Empty;
        r_cooltime.text = string.Empty;
        d_cooltime.text = string.Empty;
        f_cooltime.text = string.Empty;
    }
}
