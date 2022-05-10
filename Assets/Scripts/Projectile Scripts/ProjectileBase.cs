using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{

    private Vector3 mousePos;
    private Rigidbody2D rb;
    public attackInfo attack;

    public virtual void setCourseOfFire(int bulletSpeed, bool facingRight, Vector3 shootPos)
    {
        // Determine velocity and direction of projectile
    }

    public void setAttackInfo(attackInfo newAttack)
    {
        attack = newAttack;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy")
        {
            other.GetComponent<Combat>().ReceiveAttack(attack);
            Destroy(gameObject);
        }
    }
}
