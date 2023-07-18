using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Hiryu : MonoBehaviour
{
    Unit myUnit;
    float range;
    float dmg;
    float speed;
    Vector3 startPos;
    List<GameObject> hitTargets = new List<GameObject>();
    [SerializeField] GameObject hitParticle;

    private void Update()
    {
        if (myUnit != null)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            float distance = Vector3.Distance(startPos, transform.position);
            if (distance >= range)
            {
                hitTargets.Clear();
                gameObject.SetActive(false);
                Destroy(gameObject, 2f);
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
                ParticlePlay(other.transform.position);
                enemy.OnDamage(myUnit, dmg);
            }
        }
    }


    void ParticlePlay(Vector3 position)
    {
        IEnumerator p = Particle();
        if(p != null) StartCoroutine(p);
        IEnumerator Particle()
        {
            GameObject a = Instantiate(hitParticle, position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            Destroy(a, 0.1f);
        }
    }
    
    public void Init(Unit user, float range, float dmg, float speed)
    {
        myUnit = user;
        this.range = range;
        this.dmg = dmg;
        this.speed = speed;
        startPos = myUnit.transform.position;
    }
}
