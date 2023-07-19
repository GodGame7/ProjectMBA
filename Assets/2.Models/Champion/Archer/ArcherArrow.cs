using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrow : MonoBehaviour
{
    Unit m_unit;
    Unit t_unit;
    float dmg = 1f;
    float speed = 20f;
    float hitDistance = 1f;
    Vector3 t_pos;
    Vector3 m_pos;
    Vector3 dir;
    bool ishitted = false;
    private void Update()
    {
        
        if (t_unit != null)
        {
            t_pos = t_unit.transform.position;
            m_pos = transform.position;
            m_pos.y = t_pos.y;
            dir = t_pos - m_pos;
            dir = dir.normalized;
            transform.Translate(dir * speed * Time.deltaTime, Space.World) ;
            transform.rotation = Quaternion.LookRotation(dir);

            float distance = Vector3.Distance(m_pos, t_pos);
            if (distance <= hitDistance)
            {
                if (!ishitted) { ishitted = true; t_unit.OnDamage(m_unit, dmg); }
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
               this.GetComponentInChildren<TrailRenderer>().enabled = false;
               Destroy(gameObject);
            }
        }
    }
    public void Init(Unit user, Unit target, float dmg)
    {
        m_unit = user;
        t_unit = target;
        this.dmg = dmg;
    }
}
