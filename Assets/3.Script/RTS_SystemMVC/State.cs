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
            if (targetUnit != null) { myUnit.state_attack.Init(targetUnit, 0); myUnit.SetState(myUnit.state_attack); }
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

public class SkillState : IState
{
    #region 변수들
    Unit myUnit;
    Unit t_unit;
    Vector3 t_pos;
    #endregion
    public void Enter()
    {
      
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
       
    }
}
//todo

