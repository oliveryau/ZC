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
        base.Attack(target);

        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= bleedRate)
        {
            Debug.LogError("Bleed Applied");

            targetStats.bleedStack.Add(bleedCount);
        }
    }

    public void ApplyBleed(CharacterStats bleedTarget) //universal bleed formula
    {
        //calculate and apply bleed per turn
        bleedDamage = 0.1f * bleedTarget.maxHealth;

        bleedTarget.TakeDamage((int)bleedDamage, false);
    }
}
