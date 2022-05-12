using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum entityType{
    player, monster
}

[System.Serializable]
public struct attackInfo{ //stores all info of the weapon when it collides an enemy:
//the damage, the length of stun, the length of blinding, the poison damage, the knockback
    public float damage;
    public float stunDuration;
    public float blindDuration;
    public float poisonDuration;
    public float poisonDamage;
}

public class Combat : MonoBehaviour
{
    Weapon mainHand;
    Weapon offHand;
    BaseStats stats;

    HashSet<string> targetTags = new HashSet<string>();

    entityType type;
    void Start()
    {
        stats = this.GetComponent<BaseStats>();

        if (this.tag == "Enemy")
        {
            targetTags.Add("Player");
        }
        else
        {
            targetTags.Add("Enemy");
        }
    }
    public BaseStats getStats(){
        if (stats == null){Debug.Log("Player stats is missing!");}
        return stats;
    }

    public HashSet<string> getTargetTags()
    {
        return targetTags;
    }

    public void ReceiveAttack(attackInfo attack){
        TakeDamage(attack.damage);
        if (attack.stunDuration > 0){receiveStun(attack.stunDuration);}
        if (attack.blindDuration > 0){receiveBlind(attack.blindDuration);}
        if (attack.poisonDamage > 0){receivePoison(attack.poisonDuration, attack.poisonDamage);}
    }
    void TakeDamage(float quantity){
        stats.addHealth(-quantity);
    }

    public void receiveStun(float sec){
        //unfinished(disables input)
    }

    public void receiveBlind(float sec){
        //unfinished(creates blind effect/tunnel vision)
    }

    public void receivePoison(float sec, float damage){
        //unfinished(receives poison which tickets every second)
    }

    public void Heal(float quantity){
        stats.addHealth(quantity);
    }
    public void UseMainHand(Vector3 shootPos)
    {
        if (mainHand is null)
        {
            setMainHand(); //move this later to only call when switching/changing weapons
        }
        StartCoroutine(mainHand.Use(shootPos, targetTags));
    }

    public void UseOffHand(Vector3 shootPos)
    {
        if (offHand is null)
        {
            setOffHand();
        }
        offHand.Use(shootPos, targetTags);
    } 

    public void setMainHand(){
        mainHand = stats.getMainHand().GetComponent<Weapon>(); //change later to pass a weapon argument, and call this only when switching weapons
    }

    public void setOffHand(){
        offHand = stats.getoffHand().GetComponent<Weapon>(); //same here
    }
}
