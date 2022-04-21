using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerStats : BaseStats
{
    public int souls; 

    override protected void Die(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    protected override void Render()
    {
        //This entire render stuff should be moved to a separate script later
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x - this.transform.position.x > 0 && !facingRight || mousePos.x - this.transform.position.x < 0 && facingRight)
        {
            flip();
        }
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
    
}
