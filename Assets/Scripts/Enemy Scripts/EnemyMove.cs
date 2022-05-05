using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    public void MoveToward(Vector2 targetPos, float speed){
        transform.position = Vector2.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
    }
}
