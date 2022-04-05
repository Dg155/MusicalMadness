using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector3 newpos;
    public Transform pos;
    public Transform playerpos; // must be set in scene view
    public bool moving;
    public int roomsize;
    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform;
        moving = false;
        roomsize = 8;
        newpos = pos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving){
            if (playerpos.position.x > pos.position.x + roomsize/2){
                newpos = newpos + Vector3.right * roomsize;
                moving = true;
            }
            else if (playerpos.position.x < pos.position.x - roomsize/2){
                newpos = newpos + Vector3.left * roomsize;
                moving = true;
            }
            else if (playerpos.position.y > pos.position.y + roomsize/2){
                newpos = newpos + Vector3.up * roomsize;
                moving = true;
            }
            else if (playerpos.position.y < pos.position.y - roomsize/2){
                newpos = newpos + Vector3.down * roomsize;
                moving = true;
            }
        }
        else{
            if (Vector3.Distance(pos.position, newpos) < 0.3){
                moving = false;
            }
            else{
                pos.position = Vector3.Lerp(pos.position, newpos, 1.60F * Time.deltaTime);
            }
        }
    }
}
