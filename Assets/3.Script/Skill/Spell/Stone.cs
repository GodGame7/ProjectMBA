using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    Unit m_unit;
    Unit t_unit;
    float dmg;
    float duration = 1f;
    float speed = 10f;
    float hitDistance = 1f;
    Vector3 t_pos;
    Vector3 m_pos;
    Vector3 dir;
    bool ishitted = false;

    void Update()
    {
        if (t_unit != null)
        {
            t_pos = t_unit.transform.position;
            m_pos = transform.position;
            dir = t_pos - m_pos;
            dir.y = 0f;
            dir = dir.normalized;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.LookRotation(dir);

            float distance = Vector3.Distance(transform.position, t_pos);
            if (distance <= hitDistance)
            {
                if (!ishitted) { ishitted = true; t_unit.OnDamage(m_unit, dmg); t_unit.OnStun(m_unit, duration); }
                Destroy(gameObject, 1f);
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
                Destroy(gameObject, 1f);
            }
        }
    }
    public void Init(Unit user, Unit target, Skill skillinfo)
    {
        m_unit = user;
        t_unit = target;
        this.dmg = skillinfo.dmgs[skillinfo.level];
        duration = skillinfo.ccDurationTime;
    }
}
