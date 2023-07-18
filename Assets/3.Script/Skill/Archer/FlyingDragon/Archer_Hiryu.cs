using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HiryuIksen", menuName = "Skill/Archer/HiryuIksen")]
public class Archer_Hiryu : Skill
{
    [Header("HiryuIksen Setting")]
    public GameObject hiryuEffect;
    public GameObject magicCircle;
    public float speed;
    GameObject obj;
    Vector3 t_pos;
    Vector3 m_pos;
    public override void Activate(Vector3 t_pos)
    {
        this.t_pos = t_pos;
        this.t_pos.y = 0f;
        m_pos = myUnit.transform.position;
        m_pos.y = 0f;
        myUnit.transform.LookAt(t_pos);
    }
    public override void Execute(Vector3 t_pos)
    {
        myUnit.cm.AddCommand(new PerformCommand(myUnit, 3f, this));
        myUnit.anim.Play("Sniping_In");
        obj = Instantiate(magicCircle, myUnit.transform);
        GameManager.Instance.PlayAFX(afxs[0]);       
    }
    public override void Exit()
    {
        Destroy(obj);
        myUnit.anim.Play("Sniping_Out");
        GameManager.Instance.PlayAFX(afxs[1]);
        GameObject hiryu =
            Instantiate(hiryuEffect, myUnit.shotPos.position, Quaternion.LookRotation(t_pos - m_pos));
        hiryu.GetComponent<Arrow_Hiryu>().Init(myUnit, range[level - 1], dmgs[level-1], speed);
    }
}
