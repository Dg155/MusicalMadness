using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    float distance;
    Vector2 startPos;
    float currDistance;
    public void MoveToward(Vector2 targetPos, float speed, ref ScriptableEnemyAI ai){
        //Previous code is in green
        //transform.position = Vector2.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
        currDistance += speed * Time.deltaTime;
        float BeforeValue = currDistance/distance;
        float InterpolationValue = ai.animCurve.Evaluate(BeforeValue); //uses curve function to alter
        transform.position = Vector2.Lerp(startPos, targetPos, InterpolationValue);
    }

    public void SetPositions(Vector2 startPosition, Vector2 targetPos){
        //sets start and end position of movement, as well as Distance
        distance = Vector2.Distance(startPosition, targetPos);
        startPos = startPosition;
        currDistance = 0;
    }
}
