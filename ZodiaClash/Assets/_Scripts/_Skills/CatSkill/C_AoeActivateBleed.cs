using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AoeActivateBleed : AoeAttack
{
    public override void Attack(GameObject[] targets)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        Bleed bleed = FindObjectOfType<Bleed>();

        foreach (GameObject target in targets)
        {
            CalculateDamage(target);

            CharacterStats currentTarget = target.GetComponent<CharacterStats>();

            if (currentTarget.bleedStack > 0)
            {
                bleed.AoeBleedCalculation(currentTarget, currentTarget.bleedStack);
                currentTarget.TakeDamage(damage + bleed.bleedDamage, critCheck, "rend");

                currentTarget.bleedStack = 0; //set to 0 first

                _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>(); //remove status effect icon
                statusEffect.UpdateEffectsBar(currentTarget, "bleed");
            }
            else
            {
                //normal aoe attack
                currentTarget.TakeDamage(damage, critCheck, null);
            }
        }

        critCheck = false;
    }
}
