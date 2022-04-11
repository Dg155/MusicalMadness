using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumpet : Weapon
{
    public GameObject projectile;
    public Transform projectileTransform;
    public attackInfo attack;
    public int bulletSpeed = 1;
    private float coolDown = 0.5f;
    private bool canFire = true;

    override public IEnumerator Use()
    {
        if (canFire){
            canFire = false;
            spawnProjectile();
            yield return new WaitForSeconds(coolDown);
            canFire = true;
        }
    }

    void spawnProjectile(){
        GameObject proj = Instantiate(projectile, projectileTransform.position, Quaternion.identity);
        proj.GetComponent<QuarterNoteProjectile>().setAttackInfo(attack);//change to bullet script, use inheritance
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - proj.transform.position;
        Vector3 rotation = proj.transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, rot+90);
    }
}
