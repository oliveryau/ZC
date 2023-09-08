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
        Debug.Log("Bleed Roll: " + randomValue);

        if (randomValue <= bleedRate)
        {
            //apply bleed status effect to character
            targetStats.gameObject.GetComponent<CharacterStats>().bleedCounts += bleedCount;
        }
    }

    public void ApplyBleed(CharacterStats bleedTarget)
    {
        Debug.LogError(bleedTarget.gameObject.name);
        //calculate and apply bleed per turn
        float value = 0.05f * bleedTarget.maxHealth;

        bleedDamage = Mathf.Round(value * 10.0f) * 0.1f;

        bleedTarget.TakeDamage(bleedDamage);
    }
}
