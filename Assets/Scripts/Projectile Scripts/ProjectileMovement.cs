using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //If projectile is fired by player, find position of mouse click and set constant
        //velocity for projectile in that direction
        //If fired by enemy, set constant velocity to direction of player with margin of error
    }

    private void OnTriggerEnter2D(Collider2D other) {

        /*
        if other.tag == enemy
        {
            call enemy damage method
            destroy(gameObejct);
        }
        if other.tag == wall
        {
            reverse velocity to bounce off wall
            reverberation?
        }
        if other.tag == player
        {
            call player damage method
            destory(gameObject);
        }
        */
    }
}
