using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GrandPianoAIPhase
{
    RegularAttack, Defense, SpiralAttack
}

enum GrandPianoAIState
{
    Aggressive, Shooting
}

public class E_GrandPianoAI : MonoBehaviour
{
    public ScriptableEnemyAI ai; //AI Object that stores info 
    EnemyMove MovementScript; //movement script that moves character

    Combat CombatScript; //combat script that deals with combat

    [SerializeField] GrandPianoAIState state; //current state: Roaming, Aggressive, Retreating, Shooting

    [SerializeField] GrandPianoAIPhase phaseState;

    public Transform target; //when detecting a player, target is that player

    public Vector2 movePos; //Next Position to move to

    public float timeSinceLastAction; //time since the last action(may separate later)

    private Vector2 roomCenter;

    private bool isAlive = true;

    private bool facingRight = true;

    private Rigidbody2D rb;

    private Animator animator;

    [SerializeField] private float phaseTimer = 10f;
    private float savedTime;
    private float angle = 0f;
    private BossManager manager;
    private bool minionSpawned;
    private EnemyViolinWeapon weaponReference;
    private float oldAttackSpeed;
    private float oldProjectileSpeed;
    [SerializeField] private float spiralAttackSpeed;
    [SerializeField] private float spiralProjectileSpeed;
    private float healthPercent;

    void Start()
    {
        MovementScript = this.GetComponent<EnemyMove>();
        CombatScript = this.GetComponent<Combat>();
        state = GrandPianoAIState.Aggressive;
        movePos = this.transform.position;
        timeSinceLastAction = 0;
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        savedTime = phaseTimer;
        manager = GameObject.Find("Boss Manager").GetComponent<BossManager>(); //Replace later this is really bad method
        weaponReference = this.transform.GetComponentInChildren<EnemyViolinWeapon>(); //Replace with Grand Piano's weapons reference
        oldAttackSpeed = weaponReference.coolDownPrimary;
        oldProjectileSpeed = weaponReference.bulletSpeed;
    }
    /*
    void Update() //temporary, delete later once called in EnemyManager script
    {
        OnUpdate(new pos(0,0)); //DELETE LATER(call OnUpdate through the EnemyManager Script!)
    }
    */
    void OnCollisionEnter2D(Collision2D col)
    { //for now, any collisions will trigger a movement reset
        movePos = this.transform.position;//movement reset
    }

