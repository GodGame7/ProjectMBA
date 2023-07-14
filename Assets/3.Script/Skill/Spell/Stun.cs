using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stun", menuName = "Spell/Stun")]
public class Stun : Skill
{
    public GameObject stone;
    public override void Execute(Unit t_unit)
    {
        GameObject obj = Instantiate(stone, myUnit.shotPos.position, Quaternion.identity);
        obj.GetComponent<Stone>().Init(myUnit, t_unit, this);
    }
}
