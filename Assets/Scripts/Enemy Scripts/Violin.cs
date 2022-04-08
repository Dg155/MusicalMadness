using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Violin : MonoBehaviour
{
    private int health = 100;
    private float speed = 4f;
    private bool lockedOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float walkingTime = Random.RandomRange(1f, 3f);
    }
}
