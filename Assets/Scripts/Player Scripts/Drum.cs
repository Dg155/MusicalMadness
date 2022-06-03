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

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    new private void Start()
    {
        base.Start();
        primaryRanged = false;
        secondaryRanged = false;
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

    void SwitchRanged() //switches primary/secondaryRanged to true if combo has been completed (temporary func that we should likely delete if we improve weapon implementation)
    {
        if (LastMovesUsed.Take(3).SequenceEqual(combo1))
        {
            secondaryRanged = true;
        }
        if (LastMovesUsed.SequenceEqual(combo2))
        {
            secondaryRanged = true;
        }
        if (LastMovesUsed.SequenceEqual(combo3))
        {
            primaryRanged = true;
        }
    }

    override protected attackInfo CalculateComboDamage()
    {
        attackInfo comboAttack = new attackInfo();
        if (LastMovesUsed.Take(3).SequenceEqual(combo1))
        {
            comboAttack += comboFinisher1; //new total damage of drum secondary attack: 80

            comboAttack.targetNewDrag = comboFinisher1.targetNewDrag;
            comboAttack.animCol = animCombo1;
            comboAttack.isPiercing = true;

            bulletSpeedComboProj = 5;

            ClearLastMoves(true);
            return comboAttack;
        }
        if (LastMovesUsed.SequenceEqual(combo2))
        {
            comboAttack += comboFinisher2; //new damage: 35

            comboAttack.targetNewDrag = comboFinisher1.targetNewDrag;
            comboAttack.animCol = animCombo2;
            comboAttack.isPiercing = true;

            bulletSpeedComboProj = 7.5f;

            ClearLastMoves(true);
            return comboAttack;
        }
        if (LastMovesUsed.SequenceEqual(combo3))
        {
            comboAttack += comboFinisher3; //new damage: 100

            comboAttack.targetNewDrag = comboFinisher3.targetNewDrag;
            comboAttack.animCol = animCombo3;
            comboAttack.isPiercing = true;

            bulletSpeedComboProj = 10;

            ClearLastMoves(true);
            return comboAttack;
        }
        //no combo finisher --> default

        comboAttack.targetNewDrag = 6.5f;

        return comboAttack;
    }

    override public void meleeAttack(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {
        SwitchRanged();
        if (primaryRanged)
        {
            //If this move is a combo finisher, emit a seismic slam
            GameObject proj = Instantiate(comboProjectile, projectileTransform.position, Quaternion.identity);
            proj.GetComponent<ProjectileBase>().boostAttack(CalculateComboDamage());
            proj.GetComponent<ProjectileBase>().setCourseOfFire(bulletSpeedComboProj, facingRight, shootPos, targetTags);
        }
        else
        {
            boostMelee(CalculateComboDamage());
            FindObjectOfType<SoundEffectPlayer>().PlaySound(soundEffectL);
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
        primaryRanged = false;
        secondaryRanged = false;
    }

    override public void meleeAttackSecondary(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {
        SwitchRanged();
        if (secondaryRanged)
        {
            //If this move is a combo finisher, emit a seismic slam
            GameObject proj = Instantiate(comboProjectile, projectileTransform.position, Quaternion.identity);
            proj.GetComponent<ProjectileBase>().boostAttack(CalculateComboDamage());
            proj.GetComponent<ProjectileBase>().setCourseOfFire(bulletSpeedComboProj, facingRight, shootPos, targetTags);
        }
        else
        {
            boostMeleeSecondary(CalculateComboDamage());
            FindObjectOfType<SoundEffectPlayer>().PlaySound(soundEffectL);
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
        primaryRanged = false;
        secondaryRanged = false;
    }
}
