using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blink", menuName = "Spell/Blink")]
public class Blink : Skill
{
    public override void Execute(Vector3 t_pos)
    {
        GameManager.Instance.PlayAFX(afxs[0]);
        if (Vector3.Distance(t_pos, myUnit.transform.position) > range[level])
        {
            Vector3 direction = t_pos - myUnit.transform.position;
            Vector3 clampedDirection = Vector3.ClampMagnitude(direction, range[level]);
            Vector3 clampedPoint = myUnit.transform.position + clampedDirection;            
            myUnit.transform.position = clampedDirection;
        }
        else myUnit.transform.position = t_pos;
    }
}
