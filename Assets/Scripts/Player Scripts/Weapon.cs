using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public enum weaponMove
{
    trumpetPrimary, trumpetSecondary, drumPrimary, drumSecondary
}

public class Weapon : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    protected bool primaryRanged;
    protected bool secondaryRanged;
    public float coolDownPrimary;
    public float coolDownSecondary;
    public bool canFire = true;
    public attackInfo attack;
    protected attackInfo baseAttack;
    public attackInfo secondaryAttack;
    protected attackInfo secondaryBaseAttack;

    // weapon render variables
    Combat CombatScript;
    Transform playerTransform;
    bool facingRight;
    bool pointingAtPlayer;
    Vector3 targetPos;

    protected weaponMove primaryMove;
    protected weaponMove secondaryMove;
    public List<weaponMove> LastMovesUsed = new List<weaponMove>(); //change this to protected after testing is over
    protected int maxComboLength;
    protected bool comboTimerIsActive;
    protected float comboLossTime; // the exact point in time that the combo will be lost
    protected float comboLossTimeLimit; // max amt of time btwn player's last attack & combo being lost

    public delegate void OnWeaponMove(List<weaponMove> lastMovesUsed); //Creates an event that updates UI w/ combo moves
    public OnWeaponMove onWeaponMoveCallback;
    
    public delegate void OnComboActivated(bool comboSuccessful);
    public OnComboActivated onComboActivatedCallback;

    public ParticleSystem leftParticleSystem;
    public ParticleSystem rightParticleSystem;
    public AudioClip soundEffectL;
    public AudioClip soundEffectR;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseAttack = attack;
        secondaryBaseAttack = secondaryAttack;
        facingRight = true;  // the sprite by default is facing right
        CombatScript = transform.GetComponentInParent<Combat>();
        comboTimerIsActive = false;
        if (CombatScript.getTargetTags().Contains("Enemy")) // i.e. if the Combat Script belongs to the Player
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

    public weaponMove getPrimaryWeaponMove()
    {
        return primaryMove;
    }

    public weaponMove getSecondaryWeaponMove()
    {
        return secondaryMove;
    }

    protected void AddMoveToCombo(weaponMove newMove)
    {
        LastMovesUsed.Add(newMove);
        if (LastMovesUsed.Count > maxComboLength)
        {
            LastMovesUsed.RemoveAt(0);
        }
        if (onWeaponMoveCallback != null)
        {
            onWeaponMoveCallback.Invoke(LastMovesUsed);
        }
    }

    protected virtual attackInfo CalculateComboDamage()
    {
        return attack;
    }

    protected void ClearLastMoves(bool comboSuccessful = false) //shortens the code
    {
        LastMovesUsed.Clear();
        if (onWeaponMoveCallback != null) //If there are any methods subscribed to the event,
        {
            onComboActivatedCallback.Invoke(comboSuccessful);
        }
    }

    protected virtual async void StartComboTimer() // async instead of coroutine is used here bc a single async can be endlessly extended & only clear LastMovesUsed once after a single completion. Using coroutines forces us to constantly create new instances of coroutines which then end all at once, causing LastMovesUsed to be constantly emptied if the player continuously fires
    {
        comboTimerIsActive = true;
        while (Time.time < comboLossTime)
        {
            await Task.Yield();
        }
        //Debug.Log("Combo lost");
        ClearLastMoves();
        comboTimerIsActive = false;
    }

    virtual public IEnumerator Use(Vector3 shootPos, HashSet<string> targetTags)
    {
        if (canFire){
            canFire = false;
            if (leftParticleSystem != null){
                leftParticleSystem.Play();
            }
            AddMoveToCombo(primaryMove);
            comboLossTime = Time.time + comboLossTimeLimit; // reset the combo loss time limit
            if (!comboTimerIsActive) { StartComboTimer(); } // i.e. if the async StartComboTimer() isn't already active, start it

            if (primaryRanged)
            {
                spawnProjectile(facingRight, shootPos, targetTags);
            }
            else
            {
                meleeAttack(facingRight, shootPos, targetTags);
            }
            if (animator != null) { animator.SetBool("Fire", true);}
            yield return new WaitForSeconds(coolDownPrimary);
            if (animator != null) {animator.SetBool("Fire", false);}
            canFire = true;
        }
    }

    virtual public IEnumerator UseSecondary(Vector3 shootPos, HashSet<string> targetTags)
    {
        if (canFire){
            canFire = false;
            if (rightParticleSystem != null){
                rightParticleSystem.Play();
            }
            AddMoveToCombo(secondaryMove);
            comboLossTime = Time.time + comboLossTimeLimit; // reset the combo loss time limit
            if (!comboTimerIsActive) { StartComboTimer(); } // i.e. if the async StartComboTimer() isn't already active, start it

            if (secondaryRanged)
            {
                spawnProjectileSecondary(facingRight, shootPos, targetTags);
            }
            else
            {
                meleeAttackSecondary(facingRight, shootPos, targetTags);
            }
            if (animator != null) { animator.SetBool("Fire", true); }
            yield return new WaitForSeconds(coolDownSecondary);
            if (animator != null) { animator.SetBool("Fire", false); }
            canFire = true;
        }
    }

    public virtual void spawnProjectile(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {

    }

    public virtual void spawnProjectileSecondary(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {

    }

    public virtual void boostMelee(attackInfo attackBoost)
    {
        attack += attackBoost;
        attack.targetNewDrag = attackBoost.targetNewDrag;
        attack.animCol = attackBoost.animCol;
    }

    public virtual void boostMeleeSecondary(attackInfo attackBoost)
    {
        secondaryAttack += attackBoost;
        secondaryAttack.targetNewDrag = attackBoost.targetNewDrag;
        secondaryAttack.animCol = attackBoost.animCol;
    }

    public virtual void resetMelee()
    {
        attack = baseAttack;
    }

    public virtual void resetMeleeSecondary()
    {
        secondaryAttack = secondaryBaseAttack;
    }

    public virtual void meleeAttack(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {

    }

    public virtual void meleeAttackSecondary(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {

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
