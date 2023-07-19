using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrossFireShoot", menuName = "Skill/Archer/CrossFire")]
public class CrossFire : Skill
{
    public GameObject spawner;
    public override void Activate(Vector3 t_pos)
    {
        myUnit.anim.Play("CrossFire");        
    }
    public override void Execute(Vector3 t_pos)
    {
        GameManager.Instance.PlayAFX(afxs[0]);
        GameObject s = Instantiate(spawner, myUnit.transform.position, Quaternion.identity);
        s.GetComponent<CrossFireSpawner>().Init(myUnit, t_pos, dmgs[level - 1], level * 3, area[level-1]);
    }

}
