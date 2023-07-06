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
    AttackState attackState;
    public IdleState(Unit myUnit)
    {
        this.myUnit = myUnit;
        searchArea = new Vector3(myUnit.range, myUnit.range, myUnit.range);
    }
    public void Enter()
    {
        myUnit.Stop();
        if (attackState == null)
        {
            attackState = new AttackState(myUnit);
        }
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
            if (targetUnit != null) { attackState.Init(targetUnit); myUnit.SetState(attackState); }
            else Debug.Log("Idle state searched target but targetUnit is null, IDK why doesn't");
        }
    }
    bool Search()
    {
        Collider[] hitted =
            Physics.OverlapBox(myUnit.transform.position, searchArea, Quaternion.identity, LayerMask.NameToLayer("Unit"));
        if (hitted != null)
        {
            foreach (var col in hitted)
            {
                if (col.GetComponent<Unit>() != null)
                {
                    Unit hittedUnit = col.GetComponent<Unit>();
                    if (hittedUnit.GetTeam() != myUnit.GetTeam())
                    { targetUnit = col.GetComponent<Unit>(); return true; }                    
                }
            }
        }
        return false;
    }
}


public class AttackState : IState
{
    Unit myUnit;
    Unit t_unit;
    Vector3 t_pos;
    bool isTarget { get { return t_unit != null; } }
    float attackSpeed;
    float attackCoolTime;
    float currentAttackCoolTime;
    Animator animator;
    Vector3 searchArea;
    public AttackState(Unit myUnit)
    {
        this.myUnit = myUnit;
        t_unit = null;
        this.t_pos = myUnit.transform.position;
        searchArea = new Vector3(myUnit.range, myUnit.range, myUnit.range);
    }
    public void Init(Unit targetUnit)
    {
        t_unit = targetUnit;
    }
    public void Init(Vector3 t_pos)
    {
        this.t_pos = t_pos;
    }
    public void Enter()
    {
        SetAttackSpeed(myUnit.attackSpeed);
    }

    public void Exit()
    {

    }

    public void FixedUpdate()
    {

    }

    public void LateUpdate()
    {
        currentAttackCoolTime += Time.deltaTime;
    }

    public void Update()
    {
        if (isTarget)
        {
            if (isInRange(t_unit))
            {
                myUnit.Stop();
                //todo 사거리 안에 있을 시 공격
                if (currentAttackCoolTime >= attackCoolTime)
                {
                    //쿨타임 초기화하고 공격을 개시한다.
                    currentAttackCoolTime = 0;
                    Attack();
                }
            }
            else myUnit.MoveTo(t_unit.transform.position);
        }
        else if (!isTarget)
        {
            myUnit.MoveTo(t_pos);
            Search();
        }        
    }
    bool isInRange(Unit target)
    {
        float distance = Vector3.Distance(myUnit.transform.position, target.transform.position);
        return distance <= myUnit.range;
    }
    void SetAttackSpeed(float _attackSpeed)
    {
        attackSpeed = _attackSpeed;
        //공격 쿨타임 계산
        attackCoolTime = 1f / attackSpeed;
        currentAttackCoolTime = attackCoolTime;
        //공격속도가 1보다 빠르면 애니메이션 빠르게 재생하기 위해서 배속 설정, 아니면 기본속도 1로 재생
        if (attackSpeed > 1) animator.SetFloat("AttackSpeed", attackSpeed);
        else animator.SetFloat("AttackSpeed", 1);
    }
    void Attack()
    {
        animator.SetTrigger("Attack");
    }    
    bool Search()
    {
        Collider[] hitted =
            Physics.OverlapBox(myUnit.transform.position, searchArea, Quaternion.identity, LayerMask.NameToLayer("Unit"));
        if (hitted != null)
        {
            foreach (var col in hitted)
            {
                if (col.GetComponent<Unit>().GetTeam() != myUnit.GetTeam())
                {
                    t_unit = col.GetComponent<Unit>();
                    return true;
                }
            }
        }
        return false;
    }
}

public class MoveState : IState
{
    Unit myUnit;
    Vector3 t_pos;

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
    }
}
