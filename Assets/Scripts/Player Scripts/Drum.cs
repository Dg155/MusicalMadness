using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Drum : Weapon
{
    public GameObject projectileSecondary;
    public Transform projectileTransform;

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
        primaryRanged = false;
        secondaryRanged = true;
        primaryMove = weaponMove.drumPrimary;
        secondaryMove = weaponMove.drumSecondary;
        maxComboLength = 4;
        comboLossTimeLimit = 1.5f;

        //b = boom, s = seismic slam

        // set combo1: b-b-s --> strong woosh
        combo1.Add(weaponMove.drumPrimary);
        combo1.Add(weaponMove.drumPrimary);
        combo1.Add(weaponMove.drumSecondary);

        // set combo2: s-s-b --> flashbang; small boom w/ extremely long stun
        combo2.Add(weaponMove.drumSecondary);
        combo2.Add(weaponMove.drumSecondary);
        combo2.Add(weaponMove.drumPrimary);

        // set combo3: s-b-s-b --> megaboosh that covers entire room
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
            comboAttack += comboFinisher1; //new total damage of drum secondary attack: 75

            comboAttack.targetNewDrag = comboFinisher1.targetNewDrag;
            comboAttack.animCol = animCombo1;

            bulletSpeedSecondary = 10;

            ClearLastMoves();
            return comboAttack;
        }
        if (LastMovesUsed.SequenceEqual(combo2))
        {
            comboAttack += comboFinisher2; //new damage: 50

            comboAttack.targetNewDrag = comboFinisher1.targetNewDrag;
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

        comboAttack.targetNewDrag = 6.5f;
        bulletSpeedSecondary = 5;

        return comboAttack;
    }

    override public void meleeAttack(HashSet<string> targetTags)
    {
        boostMelee(CalculateComboDamage());
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

    override public void spawnProjectileSecondary(bool facingRight, Vector3 shootPos, HashSet<string> targetTags)
    {
        GameObject proj = Instantiate(projectileSecondary, projectileTransform.position, Quaternion.identity);
        proj.GetComponent<ProjectileBase>().boostAttack(CalculateComboDamage());
        proj.GetComponent<ProjectileBase>().setCourseOfFire(bulletSpeedSecondary, facingRight, shootPos, targetTags);
    }
}
