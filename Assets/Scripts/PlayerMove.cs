using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    float horizontal, vertical;

    public float runSpeed = 20.0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical"); 
    }
    void FixedUpdate(){
           rb.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}
