using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Trumpet : Weapon
{
    public GameObject projectile;
    public GameObject projectileSecondary;
    public Transform projectileTransform;
    
    public float bulletSpeed;
    public float bulletSpeedSecondary;

    public GameObject animCombo1, animCombo2, animCombo3;
    List<weaponMove> combo1 = new List<weaponMove>();
    List<weaponMove> combo2 = new List<weaponMove>();
    List<weaponMove> combo3 = new List<weaponMove>();

    [SerializeField] attackInfo comboFinisher1;
    [SerializeField] attackInfo comboFinisher2;
    [SerializeField] attackInfo comboFinisher3;

    public AudioClip soundEffect;

    private void Awake()
    {
        gameObject.SetActive(true);
        FindObjectOfType<SoundEffectPlayer>().PlaySound(soundEffect);
    }

    new private void Start()
    {
        base.Start();
        primaryRanged = true;
        secondaryRanged = true;
        primaryMove = weaponMove.trumpetPrimary;
        secondaryMove = weaponMove.trumpetSecondary;
        maxComboLength = 4;
        comboLossTimeLimit = 1.25f;

        //q = quarter note, h = half note

        // set combo1: q-q-h --> baby explosion w/ baby AOE & knockback
        combo1.Add(weaponMove.trumpetPrimary);
        combo1.Add(weaponMove.trumpetPrimary);
        combo1.Add(weaponMove.trumpetSecondary);

        // set combo2: h-q-q-h --> explosion w/ AOE that stuns
        combo2.Add(weaponMove.trumpetSecondary);
        combo2.Add(weaponMove.trumpetPrimary);
        combo2.Add(weaponMove.trumpetPrimary);
        combo2.Add(weaponMove.trumpetSecondary);

        // set combo3: h-h-q-h --> chunky explosion w/ AOE, knockback, & stun
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
            comboAttack += comboFinisher1; //new total damage of trumpet secondary attack: 75

            comboAttack.targetNewDrag = comboFinisher1.targetNewDrag;
            comboAttack.animCol = animCombo1;

            bulletSpeedSecondary = 10;

            ClearLastMoves();
            return comboAttack;
        }
        if (LastMovesUsed.SequenceEqual(combo2))
        {
            comboAttack += comboFinisher2; //new damage: 95

            comboAttack.animCol = animCombo2;

            bulletSpeedSecondary = 8;

            ClearLastMoves();
            return comboAttack;
        }
        if (LastMovesUsed.SequenceEqual(combo3))
        {
            comboAttack += comboFinisher3; //new damage: 125

            comboAttack.targetNewDrag = comboFinisher3.targetNewDrag;
            comboAttack.animCol = animCombo3;

            bulletSpeedSecondary = 7.5f;

            ClearLastMoves();
            return comboAttack;
        }
        //no combo finisher --> default

        bulletSpeedSecondary = 5;

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
