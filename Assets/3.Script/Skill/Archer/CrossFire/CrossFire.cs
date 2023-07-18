using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrossFireShoot", menuName = "Skill/Archer/CrossFire")]
public class CrossFire : Skill
{
    public override void Activate(Vector3 t_pos)
    {
        GameManager.Instance.PlayAFX(afxs[0]);
        //todo 애니메이션 재생
    }
}
