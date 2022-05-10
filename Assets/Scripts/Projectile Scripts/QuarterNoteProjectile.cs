using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarterNoteProjectile : ProjectileBase
{
    public override void setCourseOfFire(int bulletSpeed, bool facingRight)
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        if (facingRight){transform.rotation = Quaternion.Euler(0, 0, rot + 180);}
        else{transform.rotation = Quaternion.Euler(0, 0, rot);}
    }

}
