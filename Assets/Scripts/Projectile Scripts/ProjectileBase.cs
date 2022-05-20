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

    public virtual void setCourseOfFire(int bulletSpeed, bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {
        // Determine velocity, direction, and targets of projectile
    }

    public void setAttackInfo(attackInfo newAttack)
    {
        attack = newAttack;
    }

    public void boostAttack(attackInfo attackBoost)
    {
        attack.targetNewDrag = attackBoost.targetNewDrag;

        attack.damage += attackBoost.damage;
        attack.stunDuration += attackBoost.stunDuration;
        attack.blindDuration += attackBoost.blindDuration;
        attack.poisonDuration += attackBoost.poisonDuration;
        attack.poisonDamage += attackBoost.poisonDamage;
        attack.knockback += attackBoost.knockback;
        attack.blastRadius += attackBoost.blastRadius;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (attack.blastRadius > 0)
        {
            var colliders = Physics2D.OverlapCircleAll(rb.position, attack.blastRadius);
            attack.attackerPos = rb.position; //update the attacker's position to the place where the projectile impacted, not where it was shot from
            foreach (Collider2D c in colliders)
            {
                if (projTargetTags.Contains(c.tag))
                {
                    c.GetComponent<Combat>().ReceiveAttack(attack);
                }
            }
        }
        else if (projTargetTags.Contains(other.tag))
        {
            other.GetComponent<Combat>().ReceiveAttack(attack);
        }

        if (other.tag == "Wall" || other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}