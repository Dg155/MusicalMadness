using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerStats playerStats;
    void Start()
    {
        playerStats = this.GetComponent<PlayerStats>();
    }

    void TakeDamage(float quantity){
        playerStats.addHealth(-quantity);
    }

    void Heal(float quantity){
        playerStats.addHealth(quantity);
    }
    void UseOnHand(){
        Weapon onHandWeapon = playerStats.getEquipts().mainHand;
        //do something witht he mainHand type
    }

    void UseOffHand(){
        Weapon offHandWeapon = playerStats.getEquipts().offHand;
        //do something witht he offHand type
    }

}