    public void OnUpdate(pos roomPos)
    {
        Transform HB = GetComponentInChildren<HealthBarScript>().transform;
        if (target != null)
        {
            if (!facingRight && target.transform.position.x - this.transform.position.x > 0)
            {
                facingRight = true;
                this.transform.localScale = new Vector2(1, 1);
                HB.localScale = new Vector2(.72f, .6f);
            }
            else if (facingRight && target.transform.position.x - this.transform.position.x < 0)
            {
                facingRight = false;
                this.transform.localScale = new Vector2(-1, 1);
                HB.localScale = new Vector2(-.72f, .6f);
            }
        }
        else if (!facingRight && (movePos.x - this.transform.position.x > 0
        || (target != null && target.transform.position.x - this.transform.position.x > 0)))
        {
            facingRight = true;
            this.transform.localScale = new Vector2(1, 1);
            HB.localScale = new Vector2(.72f, .6f);
        }
        else if (facingRight && movePos.x - this.transform.position.x < 0
        || (target != null && target.transform.position.x - this.transform.position.x < 0))
        {
            facingRight = false;
            this.transform.localScale = new Vector2(-1, 1);
            HB.localScale = new Vector2(-.72f, .6f);
        }




        if (roomCenter.x != roomPos.x * 8 || roomCenter.y != roomPos.y * 8)
        {
            roomCenter = new Vector2(roomPos.x * 8, roomPos.y * 8);
        }
        //Debug.Log("ROOM CENTER IS: " + roomCenter.x.ToString() +  "," + roomCenter.y.ToString());

        switch (phaseState)
        {
            case GrandPianoAIPhase.RegularAttack:
                CombatScript.isDefensiveBoss = false;
                weaponReference.coolDownPrimary = oldAttackSpeed;
                weaponReference.bulletSpeed = oldProjectileSpeed;

                switch (state)
                {
                    case GrandPianoAIState.Aggressive:
                        //Ask AI to calculate new move position
                        if (target == null)
                        {
                            setState();
                            break;
                        }

                        timeSinceLastAction += Time.deltaTime;
            
                        if (Vector2.Distance(movePos, this.transform.position) < 0.01)
                        {
                            healthPercent = CombatScript.getStats().getHealth() / CombatScript.getStats().getMaxHealth();
                            animator.SetFloat("Speed", 1);
                            if (timeSinceLastAction > ai.aggressiveDelay)
                            {
                                if (ai.shouldFlee(healthPercent)){
                                    movePos = ai.NewRetreatPos(this.transform.position, target.position, roomCenter);
                                }
                                else{
                                    movePos = ai.NewAggressivePos(this.transform.position, target.position, roomCenter);
                                }
                                MovementScript.SetPositions(this.transform.position, movePos);
                                timeSinceLastAction = 0;
                                animator.SetFloat("Speed", 1);
                            }
                        }
                        //if not at new movePos, move toward that position
                        else
                        {
                            healthPercent = CombatScript.getStats().getHealth() / CombatScript.getStats().getMaxHealth();
                            if (ai.shouldFlee(healthPercent)){
                                MovementScript.MoveToward(movePos, ai.retreatSpeed, ref ai, ai.retreatCurve);
                            }
                            MovementScript.MoveToward(movePos, ai.aggressiveSpeed, ref ai, ai.aggressiveCurve);
                        }
                        //Check if conditions are correct to shoot, if so, calculate an aiming position and shoot
                        if (ai.isReadyToShoot(this.transform.position, target))
                        {
                            state = GrandPianoAIState.Shooting;
                            return;
                        }
                        setState();
                        break;

                    case GrandPianoAIState.Shooting:
                        //shoot, then return to previous state
                        //Debug.Log("PRESSING SHOOT BUTTON");
                        if (target == null)
                        {
                            setState();
                            break;
                        }
                        CombatScript.UseMainHand(ai.Aim(this.transform.position, target.position));
                        //ADD: once weapon usage is figured out, implement it here. Should pass the ai position into the weapon use
                        setState();
                        break;
                }

                if (checkPhase())
                {
                    phaseState = GrandPianoAIPhase.Defense;
                }
                break;

            case GrandPianoAIPhase.Defense:
                //Hold current position
                //All damage during this time is reduced by 40%(?)
                //If health is less than 50%, summon enemies
                
                animator.SetBool("Defensive", true);
                CombatScript.isDefensiveBoss = true;

                //Summon enemies
                healthPercent = CombatScript.getStats().getHealth() / CombatScript.getStats().getMaxHealth();

                if (healthPercent <= 0.5)
                {
                    //find ref to manager, and then call instantiateMinions function from there
                    if (!minionSpawned)
                    {
                        manager.InstantiateMinions();
                        minionSpawned = true;
                    }
                }



                if (checkPhase())
                {
                    animator.SetBool("Defensive", false);
                    phaseState = GrandPianoAIPhase.SpiralAttack;
                }
                break;

            case GrandPianoAIPhase.SpiralAttack:
                //Hold current position
                //Fire in spiral pattern
                animator.SetBool("Spiral", true);
                CombatScript.isDefensiveBoss = false;
                minionSpawned = false;

                weaponReference.coolDownPrimary = spiralAttackSpeed;
                weaponReference.bulletSpeed = spiralProjectileSpeed;

                float dirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                float dirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                Vector2 firePos = new Vector2(dirX, dirY);

                CombatScript.UseMainHand(ai.Aim(this.transform.position, firePos));

                angle += 3f;

                if (checkPhase())
                {
                    animator.SetBool("Spiral", false);
                    phaseState = GrandPianoAIPhase.RegularAttack;
                }
                break;
        }

    }

    bool checkPhase()
    {
        //if timer < timelimit, return false
        //if timer > timelimit, return true
        if (phaseTimer > 0)
        {
            phaseTimer -= Time.deltaTime;
            Debug.Log(phaseState);
            return false;
        }
        else
        {
            phaseTimer = savedTime;
            return true;
        }
    }

    void setState()
    {
        /*if (CombatScript.getIsStunned())
        {
            state = AIState.Stunned;
            return;
        }

        if (target == null)
        {
            target = ai.GetTarget(this.transform.position);
        }
        if (target)
        { //if there is a detectable target, consider either to aggress or retreat
            float healthPercent = CombatScript.getStats().getHealth() / CombatScript.getStats().getMaxHealth();
            //Debug.Log("HEALTH PERCENT IS " + healthPercent.ToString());
            if (ai.shouldFlee(healthPercent))
            {
                //Debug.Log("SWITCHING TO RETREAT STATE");
                state = AIState.Retreating;
            }
            else
            {
                //Debug.Log("SWITCHING TO AGGRESSIVE STATE");
                state = AIState.Aggressive;
            }
        }
        else
        {
            //Debug.Log("SWITCHING TO ROAMING STATE");
            state = AIState.Roaming;
        }*/

        if (target == null)
        {
            target = ai.GetTarget(this.transform.position);
        }
        if (target)
        {
            
            state = GrandPianoAIState.Aggressive;
        }
    }

    public void setAlive(bool liveStatus)
    {
        isAlive = liveStatus;
    }

    public bool getAlive()
    {
        return isAlive;
    }

}
