using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [SerializeField] private float currHealth = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private GameObject mainHand;
    [SerializeField] private GameObject offHand;
    [SerializeField] protected GameObject healthBar;
    [SerializeField] protected float offSet;
    protected bool facingRight;
    private HealthBarScript HB;
    protected void Start()
    {
        GameObject health = Instantiate(healthBar, (transform.position -  new Vector3(0,offSet,0)), Quaternion.identity);
        health.transform.parent = this.transform;
        HB = GetComponentInChildren<HealthBarScript>();
    }

    private void Update() {
        Render();
    }

    protected virtual void Die(){
    }

    protected virtual void Render(){
    }
    public float getHealth(){
        return currHealth;
    }

    public float getMaxHealth(){
        return maxHealth;
    }
    public void setHealth(float quantity){
        currHealth = quantity;
        if (quantity > maxHealth){maxHealth = quantity;}
        updateHealthBar();
    }
    public void addHealth(float quantity){
        //can be positive or negative value
        currHealth += quantity;
        if (currHealth > maxHealth){currHealth = maxHealth;}
        if (currHealth <= 0){Die();}
        updateHealthBar();
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

    public GameObject getMainHand(){
        return mainHand;
    }

    public void setMainHand(GameObject instrument){
        mainHand = instrument;
    }

    public GameObject getoffHand(){
        return offHand;
    }

    public void setOffHand(GameObject instrument){
        offHand = instrument;
    }

    protected void flip()
    {
        //This entire render stuff should be moved to a separate script later
        if (facingRight)
        {
            this.transform.localScale = new Vector3(-1,1,1);
            facingRight = false;
        }
        else
        {
            this.transform.localScale = new Vector3(1,1,1);
            facingRight = true;
        }
    }

    public void updateHealthBar()
    {
        float barSize = currHealth / maxHealth;
        HB.setHealthBar(barSize);
    }

}
