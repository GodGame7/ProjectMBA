using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SnipeShot", menuName = "Skill/Archer/SnipeShot")]
public class SnipeShot : Skill
{
    Unit t_unit;
    public GameObject arrow_snipe;
    public GameObject snipeEffect;
    public float speed = 10f;
    public override void Activate(Unit t_unit)
    {
        myUnit.anim.Play("Sniping_In");
    }
    public override void Execute(Unit t_unit)
    {
        myUnit.anim.Play("Sniping_Attack");
        Vector3 dir = t_unit.transform.position - myUnit.transform.position;
        Quaternion initialRotation = Quaternion.LookRotation(dir);        
        // 중앙 화살 발사
        GameObject obj = Instantiate(arrow_snipe, myUnit.shotPos.position, initialRotation);
        GameObject effect = Instantiate(snipeEffect);
        obj.GetComponent<Arrow_Snipe>().Init(myUnit, t_unit, dmgs[level - 1], effect);
    }

}
