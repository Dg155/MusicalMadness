using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{

    private Vector3 mousePos;
    private Rigidbody2D rb;
    public attackInfo attack;
    public HashSet<string> projTargetTags = new HashSet<string>();

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    public virtual void setCourseOfFire(float bulletSpeed, bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {
        // Determine velocity, direction, and targets of projectile
    }

    public void setAttackInfo(attackInfo newAttack)
    {
        attack = newAttack;
    }

    public void boostAttack(attackInfo attackBoost)
    {
        attack += attackBoost;
        attack.targetNewDrag = attackBoost.targetNewDrag;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //Check if projectile hit something that destroys the projectile
        if (projTargetTags.Contains(other.tag) || other.tag == "Wall")
        {
            //Do damage to the thing it hit if it's an opponent
            attack.attackerPos = rb.position; //attackerPos is set upon impact to set the attack's position to the place where the projectile impacted, not where it was shot from
            if (attack.blastRadius > 0)
            {
                var colliders = Physics2D.OverlapCircleAll(rb.position, attack.blastRadius);
                foreach (Collider2D c in colliders)
                {
                    if (projTargetTags.Contains(c.tag))
                    {
                        c.GetComponent<Combat>().ReceiveAttack(attack);
                    }
                }
            }
            else
            {
                if (projTargetTags.Contains(other.tag))
                {
                    other.GetComponent<Combat>().ReceiveAttack(attack);
                }
            }

            Destroy(gameObject);
        }
    }
}