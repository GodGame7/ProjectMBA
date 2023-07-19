using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiShot", menuName = "Skill/Archer/MultiShot")]
public class MultiShot : Skill
{
    public int arrowCount;
    public GameObject arrow_multishot;
    public float fanAngle = 30f; // 부채꼴의 각도 설정
    public float speed = 10f;
    public override void Activate(Vector3 t_pos)
    {
        myUnit.anim.Play("MultiShot");
    }
    public override void Execute(Vector3 t_pos)
    {        
        Vector3 dir = t_pos - myUnit.transform.position;

        Quaternion initialRotation = Quaternion.LookRotation(dir);
        Quaternion leftRotation = Quaternion.AngleAxis(-fanAngle, Vector3.up);
        Quaternion rightRotation = Quaternion.AngleAxis(fanAngle, Vector3.up);

        // 중앙 화살 발사
        GameObject obj = Instantiate(arrow_multishot, myUnit.shotPos.position, initialRotation);
        obj.GetComponent<Arrow_Multishot>().Init(myUnit, range[level - 1], dmgs[level-1], speed);
        // 왼쪽 화살 발사
        GameObject obj1 = Instantiate(arrow_multishot, myUnit.shotPos.position, initialRotation * leftRotation);
        obj1.GetComponent<Arrow_Multishot>().Init(myUnit, range[level - 1], dmgs[level - 1], speed);
        // 오른쪽 화살 발사
        GameObject obj2 = Instantiate(arrow_multishot, myUnit.shotPos.position, initialRotation * rightRotation);
        obj2.GetComponent<Arrow_Multishot>().Init(myUnit, range[level - 1], dmgs[level - 1], speed);
    }
}
