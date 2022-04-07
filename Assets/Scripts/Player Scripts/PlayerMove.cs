using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    float horizontal, vertical;

    PlayerStats playerStats;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = this.GetComponent<PlayerStats>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical"); 
    }
    void FixedUpdate(){
           rb.velocity = new Vector2(horizontal * playerStats.getMoveSpeed(), vertical * playerStats.getMoveSpeed());
    }
}
