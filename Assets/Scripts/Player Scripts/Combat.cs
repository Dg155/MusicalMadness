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

    entityType type;
    void Start()
    {
        stats = this.GetComponent<BaseStats>();
    }
    public BaseStats getStats(){
        if (stats == null){Debug.Log("Player stats is missing!");}
        return stats;
    }
    void ReceiveAttack(attackInfo attack){
        TakeDamage(attack.damage);
        if (attack.stunDuration > 0){receiveStun(attack.stunDuration);}
        if (attack.blindDuration > 0){receiveBlind(attack.blindDuration);}
        if (attack.poisonDamage > 0){receivePoison(attack.poisonDuration, attack.poisonDamage);}
    }
    void TakeDamage(float quantity){
        stats.addHealth(-quantity);
    }

    void receiveStun(float sec){
        //unfinished(disables input)
    }

    void receiveBlind(float sec){
        //unfinished(creates blind effect/tunnel vision)
    }

    void receivePoison(float sec, float damage){
        //unfinished(receives poison which tickets every second)
    }


    void Heal(float quantity){
        stats.addHealth(quantity);
    }
    public void UseMainHand()
    {
        if (mainHand is null)
        {
            setMainHand(); //move this later to only call when switching/changing weapons
        }
        mainHand.StartCoroutine("Use");
    }

    public void UseOffHand()
    {
        if (offHand is null)
        {
            setOffHand();
        }
        offHand.Use();
    } 

    public void setMainHand(){
        mainHand = stats.getMainHand().GetComponent<Weapon>(); //change later to pass a weapon argument, and call this only when switching weapons
    }

    public void setOffHand(){
        offHand = stats.getoffHand().GetComponent<Weapon>(); //same here
    }
}
