using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{

    protected Vector3 mousePos;
    protected Rigidbody2D rb;

    public AudioClip soundEffect;
    public GameObject wallCollision;

    public attackInfo attack;
    public HashSet<string> projTargetTags = new HashSet<string>();


    protected virtual void Awake(){
        //CHANGE LATER TO WORK WITH EVENTS
        if (soundEffect != null){
            FindObjectOfType<SoundEffectPlayer>().PlaySound(soundEffect);
        }
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
        attack.animCol = attackBoost.animCol;
        attack.isPiercing = attackBoost.isPiercing;
    }

    public virtual void OnTriggerStay2D(Collider2D other){
        if (!attack.isPiercing){return;}
        if (other.tag == "Wall"){
            foreach(var x in Physics2D.OverlapCircleAll(this.transform.position, 0.1f)){
                if (x.tag == "Wall"){
                    Destroy(gameObject);
                }
            }
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //Check if projectile hit something that destroys the projectile
        //Check if wall
        if (other.tag == "Wall")
        {
            if (attack.animCol != null){
                Instantiate(attack.animCol, this.transform.position, Quaternion.identity);
            }
            else if (wallCollision != null){
                Instantiate(wallCollision, this.transform.position, Quaternion.identity);
            }
            if (attack.screenShakeDeg >= 0f){
                FindObjectOfType<CameraMove>().Shake(attack.screenShakeTime, attack.screenShakeDeg);//shakes camera
            }

            if (attack.blastRadius > 0)
            {
                attack.attackerPos = rb.position;
                var colliders = Physics2D.OverlapCircleAll(rb.position, attack.blastRadius);
                foreach (Collider2D c in colliders)
                {
                    if (projTargetTags.Contains(c.tag))
                    {
                        c.GetComponent<Combat>().ReceiveAttack(attack);
                    }
                }
            }
            if (!attack.isPiercing){
                Destroy(gameObject);
            }
        }
        //Check if hit an enemy
        else if (projTargetTags.Contains(other.tag))
        {
            //Debug.Log("Path 2");
            if (other.GetComponent<EntVisAudFX>() != null){
                other.GetComponent<EntVisAudFX>().CollisionEffect(this.transform.position);
            }
            else{
                Debug.Log("COULD NOT FIND COLLISION SCRIPT");
            }
            if (attack.animCol != null){
                Instantiate(attack.animCol, this.transform.position, Quaternion.identity);
            }
            print(attack.screenShakeDeg);
            if (attack.screenShakeDeg >= 0f){
                FindObjectOfType<CameraMove>().Shake(attack.screenShakeTime, attack.screenShakeDeg);//shakes camera
            }

            //Do damage to the thing it hit if it's an opponent
            //Apply AOE if there is a blast radius
            if (attack.blastRadius > 0)
            {
                attack.attackerPos = rb.position; //attackerPos is set upon impact to set the attack's position to the place where the projectile impacted, not where it was shot from
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
                attack.attackerPos = rb.position;
                other.GetComponent<Combat>().ReceiveAttack(attack);
            }

            if (!attack.isPiercing)
            {
                Destroy(gameObject);
            }
        }
    }
}