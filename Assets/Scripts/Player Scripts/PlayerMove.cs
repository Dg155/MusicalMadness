using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;

    PlayerStats playerStats;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = this.GetComponent<PlayerStats>();
    }

    public void Move(float horizontal, float vertical)
    {
        rb.velocity = new Vector2(horizontal * playerStats.getMoveSpeed(), vertical * playerStats.getMoveSpeed());
    }
}
