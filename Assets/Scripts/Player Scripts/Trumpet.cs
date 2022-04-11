using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumpet : Weapon
{
    public GameObject projectile;
    public Transform projectileTransform;
    private int damage = 25;
    private int bulletSpeed = 1;
    private float coolDown = 0.5f;
    private bool canFire = true;

    override public IEnumerator Use()
    {
        if (canFire){
            canFire = false;
            Instantiate(projectile, projectileTransform.position, Quaternion.identity);
            yield return new WaitForSeconds(coolDown);
            canFire = true;
        }
    }
}
