using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    Unit myUnit;
    Unit t_unit { get { return myUnit.state_attack.GetTargetUnit(); } }
    public GameObject arrowPrefabs;
    [SerializeField] Transform shootPos;
    private void Start()
    {
        TryGetComponent(out myUnit);
    }
    public void OnAttack()
    {
        myUnit.currentAttackCoolTime = 0;
        GameObject obj = Instantiate(arrowPrefabs, shootPos.position, Quaternion.identity);
        obj.GetComponent<ArcherArrow>().Init(t_unit, myUnit.atk);
    }
    public void AttackEnd()
    {
        myUnit.isAttacking = false;
    }
}
