using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ViolinStats : BaseStats
{
    public int soulsDropped;
    public float directionChangeMin = 1f;
    public float directionChangeMax = 3f;
    private bool newDirection = true;
    Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        Render();
        //StartCoroutine("Move");
    }

    private IEnumerator Move()
    {
        if (newDirection)
        {
            newDirection = false;
            float horizontalVelocity = Random.Range(-1f,1f);
            float verticalVelocity = Random.Range(-1f,1f);
            rb.velocity = new Vector2(horizontalVelocity * getMoveSpeed(), verticalVelocity * getMoveSpeed());
            yield return new WaitForSeconds(Random.Range(directionChangeMin, directionChangeMax));
            newDirection = true;
        }
    }

    override protected void Die(){
        for(int i =0; i < soulsDropped; i++)
        {
            Debug.Log("Drop a soul");
        }
    }

    protected override void Render()
    {
        //This entire render stuff should be moved to a separate script later
        
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        if(playerPos.x - this.transform.position.x > 0 && !facingRight || playerPos.x - this.transform.position.x < 0 && facingRight)
        {
            flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other);
    }
}
