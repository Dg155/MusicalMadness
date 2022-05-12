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

    public bool facingRight; //the sprite by default is facing right
    Combat CombatScript;
    bool pointingAtPlayer;
    Transform playerTransform;
    Vector3 targetPos;
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        facingRight = true;
        CombatScript = transform.GetComponentInParent<Combat>();
        if (CombatScript.getTargetTags().Contains("Enemy")) //i.e. if the Combat Script belongs to the Player
        {
            pointingAtPlayer = false;
        }
        else
        {
            pointingAtPlayer = true;
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
    }
    void Update()
    {
        Render();
    }

    virtual public IEnumerator Use(Vector3 shootPos, HashSet<string> targetTags)
    {
        if (canFire){
            canFire = false;
            if (ranged){
                spawnProjectile(facingRight, shootPos, targetTags);
            }
            else{
                meleeAttack(targetTags);
            }
            animator.SetBool("Fire", true);
            yield return new WaitForSeconds(coolDown);
            animator.SetBool("Fire", false);
            canFire = true;
        }
    }

    public virtual void spawnProjectile(bool facingRight, Vector3 shootPos, HashSet<string> targetTags){

    }

    public virtual void meleeAttack(HashSet<string> targetTags){

    }

    protected virtual void Render()
    {
        if (pointingAtPlayer)
        {
            targetPos = playerTransform.position;
        }
        else
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        Vector3 direction = targetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(targetPos.x - this.transform.parent.position.x > 0 && !facingRight || targetPos.x - this.transform.parent.position.x < 0 && facingRight)
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
