using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_AttackDefBreak : NormalAttack
{
    [Header("Effects")]
    //[SerializeField] private float shatterRate;
    //public int shatterTurns;
    //public int shatterPercent;
    public float bleedRate;
    public int bleedTurns;

    public override void Attack(GameObject target)
    {
        CalculateDamage(target);

        //float randomValue = Random.Range(0f, 1f);
        //if (randomValue <= shatterRate)
        //{
        //    Defense shatter = FindObjectOfType<Defense>();

        //    targetStats.TakeDamage(damage, critCheck, "shatter"); //actual damage

        //    if (targetStats.shatterCounter <= 0) //dont overstack shatter
        //    {
        //        shatter.ShatterCalculation(targetStats, shatterPercent);
        //    }

        //    _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>(); //status effect icons
        //    statusEffect.SpawnEffectsBar(targetStats, shatterTurns, "shatter");

        //    targetStats.shatterCounter += shatterTurns;
        //    if (targetStats.shatterCounter > shatter.shatterLimit) //dont overstack shatter turns
        //    {
        //        targetStats.shatterCounter = shatter.shatterLimit;
        //    }
        //}
        //else
        //{
        //    targetStats.TakeDamage(damage, critCheck, null);
        //}

        CalculateDamage(target);

        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= bleedRate)
        {
            Bleed bleed = FindObjectOfType<Bleed>();

            targetStats.TakeDamage(damage, critCheck, "bleed"); //actual damage

            if (targetStats.bleedStack < bleed.bleedLimit)
            {
                _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>(); //status effect icon
                statusEffect.SpawnEffectsBar(targetStats, bleedTurns, "bleed");

                targetStats.bleedStack += bleedTurns;
                if (targetStats.bleedStack > bleed.bleedLimit) //dont overstack bleed
                {
                    targetStats.bleedStack = bleed.bleedLimit;
                }
            }
        }
        else
        {
            targetStats.TakeDamage(damage, critCheck, null);
        }

        critCheck = false;
    }
}
