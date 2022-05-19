using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{

    private Vector3 mousePos;
    private Rigidbody2D rb;
    public AudioClip soundEffect;
    public attackInfo attack;
    public HashSet<string> projTargetTags = new HashSet<string>();

    private void Awake(){
        //CHANGE LATER TO WORK WITH EVENTS
        FindObjectOfType<SoundEffectPlayer>().PlaySound(soundEffect);

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
        attack.damage += attackBoost.damage;
        attack.stunDuration += attackBoost.stunDuration;
        attack.blindDuration += attackBoost.blindDuration;
        attack.poisonDuration += attackBoost.poisonDuration;
        attack.poisonDamage += attackBoost.poisonDamage;
        if (attackBoost.animCol != null){
            attack.animCol = attackBoost.animCol;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
            if (attack.animCol){
                Instantiate(attack.animCol, this.transform.position, Quaternion.identity);
            }
        }
        else if (projTargetTags.Contains(other.tag))
        {
            if (other.GetComponent<EntVisAudFX>() != null){
                other.GetComponent<EntVisAudFX>().CollisionEffect(this.transform.position);
            }
            else{
                Debug.Log("COULD NOT FIND COLLISOIN SCRIPT");
            }
            if (attack.animCol){
                Instantiate(attack.animCol, this.transform.position, Quaternion.identity);
            }
            FindObjectOfType<CameraMove>().Shake(0.25f, 0.012f);//shakes camera
            other.GetComponent<Combat>().ReceiveAttack(attack);

            Destroy(gameObject);
        }
    }
}
