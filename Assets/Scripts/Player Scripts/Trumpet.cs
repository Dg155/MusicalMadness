using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Trumpet : Weapon
{
    public GameObject projectile;
    public GameObject projectileSecondary;
    public Transform projectileTransform;
    
    public int bulletSpeed;
    public int bulletSpeedSecondary;

    public GameObject animCombo1, animCombo2, animCombo3;
    List<weaponMove> combo1 = new List<weaponMove>();
    List<weaponMove> combo2 = new List<weaponMove>();
    List<weaponMove> combo3 = new List<weaponMove>();

    private void Awake()
    {

        gameObject.SetActive(true);
    }

    new private void Start() {
        base.Start();
        ranged = true;
        primaryMove = weaponMove.trumpetPrimary;
        secondaryMove = weaponMove.trumpetSecondary;
        maxComboLength = 4;
        comboLossTimeLimit = 1.5f;

        // set combo1: q-q-h
        combo1.Add(weaponMove.trumpetPrimary);
        combo1.Add(weaponMove.trumpetPrimary);
        combo1.Add(weaponMove.trumpetSecondary);

        // set combo2: q-q-q-h
        combo2.Add(weaponMove.trumpetPrimary);
        combo2.Add(weaponMove.trumpetPrimary);
        combo2.Add(weaponMove.trumpetPrimary);
        combo2.Add(weaponMove.trumpetSecondary);

        // set combo3: h-h-q-h
        combo3.Add(weaponMove.trumpetSecondary);
        combo3.Add(weaponMove.trumpetSecondary);
        combo3.Add(weaponMove.trumpetPrimary);
        combo3.Add(weaponMove.trumpetSecondary);
    }

    override protected attackInfo CalculateComboDamage()
    {
        attackInfo comboAttack = new attackInfo();
        if (LastMovesUsed.Take(3).SequenceEqual(combo1)) // Personal Note: since lists are reference types, we can't use == to compare them like we can with value types. == will check if 2 lists refer to the same object, not if they have the same values.
        {
            Debug.Log("You did combo1!");
            comboAttack.damage = 25; //new total damage of trumpet secondary attack: 75
            comboAttack.animCol = animCombo1;
            LastMovesUsed.Clear();
            return comboAttack;
        }
        if (LastMovesUsed.SequenceEqual(combo2))
        {
            Debug.Log("You did combo2!");
            comboAttack.damage = 45; //new damage: 95
            comboAttack.animCol = animCombo2;
            LastMovesUsed.Clear();
            return comboAttack;
        }
        if (LastMovesUsed.SequenceEqual(combo3))
        {
            Debug.Log("You did combo3!");
            comboAttack.damage = 75; //new damage: 125
            comboAttack.animCol = animCombo3;
            LastMovesUsed.Clear();
            return comboAttack;
        }
        comboAttack.damage = 0; //new damage: default
        return comboAttack;
    }

    override public void spawnProjectile(bool facingRight, Vector3 shootPos, HashSet<string> targetTags){
        GameObject proj = Instantiate(projectile, projectileTransform.position, Quaternion.identity);
        proj.GetComponent<ProjectileBase>().boostAttack(CalculateComboDamage());
        proj.GetComponent<ProjectileBase>().setCourseOfFire(bulletSpeed, facingRight, shootPos, targetTags);
    }

    override public void spawnProjectileSecondary(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {
        GameObject proj = Instantiate(projectileSecondary, projectileTransform.position, Quaternion.identity);
        proj.GetComponent<ProjectileBase>().boostAttack(CalculateComboDamage());
        proj.GetComponent<ProjectileBase>().setCourseOfFire(bulletSpeedSecondary, facingRight, shootPos, targetTags);
    }
}
