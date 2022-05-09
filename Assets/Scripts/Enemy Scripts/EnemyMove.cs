using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    float distance;
    Vector2 startPos;
    float currDistance;
    void Start(){
        rb = this.GetComponent<Rigidbody2D>();
    }
    public void MoveToward(Vector2 targetPos, float speed, ref ScriptableEnemyAI ai, AnimationCurve curve){
        //Previous code is in green
        //transform.position = Vector2.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
        currDistance += speed * Time.deltaTime;
        float BeforeValue = currDistance/distance;
        float InterpolationValue = curve.Evaluate(BeforeValue); //uses curve function to alter
        //transform.position = Vector2.Lerp(startPos, targetPos, InterpolationValue);
        rb.MovePosition(Vector2.Lerp(startPos, targetPos, InterpolationValue));
    }

    public void SetPositions(Vector2 startPosition, Vector2 targetPos){
        //sets start and end position of movement, as well as Distance
        distance = Vector2.Distance(startPosition, targetPos);
        startPos = startPosition;
        currDistance = 0;
    }
}
