using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{

    private Vector3 mousePos;
    private Rigidbody2D rb;
    public attackInfo attack;
    public HashSet<string> projTargetTags = new HashSet<string>();

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
        attack.damage += attackBoost.damage;
        attack.stunDuration += attackBoost.stunDuration;
        attack.blindDuration += attackBoost.blindDuration;
        attack.poisonDuration += attackBoost.poisonDuration;
        attack.poisonDamage += attackBoost.poisonDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (projTargetTags.Contains(other.tag))
        {
            other.GetComponent<Combat>().ReceiveAttack(attack);
            Destroy(gameObject);
        }
    }
}
