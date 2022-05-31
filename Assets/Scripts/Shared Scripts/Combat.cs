using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum entityType{
    player, monster
}

[System.Serializable]
public struct attackInfo{ //stores all info of the weapon when it collides an enemy

    //attack stat vars (except for knockback)
    public float damage;
    public float stunDuration;
    public float blindDuration;
    public float poisonDuration;
    public float poisonDamage;

    //visual vars
    public float screenShakeDeg;
    public float screenShakeTime;
    public GameObject animCol; //The animation upon collision

    //knockback & explosion vars
    public Vector3 attackerPos;
    public float knockback;
    public float blastRadius;
    public float targetNewDrag; //how slidy we want the target to be once they receive knockback

    public bool isPiercing;

    //allows us to add attackInfo instances
    public static attackInfo operator+ (attackInfo a, attackInfo b)
    {
        attackInfo c = new attackInfo();
        c.damage = a.damage + b.damage;
        c.stunDuration = a.stunDuration + b.stunDuration;
        c.blindDuration = a.blindDuration + b.blindDuration;
        c.poisonDuration = a.poisonDuration + b.poisonDuration;
        c.poisonDamage = a.poisonDamage + b.poisonDamage;

        c.screenShakeDeg = a.screenShakeDeg + b.screenShakeDeg;
        c.screenShakeTime = a.screenShakeTime + b.screenShakeTime;

        c.knockback = a.knockback + b.knockback;
        c.blastRadius = a.blastRadius + b.blastRadius;

        if (c.knockback > 0) { c.stunDuration += .01f; } //knockback ALWAYS needs stun >0

        return c;
    }
}

public class Combat : MonoBehaviour
{
    Weapon mainHand;
    Weapon offHand;
    BaseStats stats;
    Rigidbody2D rb;

    HashSet<string> targetTags = new HashSet<string>();

    entityType type;

    public bool isStunned; //false by default
    public bool isVulnerableToPierce; //true by default; determines whether an enemy can be hit by a piercing attack (so it doesn't take tons of damage instantly)
    public float pierceInvulTime; //how long someone is invulnerable to piercing attacks after being hit by one (should be the same for all enemies & player)

    void Start()
    {
        stats = this.GetComponent<BaseStats>();
        rb = this.GetComponent<Rigidbody2D>();

        isVulnerableToPierce = true;

        if (this.tag == "Enemy")
        {
            targetTags.Add("Player");
        }
        else
        {
            targetTags.Add("Enemy");
        }
    }
    public BaseStats getStats(){
        if (stats == null){Debug.Log("Player stats is missing!");}
        return stats;
    }

    public HashSet<string> getTargetTags()
    {
        return targetTags;
    }

    public void ReceiveAttack(attackInfo attack){

        EntVisAudFX FX = this.GetComponent<EntVisAudFX>();
        if (FX != null){
            FX.Flash();
        }

        if (attack.isPiercing == true){
            StartCoroutine(receivePierce(pierceInvulTime));
        }
        TakeDamage(attack.damage);
        if (attack.stunDuration > 0){StartCoroutine(receiveStun(attack.stunDuration));}
        if (attack.blindDuration > 0){receiveBlind(attack.blindDuration);}
        if (attack.poisonDamage > 0){receivePoison(attack.poisonDuration, attack.poisonDamage);}
        if (attack.knockback > 0){receiveKnockback(attack.attackerPos, attack.knockback, attack.targetNewDrag);}
    }
    void TakeDamage(float quantity){
        stats.addHealth(-quantity);
    }

    public IEnumerator receivePierce(float sec){
        isVulnerableToPierce = true;
        yield return new WaitForSeconds(sec);
        isVulnerableToPierce = false;
    }

    public bool getIsStunned()
    {
        return isStunned;
    }

    public IEnumerator receiveStun(float sec){
        isStunned = true;
        yield return new WaitForSeconds(sec);
        isStunned = false;
    }

    public void receiveBlind(float sec){
        //unfinished(creates blind effect/tunnel vision)
    }

    public void receivePoison(float sec, float damage){
        //unfinished(receives poison which tickets every second)
    }

    public void receiveKnockback(Vector3 attackerPos, float knockback, float drag){
        Vector3 kbVec = Vector3.Normalize(this.transform.position - attackerPos) * knockback;
        rb.velocity = Vector3.zero;
        rb.drag = drag;
        rb.AddForce(kbVec, ForceMode2D.Impulse);
    }

    public void Heal(float quantity){
        stats.addHealth(quantity);
    }
    public void UseMainHand(Vector3 shootPos)
    {
        if (mainHand is null)
        {
            setMainHand(); //move this later to only call when switching/changing weapons
        }
        else{
            StartCoroutine(mainHand.Use(shootPos, targetTags));
        }
    }

    public void UseMainHandSecondary(Vector3 shootPos)
    {
        if (mainHand is null)
        {
            setMainHand(); //move this later to only call when switching/changing weapons
        }
        else{
            StartCoroutine(mainHand.UseSecondary(shootPos, targetTags));
        }
    }

    public void UseOffHand(Vector3 shootPos)
    {
        if (offHand is null)
        {
            setOffHand();
        }
        else{
            offHand.Use(shootPos, targetTags); //if I want to give the same weapon a secondary attack, all I have to do is change this line to mainHand.SecondaryAttack(shootPos, targetTags)
        } 
    } 

    public void setMainHand(){
        mainHand = stats.getMainHand().GetComponent<Weapon>(); //change later to pass a weapon argument, and call this only when switching weapons
    }

    public void setOffHand(){
        offHand = stats.getoffHand().GetComponent<Weapon>(); //same here
    }
}
