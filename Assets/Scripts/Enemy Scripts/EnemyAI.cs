using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy AI written by Hung Bui
enum AIState {
    Roaming, Aggressive, Retreating, Shooting, Stunned, Knocked
}
public class EnemyAI : MonoBehaviour
{
    public ScriptableEnemyAI ai; //AI Object that stores info 
    EnemyMove MovementScript; //movement script that moves character

    Combat CombatScript; //combat script that deals with combat

    [SerializeField] AIState state; //current state: Roaming, Aggressive, Retreating, Shooting

    public Transform target; //when detecting a player, target is that player

    public Vector2 movePos; //Next Position to move to

    public float timeSinceLastAction; //time since the last action(may separate later)

    private Vector2 roomCenter;

    private bool isAlive = true;

    private bool facingRight = true;

    private Rigidbody2D rb;

    void Start()
    {
        MovementScript = this.GetComponent<EnemyMove>();
        CombatScript = this.GetComponent<Combat>();
        state = AIState.Roaming;
        movePos = this.transform.position;
        timeSinceLastAction = 0;
        rb = this.GetComponent<Rigidbody2D>();
        
    }
    /*
    void Update() //temporary, delete later once called in EnemyManager script
    {
        OnUpdate(new pos(0,0)); //DELETE LATER(call OnUpdate through the EnemyManager Script!)
    }
    */
    void OnCollisionEnter2D(Collision2D col){ //for now, any collisions will trigger a movement reset
        movePos = this.transform.position;//movement reset
    }
    
    public void OnUpdate(pos roomPos)
    {
        if (target != null){
            if (!facingRight && target.transform.position.x - this.transform.position.x > 0){
                facingRight = true;
                this.transform.localScale = new Vector2(1, 1);
            }
            else if (facingRight && target.transform.position.x - this.transform.position.x < 0)
            {
                facingRight = false;
                this.transform.localScale = new Vector2(-1, 1);
            }
        }
        else if (!facingRight && (movePos.x - this.transform.position.x > 0
        || (target !=null &&  target.transform.position.x - this.transform.position.x > 0)))
        {
            facingRight = true;
            this.transform.localScale = new Vector2(1, 1);
        }
        else if (facingRight && movePos.x - this.transform.position.x < 0
        || (target !=null &&  target.transform.position.x - this.transform.position.x < 0))
        {
            facingRight = false;
            this.transform.localScale = new Vector2(-1, 1);
        }




        if (roomCenter.x != roomPos.x * 8 || roomCenter.y != roomPos.y * 8){
            roomCenter = new Vector2(roomPos.x * 8, roomPos.y * 8);
        }
        //Debug.Log("ROOM CENTER IS: " + roomCenter.x.ToString() +  "," + roomCenter.y.ToString());
        switch (state){

            case AIState.Stunned:
                setState(); //check if is still stunned
                if (state != AIState.Stunned){
                    state = AIState.Knocked;
                }
                break;

            case AIState.Knocked:
                //In this state, no movement occurs per frame
                //When rb velocity is zero, we can set the state again
                if(Vector2.Distance(Vector2.zero, this.rb.velocity) < 0.1){
                        movePos = this.transform.position;
                        setState();
                        break;
                    }
                //knocked enemies can still use weapons however
                target = ai.GetTarget(this.transform.position);
                if (target != null){
                    CombatScript.UseMainHand(ai.Aim(this.transform.position, target.position));
                }
                break;
        


            case AIState.Roaming:
                //Ask AI to calculate new move position
                if (Vector2.Distance(movePos, this.transform.position) < 0.01){
                    movePos = ai.NewRoamPos(this.transform.position, roomCenter);
                    MovementScript.SetPositions(this.transform.position, movePos);
                }
                //If not at new movePos, move toward that position
                else{
                    MovementScript.MoveToward(movePos, ai.roamingSpeed, ref ai, ai.roamingCurve);
                }
                setState();
                break;


            case AIState.Aggressive:
                //Ask AI to calculate new move position
                if (target == null){
                    setState();
                    break;
                }
                timeSinceLastAction += Time.deltaTime;
                if (Vector2.Distance(movePos, this.transform.position) < 0.01){
                    if (timeSinceLastAction > ai.aggressiveDelay){
                        movePos = ai.NewAggressivePos(this.transform.position, target.position, roomCenter);
                        MovementScript.SetPositions(this.transform.position, movePos);
                        timeSinceLastAction = 0;
                    }
                }
                //if not at new movePos, move toward that position
                else{
                    MovementScript.MoveToward(movePos, ai.aggressiveSpeed, ref ai, ai.aggressiveCurve);
                }
                //Check if conditions are correct to shoot, if so, calculate an aiming position and shoot
                if (ai.isReadyToShoot(this.transform.position, target)){
                    state = AIState.Shooting;
                    return;
                }
                setState();
                break;

            case AIState.Shooting:
                //shoot, then return to previous state
                //Debug.Log("PRESSING SHOOT BUTTON");
                if (target == null){
                    setState();
                    break;
                }
                CombatScript.UseMainHand(ai.Aim(this.transform.position, target.position));
                //ADD: once weapon usage is figured out, implement it here. Should pass the ai position into the weapon use
                setState();
                break;

            case AIState.Retreating:
                timeSinceLastAction += Time.deltaTime;
                if (target == null){
                    setState();
                }
                if (Vector2.Distance(movePos, this.transform.position) < 0.5){
                    if (timeSinceLastAction > ai.retreatDelay){
                        movePos = ai.NewRetreatPos(this.transform.position, target.position, roomCenter);
                        MovementScript.SetPositions(this.transform.position, movePos);
                        timeSinceLastAction = 0;
                    }
                }
                //if not at new movePos, move toward that position
                else{
                    MovementScript.MoveToward(movePos, ai.retreatSpeed, ref ai, ai.retreatCurve);
                }
                //Check if conditions are correct to shoot, if so, calculate an aiming position and shoot
                setState(); //normally, will stay retreating unless somehow heals
                break;

        }
        
    }

    void setState(){
        //in spite of everything above, the enemy may still be stunned
        if (CombatScript.getIsStunned()){
            state = AIState.Stunned;
            return;
        }
        
        if (target == null){
            target = ai.GetTarget(this.transform.position);
        }
        if (target){ //if there is a detectable target, consider either to aggress or retreat
            float healthPercent = CombatScript.getStats().getHealth()/CombatScript.getStats().getMaxHealth();
            //Debug.Log("HEALTH PERCENT IS " + healthPercent.ToString());
            if (ai.shouldFlee(healthPercent)){
                //Debug.Log("SWITCHING TO RETREAT STATE");
                state = AIState.Retreating;
            }
            else{
                //Debug.Log("SWITCHING TO AGGRESSIVE STATE");
                state = AIState.Aggressive;
                }
        }
        else{
            //Debug.Log("SWITCHING TO ROAMING STATE");
            state = AIState.Roaming;
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
