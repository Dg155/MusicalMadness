using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumpet : Weapon
{
    public GameObject projectile;
    public Transform projectileTransform;
    
    public int bulletSpeed;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    new private void Start() {
        base.Start();
        ranged = true;
    }

    override public void spawnProjectile(bool facingRight, Vector3 shootPos, HashSet<string> targetTags){
        GameObject proj = Instantiate(projectile, projectileTransform.position, Quaternion.identity);
        proj.GetComponent<ProjectileBase>().setCourseOfFire(bulletSpeed, facingRight, shootPos, targetTags);
    }
}
