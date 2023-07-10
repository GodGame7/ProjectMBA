using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrow : MonoBehaviour
{
    Unit t_unit;
    float dmg = 1f;
    float speed = 20f;
    float hitDistance = 1f;
    Vector3 t_pos;
    Vector3 m_pos;
    Vector3 dir;
    private void Update()
    {
        if (t_unit != null)
        {
            t_pos = t_unit.transform.position;
            m_pos = transform.position;
            dir = t_pos - m_pos;
            dir.y = 0f;
            dir = dir.normalized;
            transform.Translate(dir * speed * Time.deltaTime, Space.World) ;
            transform.rotation = Quaternion.LookRotation(dir);

            float distance = Vector3.Distance(transform.position, t_pos);
            if (distance <= hitDistance)
            {
                t_unit.OnDamage(dmg);
                Destroy(gameObject);
            }
        }
        else if (t_unit == null)
        {
            dir = t_pos - m_pos;
            dir.y = 0f;
            dir = dir.normalized;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.LookRotation(dir);

            float distance = Vector3.Distance(transform.position, t_pos);
            if (distance <= hitDistance)
            {
                t_unit.OnDamage(dmg);
                Destroy(gameObject);
            }
        }
    }
    public void Init(Unit target, float dmg)
    {
        t_unit = target;
        this.dmg = dmg;
    }
}
