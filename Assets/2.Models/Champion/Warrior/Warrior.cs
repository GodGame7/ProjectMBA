using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    Unit myUnit;
    Unit t_unit { get { return myUnit.state_attack.GetTargetUnit(); } }
    private void Start()
    {
        TryGetComponent(out myUnit);
    }
    public void OnAttack()
    {
        myUnit.currentAttackCoolTime = 0;
        t_unit.OnDamage(myUnit, myUnit.atk);
    }
    public void AttackEnd()
    {
        myUnit.isAttacking = false;
    }
}
