using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarterNoteProjectile : MonoBehaviour
{
    private Vector3 mousePos;
    private Rigidbody2D rb;

    public attackInfo attack;

    void Start()
    {
        /*rb = GetComponent<Rigidbody2D>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot+90);*/
    }

    public void setAttackInfo(attackInfo a){
        attack = a;
    }
    private void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy"){
            //other.GetComponent<EnemyCombat>().ReceiveAttack(attackInfo);
        }
    }
}
