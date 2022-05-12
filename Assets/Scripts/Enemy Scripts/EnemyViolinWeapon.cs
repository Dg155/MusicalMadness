using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViolinWeapon : Weapon
{
    public GameObject projectile;
    private Transform projectileTransform;
    
    public int bulletSpeed;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    new private void Start() {
        base.Start();
        ranged = true;
        projectileTransform = this.transform;
    }

    override public void spawnProjectile(bool facingRight, Vector3 shootPos, HashSet<string> targetTags){
        GameObject proj = Instantiate(projectile, projectileTransform.position, Quaternion.identity);
        proj.GetComponent<ProjectileBase>().setCourseOfFire(bulletSpeed, facingRight, shootPos, targetTags);
    }
}
