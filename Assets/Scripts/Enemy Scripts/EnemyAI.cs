using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AIState {
    Roaming, Aggressive, Retreating, Shooting
}
public class EnemyAI : MonoBehaviour
{
    public ScriptableEnemyAI ai; //AI Object that stores info 
    EnemyMove Movement; //movement script that moves character

    Combat Combat; //combat script that deals with combat

    AIState state; //current state: Roaming, Aggressive, Retreating, Shooting

    public Transform target; //when detecting a player, target is that player

    public Vector2 movePos; //Next Position to move to

    public pos boundedPos; //Coordinates that the enemy is bounded to

    void Start()
    {
        Movement = this.GetComponent<EnemyMove>();
        Combat = this.GetComponent<Combat>();
        state = AIState.Roaming;
        movePos = this.transform.position;
        boundedPos = new pos(0,0);
        
    }
    void Update() //temporary, delete later once called in EnemyManager script
    {
        OnUpdate();
    }
    void OnUpdate()
    {
        switch (state){

            case AIState.Roaming:
                //Check if at the new movePos, if so ask ai to calculate a new roaming position
                if (Vector2.Distance(movePos, this.transform.position) < 0.01){
                    movePos = ai.NewRoamPos(this.transform.position, new pos(0,0), 8);
                }
                //If not at new movePos, move toward that position
                else{
                    Movement.MoveToward(movePos, ai.roamSpeed);
                }
                //If player is nearby, set either to Aggressive or Retreating(currently defaults to aggressive)
                target = ai.GetTarget(this.transform.position);
                if (target){ //if there is a detectable target, become aggresive
                    state = AIState.Aggressive;
                }
                break;
            case AIState.Aggressive:
                //Check if at the new movePos, if so ask ai to calculate 
                //a new aggresive roaming position based on enemy location
                //movePos = ai.NewAggressivePos(this.transform.position, target.position);
                if (Vector2.Distance(movePos, this.transform.position) < 0.01){
                    movePos = ai.NewAggressivePos(this.transform.position, target.position);
                }
                //if not at new movePos, move toward that position
                else{
                    Movement.MoveToward(movePos, ai.aggressiveSpeed);
                }
                //Check if conditions are correct to shoot, if so, calculate an aiming position and shoot

                

                //Check if conditions are correct to return to Roaming
                target = ai.GetTarget(this.transform.position);
                if (!target){ //if there is not a detectable target, roam
                    state = AIState.Roaming;
                }
                break;
            case AIState.Shooting:
                break;
            case AIState.Retreating:
                break;
        }
        
    }
}
