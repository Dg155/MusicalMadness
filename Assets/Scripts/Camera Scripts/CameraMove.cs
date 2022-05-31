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
    bool shaking = false;
    public LevelInfo levelInfo;
    public bool inMaze = true;
    private Vector3 offset;

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform;
        moving = false;
        roomsize = 8;
        newpos = pos.position;
        rb = this.GetComponent<Rigidbody2D>();
        offset = transform.position - playerpos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (inMaze)
        {
            if (shaking){
            return;
            }
            if (!moving){
                if (playerpos.position.x > pos.position.x + roomsize/2){
                    newpos = newpos + Vector3.right * roomsize;
                    levelInfo.changePos(1, 0);
                    moving = true;
                }
                else if (playerpos.position.x < pos.position.x - roomsize/2){
                    newpos = newpos + Vector3.left * roomsize;
                    levelInfo.changePos(-1, 0);
                    moving = true;
                }
                else if (playerpos.position.y > pos.position.y + roomsize/2){
                    newpos = newpos + Vector3.up * roomsize;
                    levelInfo.changePos(0, 1);
                    moving = true;
                }
                else if (playerpos.position.y < pos.position.y - roomsize/2){
                    newpos = newpos + Vector3.down * roomsize;
                    levelInfo.changePos(0, -1);
                    moving = true;
                }
            }
            else{
                if (Vector3.Distance(pos.position, newpos) < 0.1){
                    moving = false;
                }
                else{
                    pos.position = Vector3.Lerp(pos.position, newpos, 4.5F * Time.deltaTime);
                }
            }
        }
        else {transform.position = playerpos.position + offset;}
    }

    public void Shake(float duration, float magnitude){
        if (inMaze) {StartCoroutine(shakeCamera(duration, magnitude));}
    }
    IEnumerator shakeCamera(float duration, float magnitude){
        shaking = true;
        Vector3 originalPosition = this.transform.position;
        float timeElapsed = 0f;
        while (timeElapsed < duration){
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            rb.MovePosition(this.transform.position + new Vector3(x, y, 0));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        this.transform.position = originalPosition;
        shaking = false;
    }
}
