using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : BaseStats
{
    public int minSoulsDropped;
    public int maxSoulsDropped;
    public GameObject soul;

    override protected void Die(){
        GetComponent<EnemyAI>().setAlive(false);
    }

    public void destroyEnemy(){
        int soulsDropped = Random.Range(minSoulsDropped, maxSoulsDropped);
        for(int i =0; i < soulsDropped; i++)
        {
            float offSet = Random.Range(-0.2f, 0.2f);
            Instantiate(soul, transform.position + new Vector3(offSet, offSet), Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
