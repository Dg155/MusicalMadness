using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Weapon : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    protected bool ranged;
    public float coolDown = 0.5f;
    private bool canFire = true;
    protected attackInfo attack;

    private bool facingRight;
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("WeaponHold");
        this.transform.position = player.transform.position;
        this.transform.parent = player.transform;
    }

    void Update()
    {
        Render();
    }

    virtual public IEnumerator Use(Vector3 shootPos)
    {
        if (canFire){
            canFire = false;
            if (ranged){
                spawnProjectile(facingRight, shootPos);
            }
            else{
                meleeAttack();
            }
            animator.SetBool("Fire", true);
            yield return new WaitForSeconds(coolDown);
            animator.SetBool("Fire", false);
            canFire = true;
        }
    }

    public virtual void spawnProjectile(bool facingRight, Vector3 shootPos){

    }

    public virtual void meleeAttack(){

    }

    protected virtual void Render()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(mousePosition.x - this.transform.parent.position.x > 0 && !facingRight || mousePosition.x - this.transform.parent.position.x < 0 && facingRight)
        {
            flip();
        }
    }

    private void flip(){
        //This entire render stuff should be moved to a separate script later
        if (facingRight){
            this.transform.localScale = new Vector3(-1,-1,1);
            facingRight = false;
        }
        else{
            this.transform.localScale = new Vector3(1,1,1);
            facingRight = true;
        }
    }
}
