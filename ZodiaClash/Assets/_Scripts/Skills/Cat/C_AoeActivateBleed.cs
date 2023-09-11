using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AoeActivateBleed : AoeAttack
{
    [Header("Effects")]
    [SerializeField] private int bleedTurns;

    public override void Attack(GameObject[] targets)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        _Bleed bleed = FindObjectOfType<_Bleed>();

        foreach (GameObject target in targets)
        {
            CalculateDamage(target);

            CharacterStats currentTarget = target.GetComponent<CharacterStats>();

            if (currentTarget.bleedStack > 0)
            {
                bleed.AoeBleedCalculation(currentTarget, currentTarget.bleedStack);
                currentTarget.TakeDamage(damage + bleed.bleedDamage, critCheck, "rend");

                currentTarget.bleedStack = 0;
            }
            else
            {
                currentTarget.bleedStack += bleedTurns;
                currentTarget.TakeDamage(damage, critCheck, "bleed");
            }
        }

        critCheck = false;
    }
}
