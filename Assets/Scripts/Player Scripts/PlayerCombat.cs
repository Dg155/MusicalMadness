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
    public void UseOnHand(){
        Weapon mainHand = playerStats.getMainHand().GetComponent<Weapon>();
        mainHand.StartCoroutine("Use");
    }

    public void UseOffHand(){
        playerStats.getoffHand().GetComponent<Weapon>().Use();
    } 

}
