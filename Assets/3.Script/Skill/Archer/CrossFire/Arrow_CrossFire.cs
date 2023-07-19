using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_CrossFire : MonoBehaviour
{
    Unit myUnit;
    Vector3 t_pos;
    bool shoot = false;
    bool explode = false;
    float dmg;
    RaycastHit[] hitted;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject bullet;
    private void Update()
    {
        if (shoot)
        {
            transform.position = Vector3.Lerp(transform.position, t_pos, 5f * Time.deltaTime);
            float distance = Vector3.Distance(transform.position, t_pos);
            if (distance <= 0.05f)
            {
                Explode();
            }
        }
    }
    public void Shoot()
    {
        shoot = true;
    }
    public void Init(Unit user, Vector3 t_pos, float dmg)
    {
        myUnit = user;
        this.t_pos = t_pos;
        this.dmg = dmg;
    }
    void Explode()
    {
        if (!explode)
        {
            explode = true;
            explosion.Play();
            bullet.SetActive(false);
            //�ҷ� ���� ����
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (Collider col in colliders)
            {                
                Unit unit;
                if (col.TryGetComponent(out unit))
                {
                    Debug.Log(unit.champName);
                    if (unit.GetTeam() != myUnit.GetTeam())
                    {
                        // ������ �������� �ִ� �Լ� ȣ�� (�� ��ũ��Ʈ�� �ش� �Լ��� �����ؾ� ��)
                        unit.OnDamage(myUnit, dmg);
                    }
                }
            }
            Destroy(this.gameObject, 1f);
        }
    }
}
