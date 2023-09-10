using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_SingleBleed : NormalAttack
{
    [Header("Effects")]
    [SerializeField] private float bleedRate; //bleed chance
    [SerializeField] private int bleedCount; //number of turns to bleed

    private float bleedDamage; //damage of bleed

    public override void Attack(GameObject target)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        CalculateDamage(target);

        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= bleedRate)
        {
            targetStats.TakeDamage(damage, critCheck, "bleed");

            targetStats.bleedStack.Add(bleedCount);
        }
        else
        {
            targetStats.TakeDamage(damage, critCheck, null);
        }

        critCheck = false;
    }

    public void ApplyBleed(CharacterStats bleedTarget) //universal bleed formula
    {
        //calculate and apply bleed per turn
        bleedDamage = 0.1f * bleedTarget.maxHealth;

        bleedTarget.TakeDamage((int)bleedDamage, false, "bleed");
    }
}
