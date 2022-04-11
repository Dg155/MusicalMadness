using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerStats : BaseStats
{
    public int souls; // currency

    override protected void Die(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    protected override void Render()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x - this.transform.position.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
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
