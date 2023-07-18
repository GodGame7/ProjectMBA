using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    void Enter();
    void Exit();
    void FixedUpdate();
    void LateUpdate();
    void Update();
}
public class IdleState : IState
{
    Unit myUnit;
    Vector3 searchArea;
    Unit targetUnit;
    public IdleState(Unit myUnit)
    {
        this.myUnit = myUnit;
        searchArea = new Vector3(myUnit.range * 1.1f, myUnit.range * 1.1f, myUnit.range * 1.1f);
    }
    public void Enter()
    {
        myUnit.Stop();
        myUnit.anim.Play("Idle");
    }
    public void Exit()
    {
        
    }
    public void FixedUpdate()
    {
        
    }
    public void LateUpdate()
    {
        
    }
    public void Update()
    {
        if (Search())
        {
            if (targetUnit != null) { myUnit.cm.AddCommand(new AttackCommand(myUnit, targetUnit)); }
            else Debug.Log("Idle state searched target but targetUnit is null, IDK why doesn't");
        }
    }
    bool Search()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Unit");
        Collider[] hitted =
            Physics.OverlapBox(myUnit.transform.position, searchArea, Quaternion.identity, layerMask);
        if (hitted != null)
        {
            foreach (var col in hitted)
            {
                if (col.GetComponent<Unit>() != null)
                {
                    Unit hittedUnit = col.GetComponent<Unit>();
                    if (hittedUnit.GetTeam() != myUnit.GetTeam() && hittedUnit.isAlive)
                    { targetUnit = col.GetComponent<Unit>(); return true; }                    
                }
            }
        }
        return false;
    }
}

public class AttackState : IState
{
    #region 변수들
    Unit myUnit;
    Unit t_unit;
    Vector3 t_pos;
    Vector3 searchArea;
    float rotationSpeed = 10f; // 회전 속도 조정
    bool isTarget { get { return t_unit != null; } }
    float attackSpeed { get { return myUnit.attackSpeed; } }
    float attackCoolTime { 
        get { return myUnit.attackCoolTime; } 
        set { myUnit.attackCoolTime = value; } }
    float currentAttackCoolTime {
        get { return myUnit.currentAttackCoolTime; }
        set { myUnit.currentAttackCoolTime = value; } }
    bool isAttacking { 
        get { return myUnit.isAttacking; } 
        set { myUnit.isAttacking = value; } }
    Animator animator;
    #endregion
    bool isArrive()
    {
        if (Vector3.Distance(myUnit.transform.position, t_pos) < 0.1f)
        {
            myUnit.transform.position = t_pos;
            return true;
        }
        return false;
    }
    public AttackState(Unit myUnit)
    {
        this.myUnit = myUnit;
        t_unit = null;
        this.t_pos = myUnit.transform.position;
        searchArea = new Vector3(myUnit.range*1.1f, myUnit.range * 1.1f, myUnit.range * 1.1f);
        animator = myUnit.anim;
    }
    public void Init(Unit targetUnit, int index)
    {
        t_unit = targetUnit;
        t_pos = t_unit.transform.position - ((t_unit.transform.position - myUnit.transform.position).normalized);
        if(index == 0) t_pos = myUnit.transform.position;
    }    
    public void Init(Vector3 t_pos)
    {
        t_unit = null;
        this.t_pos = t_pos;
    }
    #region 빈 메소드
    public void Enter()
    {

    }
    public void Exit()
    {
        isAttacking = false;
    }
    public void FixedUpdate()
    {

    }
    public void LateUpdate()
    {
        
    }
    #endregion
    public void Update()
    {
        if (!isAttacking)
        {
            if (isTarget)
            {
                if (t_unit.isAlive)
                {
                    if (isInRange(t_unit))
                    {
                        myUnit.Stop();
                        Quaternion targetRotation = Quaternion.LookRotation(t_unit.transform.position - myUnit.transform.position);
                        myUnit.transform.rotation = Quaternion.Slerp(myUnit.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * attackSpeed);
                        if (currentAttackCoolTime >= attackCoolTime)
                        {
                            Attack();
                        }
                    }
                    else myUnit.MoveTo(t_unit.transform.position);
                }
                else t_unit = null;
            }
            else if (!isTarget)
            {
                myUnit.MoveTo(t_pos);
                Search();
                if (isArrive())
                {
                    if (isArrive())
                    {
                        myUnit.SetState(myUnit.state_idle);
                    }
                }
            }
        }
        else {
            Quaternion targetRotation = Quaternion.LookRotation(t_unit.transform.position - myUnit.transform.position);
            myUnit.transform.rotation = Quaternion.Slerp(myUnit.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * attackSpeed);
        }
    }
    bool isInRange(Unit target)
    {
        float distance = Vector3.Distance(myUnit.transform.position, target.transform.position);
        return distance <= myUnit.range;
    }
    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }    
    void Search()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Unit");
        Collider[] hitted =
            Physics.OverlapBox(myUnit.transform.position, searchArea, Quaternion.identity, layerMask);
        if (hitted != null)
        {
            foreach (var col in hitted)
            {
                if (col.GetComponent<Unit>().GetTeam() != myUnit.GetTeam() && col.GetComponent<Unit>().isAlive)
                {
                    t_unit = col.GetComponent<Unit>();
                    return;
                }
            }
        }
    }
    public Unit GetTargetUnit()
    {
        return t_unit;
    }
}

