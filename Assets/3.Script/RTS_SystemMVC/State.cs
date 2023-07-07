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
        attackState = new AttackState(myUnit);
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
        searchArea = new Vector3(myUnit.range, myUnit.range, myUnit.range);
        animator = myUnit.anim;
        SetAttackSpeed(myUnit.attackSpeed);
    }
    public void Init(Unit targetUnit)
    {
        t_unit = targetUnit;
    }    public void Init(Vector3 t_pos)
    {
        t_unit = null;
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
        
    }
    public void Update()
    {
        if (isTarget)
        {
            if (isInRange(t_unit))
            {
                float rotationSpeed = 5f; // 회전 속도 조정
                Quaternion targetRotation = Quaternion.LookRotation(t_unit.transform.position - myUnit.transform.position);
                myUnit.transform.rotation = Quaternion.Slerp(myUnit.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * attackSpeed);
                myUnit.Stop();
                Debug.Log(attackCoolTime);
                if (currentAttackCoolTime >= attackCoolTime)
                {                    
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
            if (isArrive())
            {
                if (isArrive())
                {
                    myUnit.SetState(myUnit.state_idle);
                }
            }
        }
        currentAttackCoolTime += Time.deltaTime;
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
        int layerMask = 1 << LayerMask.NameToLayer("Unit");
        Collider[] hitted =
            Physics.OverlapBox(myUnit.transform.position, searchArea, Quaternion.identity, layerMask);
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

//todo

