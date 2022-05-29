using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Drum : Weapon
{
    public GameObject comboProjectile;
    public Transform projectileTransform;

    public float bulletSpeedComboProj;

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
        primaryMove = weaponMove.drumPrimary;
        secondaryMove = weaponMove.drumSecondary;
        maxComboLength = 4;
        comboLossTimeLimit = 1.5f;

        //1 = small blast, 2 = big blast

        // set combo1: 1-1-2
        combo1.Add(weaponMove.drumPrimary);
        combo1.Add(weaponMove.drumPrimary);
        combo1.Add(weaponMove.drumSecondary);

        // set combo2: 1-2-2
        combo2.Add(weaponMove.drumPrimary);
        combo2.Add(weaponMove.drumSecondary);
        combo2.Add(weaponMove.drumSecondary);

        // set combo3: 2-1-2-1
        combo3.Add(weaponMove.drumSecondary);
        combo3.Add(weaponMove.drumPrimary);
        combo3.Add(weaponMove.drumSecondary);
        combo3.Add(weaponMove.drumPrimary);
    }

    override protected attackInfo CalculateComboDamage()
    {
        attackInfo comboAttack = new attackInfo();
        if (LastMovesUsed.Take(3).SequenceEqual(combo1))
        {
            comboAttack += comboFinisher1; //new total damage of drum secondary attack: 80

            comboAttack.targetNewDrag = comboFinisher1.targetNewDrag;
            comboAttack.animCol = animCombo1;

            bulletSpeedComboProj = 5;

            ClearLastMoves();
            return comboAttack;
        }
        if (LastMovesUsed.SequenceEqual(combo2))
        {
            comboAttack += comboFinisher2; //new damage: 35

            comboAttack.targetNewDrag = comboFinisher1.targetNewDrag;
            comboAttack.animCol = animCombo2;

            bulletSpeedComboProj = 7.5f;

            ClearLastMoves();
            return comboAttack;
        }
        if (LastMovesUsed.SequenceEqual(combo3))
        {
            comboAttack += comboFinisher3; //new damage: 100

            comboAttack.targetNewDrag = comboFinisher3.targetNewDrag;
            comboAttack.animCol = animCombo3;

            bulletSpeedComboProj = 10;

            ClearLastMoves();
            return comboAttack;
        }
        //no combo finisher --> default

        primaryRanged = false;
        secondaryRanged = false;

        comboAttack.targetNewDrag = 6.5f;

        return comboAttack;
    }

    override public void spawnProjectile(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {
        boostMelee(CalculateComboDamage());
        if (primaryRanged)
        {
            //If this move is a combo finisher, emit a seismic slam
            attack.isPiercing = true;
            GameObject proj = Instantiate(comboProjectile, projectileTransform.position, Quaternion.identity);
            proj.GetComponent<ProjectileBase>().boostAttack(CalculateComboDamage());
            proj.GetComponent<ProjectileBase>().setCourseOfFire(bulletSpeedComboProj, facingRight, shootPos, targetTags);
        }
        else
        {
            attack.isPiercing = false;
            attack.attackerPos = transform.position;
            var colliders = Physics2D.OverlapCircleAll(transform.position, attack.blastRadius);
            foreach (Collider2D c in colliders)
            {
                if (targetTags.Contains(c.tag))
                {
                    c.GetComponent<Combat>().ReceiveAttack(attack);
                }
            }
            resetMelee();
        }
        primaryRanged = true;
        secondaryRanged = true;
    }

    override public void spawnProjectileSecondary(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {
        boostMeleeSecondary(CalculateComboDamage());
        if (secondaryRanged)
        {
            //If this move is a combo finisher, emit a seismic slam
            secondaryAttack.isPiercing = true;
            GameObject proj = Instantiate(comboProjectile, projectileTransform.position, Quaternion.identity);
            proj.GetComponent<ProjectileBase>().boostAttack(CalculateComboDamage());
            proj.GetComponent<ProjectileBase>().setCourseOfFire(bulletSpeedComboProj, facingRight, shootPos, targetTags);
        }
        else
        {
            secondaryAttack.isPiercing = false;
            secondaryAttack.attackerPos = transform.position;
            var colliders = Physics2D.OverlapCircleAll(transform.position, secondaryAttack.blastRadius);
            foreach (Collider2D c in colliders)
            {
                if (targetTags.Contains(c.tag))
                {
                    c.GetComponent<Combat>().ReceiveAttack(secondaryAttack);
                }
            }
            resetMeleeSecondary();
        }
        primaryRanged = true;
        secondaryRanged = true;
    }
}
