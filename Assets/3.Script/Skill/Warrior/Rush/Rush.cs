using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rush", menuName = "Skill/Warrior/Rush")]
public class Rush : Skill
{
    [Header("Rush Skill Setting")]
    [Tooltip("시전 후 적에게 도달하는데 걸리는 시간")]
    public float rushSpeed;
    float curSpeed;
    Unit t_unit;

    public override void Activate(Unit t_unit)
    {
        this.t_unit = t_unit;
    }
    public override void Execute(Unit t_unit)
    {
        myUnit.cm.AddCommand(new PerformCommand(myUnit, rushSpeed, this));
        GameManager.Instance.PlayAFX(afxs[0]);
        myUnit.transform.LookAt(t_unit.transform);
        myUnit.Rush(t_unit, rushSpeed);
    }
    public override void Exit()
    {
        t_unit.OnDamage(myUnit, dmgs[level - 1]);
        t_unit.OnStun(myUnit, ccDurationTime);
    }
}
