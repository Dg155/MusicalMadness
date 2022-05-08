using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI", menuName = "ScriptableObjects/EnemyAI")]
public class ScriptableEnemyAI : ScriptableObject
{
    //MOVEMENT CURVES
    [Header("MOVEMENT CURVES(One for now)")]
    public AnimationCurve animCurve;
    //ROAMING (choosing where to move)
    [Header("ROAMING STATE INFO")]
    public float roamSpeed; //speed during roaming



    //AGGRESSIVE (choosing where to move)
    [Header("AGGRESSIVE STATE INFO")]
    public float aggressiveWait; //wait time before moving again
    public float aggressiveSpeed; //speed while aggressive
    public float aggressiveDistanceFromTarget; //used to calculate the preferred distance from target
    public float aggressiveStrafeFactor; //used to calculate the degree of strafing


    [Header("SHOOTING STATE INFO")]
    //SHOOTING (choosing where to shoot, when to shoot(conditionally))
    public float aimOffsetFactor; //used to calculate how aim is offsetted(0 is perfect aim)


    //DETECTION/SENSES
    [Header("SENSES")]
    public string targetTag; //tag of element to select as target
    public float detectionRadius; //radius of detection of target

    [Header("Will add in retreating later")]
    public float fleeHealthPercent; //float 0 - 1 indicating what proportion of health before mode should switch to flee


    public Vector2 NewRoamPos(Vector2 currPos, pos cellPos, int cellsize){ 
        //UNFINISHED: Calculate a new roam position
        float offsetSize = cellsize/2 - 1;
        Debug.Log(offsetSize);
        Vector2 offset = new Vector2(Random.Range(-1,1), Random.Range(-1,1));
        //add boundary-checking (changing x and y to fit into the boundaries of the room)
        return offset; // + currPosition
    }
    public Vector2 NewAggressivePos(Vector2 currPos, Vector2 targetLocation){ 

        Vector2 towardVector = (targetLocation - currPos).normalized;
        Vector2 perpendicularVector = Vector2.Perpendicular(towardVector).normalized;
        //again, add some way to boundary-check
        return targetLocation - towardVector * aggressiveDistanceFromTarget + perpendicularVector * Random.Range(-aggressiveStrafeFactor, aggressiveStrafeFactor);
    }
    public Transform GetTarget(Vector2 currPos){//UNFINISHED: Detect nearby targets(must have collider)
        var colliders = Physics2D.OverlapCircleAll(currPos, detectionRadius);
        foreach(Collider2D c in colliders){
            if (c.tag == targetTag){
                return c.transform;
            }
        }
        return null;
    }

    public Vector2 Aim(Vector2 currPos, Vector2 targetLocation){
        //used to calculate the aim position for the next attack
        Vector2 perpendicularVector = Vector2.Perpendicular((targetLocation - currPos).normalized);
        return targetLocation + perpendicularVector * Random.Range(-aimOffsetFactor, aimOffsetFactor);
    }

    public bool isReadyToShoot(Vector2 currPos, Transform enemy){
        //UNFINISHED/NEEDS TO BE DISCUSSED. Current placeholder calculates if distance is within a threshhold, returning true if is
        if (Vector2.Distance(currPos, enemy.position) < aggressiveDistanceFromTarget + 4){
            return true;
        }
        return false;
    }

    public bool shouldFlee(float healthPercent){
        if (healthPercent < fleeHealthPercent){
            return true;
        }
        return false;
    }
}
