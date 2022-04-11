using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct attackInfo{ //stores all info of the weapon when it collides an enemy:
//the damage, the length of stun, the length of blinding, the poison damage, the knockback
    public float damage;
    public float stunDuration;
    public float blindDuration;
    public float poisonDuration;
    public float poisonDamage;


}



public class Weapon : MonoBehaviour
{
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Render();
    }

    public virtual IEnumerator Use()
    {
        yield return new WaitForSeconds(1f);
    }

    protected virtual void Render()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
