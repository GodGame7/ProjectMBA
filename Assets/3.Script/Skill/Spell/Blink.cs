using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blink", menuName = "Spell/Blink")]
public class Blink : Skill
{
    public override void Execute(Vector3 t_pos)
    {
        Debug.Log("Blink.Execute(Vector3 t_pos) ½ÇÇà");
        GameManager.Instance.PlayAFX(afxs[0]);
        if (Vector3.Distance(t_pos, myUnit.transform.position) > range[level - 1])
        {
            Vector3 direction = t_pos - myUnit.transform.position;
            Vector3 clampedDirection = Vector3.ClampMagnitude(direction, range[level - 1]);
            Vector3 clampedPoint = myUnit.transform.position + clampedDirection;
            myUnit.Blink(clampedPoint);
        }
        else myUnit.Blink(t_pos);
        
    }
}
