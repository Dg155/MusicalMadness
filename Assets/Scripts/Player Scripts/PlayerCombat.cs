using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    //Dictionary with weapons as the keys and shootmethods as the values
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
        //Check if left mouse button is clicked, get mainHand weapon, index into dictionary and call the value
        // the value will be the specific shoot method, which will spawn projectiles 
        Weapon onHandWeapon = playerStats.getEquipts().mainHand;
        //do something witht he mainHand type
    }

    void UseOffHand(){
        Weapon offHandWeapon = playerStats.getEquipts().offHand;
        //do something witht he offHand type
    } 

}
