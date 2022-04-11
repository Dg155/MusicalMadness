using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Weapon mainHand;
    Weapon offHand;
    PlayerStats playerStats;
    void Start()
    {
        playerStats = this.GetComponent<PlayerStats>();
    }

    void ReceiveAttack(attackInfo attack){
        TakeDamage(attack.damage);
        if (attack.stunDuration > 0){receiveStun(attack.stunDuration);}
        if (attack.blindDuration > 0){receiveBlind(attack.blindDuration);}
        if (attack.poisonDamage > 0){receivePoison(attack.poisonDuration, attack.poisonDamage);}
    }
    void TakeDamage(float quantity){
        playerStats.addHealth(-quantity);
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
        playerStats.addHealth(quantity);
    }
    public void UseMainHand(){
        if (mainHand is null){
            setMainHand(); //move this later to only call when switching/changing weapons
        }
        mainHand.StartCoroutine("Use");
    }

    public void UseOffHand(){
        if (offHand is null){
            setOffHand();
        }
        offHand.Use();
    } 
    




    public void setMainHand(){
        mainHand = playerStats.getMainHand().GetComponent<Weapon>(); //change later to pass a weapon argument, and call this only when switching weapons
    }

    public void setOffHand(){
        offHand = playerStats.getoffHand().GetComponent<Weapon>(); //same here
    }
}
