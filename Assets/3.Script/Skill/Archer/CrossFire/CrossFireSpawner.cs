using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFireSpawner : MonoBehaviour
{
    public GameObject cfPrefabs;
    Vector3 t_pos;
    float area;
    Unit myUnit;
    float dmg;
    int count;
    Vector3 spawnPoint;
    Vector3 shootPoint;
    Arrow_CrossFire[] cfs;
    void SpawnBullet()
    {
        IEnumerator c = Spawn_co();
        StartCoroutine(c);
        IEnumerator Spawn_co()
        {
            for (int i = 0; i < count; i++)
            {
                GameObject j = Instantiate(cfPrefabs);                
                spawnPoint = (transform.position - transform.forward) + Random.insideUnitSphere * area;
                spawnPoint.y = transform.position.y + 3f;
                j.transform.position = spawnPoint;
                shootPoint.x = Random.Range(t_pos.x - area, t_pos.x + area);
                shootPoint.z = Random.Range(t_pos.z - area, t_pos.z + area);
                shootPoint.y = t_pos.y;
                j.GetComponent<Arrow_CrossFire>().Init(myUnit, shootPoint, dmg);
                cfs[i] = j.GetComponent<Arrow_CrossFire>();
                yield return new WaitForSeconds(0.7f / count);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < cfs.Length; i++)
            {
                cfs[i].Shoot();
                yield return new WaitForSeconds(0.3f / count);
            }
            Destroy(this.gameObject);
        }
    }
    public void Init(Unit user, Vector3 t_pos, float dmg, int count, float area)
    {
        myUnit = user;
        this.t_pos = t_pos;
        this.dmg = dmg;
        this.count = count;
        this.area = area;
        cfs = new Arrow_CrossFire[count];
        SpawnBullet();
    }
}
