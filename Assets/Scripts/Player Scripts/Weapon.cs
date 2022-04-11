using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Weapon : MonoBehaviour
{
    Rigidbody2D rb;
    protected bool ranged;
    private float coolDown = 0.5f;
    private bool canFire = true;
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Render();
    }

    virtual public IEnumerator Use()
    {
        if (canFire){
            canFire = false;
            if (ranged){
                spawnProjectile();
            }
            else{
                meleeAttack();
            }
            yield return new WaitForSeconds(coolDown);
            canFire = true;
        }
    }

    public virtual void spawnProjectile(){

    }

    public virtual void meleeAttack(){

    }

    protected virtual void Render()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
