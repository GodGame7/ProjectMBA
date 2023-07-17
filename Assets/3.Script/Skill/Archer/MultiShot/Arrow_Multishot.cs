using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Multishot : MonoBehaviour
{
    [SerializeField] Unit myUnit;
    [SerializeField] Vector3 startPos;
    [SerializeField] float speed = 5f;
    [SerializeField] float range;
    [SerializeField] float dmg;
    [SerializeField] private List<GameObject> hitTargets = new List<GameObject>();
    private void Update()
    {
        if (myUnit != null)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            float distance = Vector3.Distance(startPos, transform.position);
            if (distance >= range)
            {
                Destroy(gameObject, 0.1f);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ºÎµúÇû´Ù");
        if (hitTargets.Contains(other.gameObject))
        {
            return;
        }
        Unit enemy;
        if (other.TryGetComponent(out enemy))
        {
            if (myUnit.GetTeam() != enemy.GetTeam())
            {
                hitTargets.Add(enemy.gameObject);
                enemy.OnDamage(myUnit, dmg);
            }
        }        
    }
    public void Init(Unit user, float range, float dmg, float speed)
    {
        myUnit = user;
        this.range = range;
        this.dmg = dmg;
        this.speed = speed;
        startPos = myUnit.shotPos.position;
    }
}
