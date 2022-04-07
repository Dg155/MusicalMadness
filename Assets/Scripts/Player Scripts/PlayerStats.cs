using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Weapon{
    piano, drum, guitar
}

public enum Clothing{
    ironVest, speedBoots
}

public struct Equipts{
    //Keep this struct
    public Weapon mainHand;
    public Weapon offHand;
    public Clothing boots;
    public Clothing pants;
    public Clothing shirt;
    public Clothing accessory;
}


public class PlayerStats : MonoBehaviour
{
    //PRIVATE THESE LATER
    public float currHealth = 100;
    public float maxHealth = 100;
    public float moveSpeed = 4;
    public Equipts equipment;
    public int souls; // currency
    void Start()
    {
        equipment = new Equipts();
    }

    void Die(){
        //do something
    }
    public float getHealth(){
        return currHealth;
    }
    public void setHealth(float quantity){
        currHealth = quantity;
        if (quantity > maxHealth){maxHealth = quantity;}
    }
    public void addHealth(float quantity){
        //can be positive or negative value
        currHealth += quantity;
        if (currHealth > maxHealth){currHealth = maxHealth;}
        if (currHealth < 0){Die();}
    }

    public float getMoveSpeed(){
        return moveSpeed;
    }
    public void setMoveSpeed(float quantity){
        moveSpeed = quantity;
    }
    public void addMoveSpeed(float quantity){
        moveSpeed += quantity;
        if (moveSpeed < 0){moveSpeed = 0;}
    }

    public int getSouls(){
        return souls;
    }
    public void setSouls(int quantity){
        souls = quantity;
    }
    public void addSouls(int quantity){
        souls += quantity;
        if (souls < 0){souls = 0;}
    }

    public Equipts getEquipts(){
        return equipment;
    }




    
}
