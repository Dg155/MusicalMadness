using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : BaseStats
{
    public int minSoulsDropped;
    public int maxSoulsDropped;
    public GameObject soul;
    public float heartHealPercentage = 10;
    public int heartDropPercentage = 10;
    public GameObject heart;

    public GameObject deathParticle;

    public bool isBoss;


    override protected void Start(){
        GameObject health = Instantiate(healthBar, (transform.position -  new Vector3(0,offSet,0)), Quaternion.identity);
        health.transform.parent = this.transform;
        HB = GetComponentInChildren<HealthBarScript>();
    }
    override protected void Die(){
        if (isBoss) { GetComponent<E_GrandPianoAI>().setAlive(false); }
        else { GetComponent<EnemyAI>().setAlive(false); }
        GetComponent<EntVisAudFX>().DeathEffect();
    }

    public void destroyEnemy(){
        int soulsDropped = Random.Range(minSoulsDropped, maxSoulsDropped);
        for(int i =0; i < soulsDropped; i++)
        {
            float offSet = Random.Range(-0.2f, 0.2f);
            Instantiate(soul, transform.position + new Vector3(offSet, offSet), Quaternion.identity);
        }
        if (Random.Range(0, 100) < heartDropPercentage)
        {
            float heartHealAmount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().getMaxHealth() * (heartHealPercentage / 100);
            Debug.Log(heartHealAmount);
            heart.GetComponent<ItemObject>().item.setItemWorth(heartHealAmount);
            float offSet = Random.Range(-0.2f, 0.2f);
            Instantiate(heart, transform.position + new Vector3(offSet, offSet), Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
