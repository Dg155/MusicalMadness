using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumpet : Weapon
{
    public GameObject projectile;
    public Transform projectileTransform;
    private int damage = 25;
    private int attackSpeed = 1;
    private float coolDown = 0.5f;
    private bool canFire = true;

    private void Update() {
        if (canFire && Input.GetMouseButton(0))
        {
            StartCoroutine("Use");
        }
        Render();
    }

    override protected IEnumerator Use()
    {
        canFire = false;
        Instantiate(projectile, projectileTransform.position, Quaternion.identity);
        yield return new WaitForSeconds(coolDown);
        canFire = true;
    }
}
