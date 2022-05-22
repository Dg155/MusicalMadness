using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{

    private Vector3 mousePos;
    private Rigidbody2D rb;
    public AudioClip soundEffect;
    public GameObject wallCollision;
    public attackInfo attack;
    public HashSet<string> projTargetTags = new HashSet<string>();

    public void Awake(){
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
        attack.animCol = attackBoost.animCol;
        attack.screenShakeDeg = attackBoost.screenShakeDeg;
        attack.screenShakeTime = attackBoost.screenShakeTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            if (attack.animCol != null){
                Instantiate(attack.animCol, this.transform.position, Quaternion.identity);
            }
            else if (wallCollision != null){
                Instantiate(wallCollision, this.transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else if (projTargetTags.Contains(other.tag))
        {
            if (other.GetComponent<EntVisAudFX>() != null){
                other.GetComponent<EntVisAudFX>().CollisionEffect(this.transform.position);
            }
            else{
                Debug.Log("COULD NOT FIND COLLISION SCRIPT");
            }
            if (attack.animCol != null){
                Instantiate(attack.animCol, this.transform.position, Quaternion.identity);
            }
            if (attack.screenShakeDeg >= 0f){
                Debug.Log("ALOHA");
                FindObjectOfType<CameraMove>().Shake(attack.screenShakeTime, attack.screenShakeDeg);//shakes camera
            }
            other.GetComponent<Combat>().ReceiveAttack(attack);

            Destroy(gameObject);
        }
    }
}
