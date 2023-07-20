using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blink", menuName = "Spell/Blink")]
public class Blink : Skill
{
    public GameObject flashEffect;
    GameObject efc;
    Vector3 efcPos;
    public float flashRange;
    public override void Execute(Vector3 t_pos)
    {
        GameManager.Instance.PlayAFX(afxs[0]);
        efcPos= myUnit.transform.position;
        if (Vector3.Distance(t_pos, myUnit.transform.position) > flashRange)
        {
            Vector3 direction = t_pos - myUnit.transform.position;
            Vector3 clampedDirection = Vector3.ClampMagnitude(direction, flashRange);
            Vector3 clampedPoint = myUnit.transform.position + clampedDirection;
            myUnit.Blink(clampedPoint);
        }
        else myUnit.Blink(t_pos);
        if (efc != null)
        {
            efc.transform.position = efcPos;
            efc.SetActive(false); efc.SetActive(true);
            return;
        }
        efc = Instantiate(flashEffect, efcPos, flashEffect.transform.rotation);
    }
}
