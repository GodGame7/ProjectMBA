using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    Unit m_unit;
    [SerializeField] Unit t_unit;
    [SerializeField] float dmg;
    float duration = 1f;
    float speed = 10f;
    float hitDistance = 1f;
    Vector3 t_pos;
    Vector3 m_pos;
    Vector3 dir;
    bool ishitted = false;
    [SerializeField] float distance;
    void Update()
    {
        if (t_unit != null)
        {
            t_pos = t_unit.transform.position;
            m_pos = transform.position;
            m_pos.y = t_unit.transform.position.y;
            dir = t_pos - m_pos;
            dir = dir.normalized;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);

            distance = Vector3.Distance(m_pos, t_pos);
            if (distance <= hitDistance)
            {
                if (!ishitted) { ishitted = true; t_unit.OnDamage(m_unit, dmg); t_unit.OnStun(m_unit, duration); }
                Destroy(gameObject);
            }
        }
        else if (t_unit == null)
        {
            dir = t_pos - m_pos;
            dir.y = 0f;
            dir = dir.normalized;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);

            distance = Vector3.Distance(transform.position, t_pos);
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
        this.dmg = skillinfo.dmgs[skillinfo.level-1];
        duration = skillinfo.ccDurationTime;
    }
}