public class MoveState : IState
{
    Unit myUnit;
    Vector3 t_pos;
    bool isArrive()
    {
        if (Vector3.Distance(myUnit.transform.position, t_pos) < 0.1f)
        {
            myUnit.transform.position = t_pos;
            return true;
        }
        return false;
    }
    public MoveState(Unit myUnit)
    {
        this.myUnit = myUnit;
        t_pos = myUnit.transform.position;
    }
    public void Init(Vector3 t_pos)
    {
        this.t_pos = t_pos;
    }
    public void Enter()
    {
        myUnit.anim.Play("Idle");
        myUnit.MoveTo(t_pos);
    }
    public void Exit()
    {
        myUnit.Stop();
    }
    public void FixedUpdate()
    {
    }
    public void LateUpdate()
    {
    }
    public void Update()
    {
        if (isArrive())
        {
            myUnit.SetState(myUnit.state_idle);
        }
    }
}

public class DieState : IState
{
    Unit myUnit;
    void Revive()
    {
        if (myUnit.reviveTime > 0) myUnit.reviveTime -= Time.deltaTime;
        else if (myUnit.reviveTime <= 0) myUnit.Revival();
    }
    public DieState(Unit myUnit)
    {
        this.myUnit = myUnit;
    }
    public void Enter()
    {
        myUnit.Death();
    }

    public void Exit()
    {

    }

    public void FixedUpdate()
    {
    }

    public void LateUpdate()
    {
        Revive();
    }

    public void Update()
    {
    }
}

public enum StateInSkill { None, Activated, Executed, Exited }
public class SkillState : IState
{
    #region 변수들
    Skill skill;
    Unit myUnit;
    Unit t_unit;
    Vector3 t_pos;
    #endregion
    StateInSkill state = StateInSkill.None;
    float activateTime;
    int index;
    public SkillState(Unit user)
    {
        myUnit = user;
    }
    public void Init(Skill skill, int index)
    {
        this.skill = skill;
        t_unit = null;
        t_pos = myUnit.transform.position;
        this.index = index;
        activateTime = skill.activateTime[skill.level - 1];
        this.skill.Init(myUnit);
    }
    public void Init(Skill skill, int index, Unit t_unit)
    {
        this.skill = skill;
        this.t_unit = t_unit;
        t_pos = t_unit.transform.position;
        this.index = index;
        activateTime = skill.activateTime[skill.level - 1];
        this.skill.Init(myUnit);
    }
    public void Init(Skill skill, int index, Vector3 t_pos)
    {
        this.skill = skill;
        t_unit = null;
        this.t_pos = t_pos;
        this.index = index;
        activateTime = skill.activateTime[skill.level - 1];
        this.skill.Init(myUnit);
    }
    public void Enter()
    {
        myUnit.anim.Play("Wait");
        ActivateSkill();
    }
    public void Exit()
    {
        state = StateInSkill.None;
        skill = null;
        t_unit = null;        
    }
    public void FixedUpdate()
    {
      
    }
    public void LateUpdate()
    {
      
    }
    public void Update()
    {
        if (skill != null)
        {
            switch (state)
            {
                case StateInSkill.None: break;
                case StateInSkill.Activated:
                    if (activateTime > 0)
                    {
                        Rotate();
                        activateTime -= Time.deltaTime;
                        if (activateTime <= 0.01f)
                        {
                            activateTime = 0f;
                            ExecuteSkill();
                        }
                    }
                    break; 
                case StateInSkill.Executed: if (myUnit.isAlive) { myUnit.SetState(myUnit.state_idle); } break;
            }
        }
    }
    void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(t_pos - myUnit.transform.position);
        myUnit.transform.rotation = Quaternion.Slerp(myUnit.transform.rotation, targetRotation, 10f * Time.deltaTime / activateTime);
    }
    void ActivateSkill()
    {
        if (skill != null)
        {
            switch (skill.inputType)
            {
                case InputType.Instant: skill.Activate(); state = StateInSkill.Activated;  break;
                case InputType.Target: skill.Activate(t_unit); state = StateInSkill.Activated; break;
                case InputType.NonTarget: skill.Activate(t_pos); state = StateInSkill.Activated; break;
            }
        }
    }
    void ExecuteSkill()
    {
        if (skill != null)
        {
            switch (skill.inputType)
            {
                case InputType.Instant: skill.Execute(); state = StateInSkill.Executed; myUnit.sm.SkillUsed(index); break;
                case InputType.Target: skill.Execute(t_unit); state = StateInSkill.Executed; myUnit.sm.SkillUsed(index); break;
                case InputType.NonTarget: skill.Execute(t_pos); state = StateInSkill.Executed; myUnit.sm.SkillUsed(index); break;
            }
        }
    }
}
//스킬 시전 중에 캐릭터를 조작하지 못하도록 하기 위한 State
//스킬 Execute()문에서 접근해야 함
public class PerformState : IState
{
    Unit myUnit;
    float time;
    Skill skill;
    public PerformState(Unit myUnit)
    {
        this.myUnit = myUnit;
    }
    public void Init(float time, Skill skill)
    {
        this.time = time;
        this.skill = skill;
    }
    public void Enter()
    {
        myUnit.Stop();
    }
    public void Exit()
    {
    }
    public void FixedUpdate()
    {
    }
    public void LateUpdate()
    {
    }
    public void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0.001f)
            {
                skill.Exit();
                time = 0;
                if(myUnit.isAlive) myUnit.SetState(myUnit.state_idle);
                myUnit.cm.AddCommand(new StopCommand(myUnit));
            }
        }
    }
}
//todo