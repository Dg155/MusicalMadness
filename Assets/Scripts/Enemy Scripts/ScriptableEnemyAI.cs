using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Enemy AI written by Hung Bui
[CreateAssetMenu(fileName = "EnemyAI", menuName = "ScriptableObjects/EnemyAI")]
public class ScriptableEnemyAI : ScriptableObject
{
    public float offsetSize = 2.5f;
    //DETECTION/SENSES
    [Header("Senses/Targets")]
    public string targetTag; //tag of element to select as target
    public float detectionRadius; //radius of detection of target


    //ROAMING (choosing where to move)
    [Header("RoamingState Info")]
    public AnimationCurve roamingCurve;
    public float roamingDelay; //delay before moving
    public float roamingSpeed; //speed during roaming



    //AGGRESSIVE (choosing where to move)
    [Header("AggressiveState info")]
    public AnimationCurve aggressiveCurve;
    public float aggressiveDelay; //wait time before moving again
    public float aggressiveSpeed; //speed while aggressive
    public float aggressiveDistanceFromTarget; //used to calculate the preferred distance from target
    public float aggressiveStrafeFactor; //used to calculate the degree of strafing

    [Header("Retreating Info")]
    public AnimationCurve retreatCurve;
    public float retreatHealthPercent; //float 0 - 1 indicating what proportion of health before mode should switch to flee
    public float retreatDelay; //wait time before moving again
    public float retreatSpeed; //speed while aggressive
    public float retreatDistanceFromTarget; //used to calculate the preferred distance from target
    public float retreatStrafeFactor; //used to calculate the degree of strafing

    [Header("ShootingState Info")]
    //SHOOTING (choosing where to shoot, when to shoot(conditionally))
    public float aimOffsetFactor; //used to calculate how aim is offsetted(0 is perfect aim)
    public float shootDistanceMin, shootDistanceMax;


    void FitIntoBoundary(ref Vector2 newPos, Vector2 roomCenter, float offset){
        //helper ffunction to bound enemies from walking to other rooms
        if (newPos.x < roomCenter.x - offset){
            newPos.x = roomCenter.x - offset;
        }
        else if (newPos.x > roomCenter.x + offset){
            newPos.x = roomCenter.x + offset;
        }
        if (newPos.y > roomCenter.y + offset){
            newPos.y = roomCenter.y + offset;
        }
        else if (newPos.y < roomCenter.y - offset){
            newPos.y = roomCenter.y - offset;
        }
    }

    //ALL METHODS BELOW CALLED FROM THE EnemyAI.cs script!
    public Vector2 NewRoamPos(Vector2 currPos, Vector2 roomCenter){ 
        //Calculates new roaming position
        Vector2 offset = new Vector2(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f));
        Vector2 newPos = offset + currPos;
        FitIntoBoundary(ref newPos, roomCenter, offsetSize); //hardcoded as 2.5 for now
        return newPos;
    }

    public Vector2 NewAggressivePos(Vector2 currPos, Vector2 targetLocation, Vector2 roomCenter){ 
        //Calculates new aggressive position
        Vector2 towardVector = (targetLocation - currPos).normalized;
        Vector2 perpendicularVector = Vector2.Perpendicular(towardVector).normalized;
        Vector2 newPos = targetLocation - towardVector * aggressiveDistanceFromTarget + perpendicularVector * Random.Range(-aggressiveStrafeFactor, aggressiveStrafeFactor);
        FitIntoBoundary(ref newPos, roomCenter, offsetSize);
        return newPos;
    }
    public Vector2 NewRetreatPos(Vector2 currPos, Vector2 targetLocation, Vector2 roomCenter){ 
        //Calculates new retreat position
        Vector2 towardVector = (targetLocation - currPos).normalized;
        Vector2 perpendicularVector = Vector2.Perpendicular(towardVector).normalized;
        Vector2 newPos = targetLocation - towardVector * retreatDistanceFromTarget + perpendicularVector * Random.Range(-retreatStrafeFactor, retreatStrafeFactor);
        FitIntoBoundary(ref newPos, roomCenter, offsetSize);
        return newPos;
    }

    public Transform GetTarget(Vector2 currPos){
        var colliders = Physics2D.OverlapCircleAll(currPos, detectionRadius);
        foreach(Collider2D c in colliders){
            if (c.tag == targetTag){
                return c.transform;
            }
        }
        return null;
    }

    public Vector2 Aim(Vector2 currPos, Vector2 targetLocation){
        //Calculates Aim position for the next attack
        Vector2 perpendicularVector = Vector2.Perpendicular((targetLocation - currPos).normalized);
        return targetLocation + perpendicularVector * Random.Range(-aimOffsetFactor, aimOffsetFactor);
    }

    public bool isReadyToShoot(Vector2 currPos, Transform enemy){
        //UNFINISHED/NEEDS TO BE DISCUSSED. Current placeholder calculates if distance is within a threshhold, returning true if is
        if (shootDistanceMin <= Vector2.Distance(currPos, enemy.position) 
            && 
            Vector2.Distance(currPos, enemy.position) <= shootDistanceMax){
            return true;
        }
        return false;
    }

    public bool shouldFlee(float healthPercent){
        if (healthPercent < retreatHealthPercent){
            return true;
        }
        return false;
    }
}
