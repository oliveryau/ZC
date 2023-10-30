using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_SingleLifesteal : NormalAttack
{
    [Header("Effects")]
    [SerializeField] private int lifestealPercent;
    private float lifestealAmount;

    public override void Attack(GameObject target)
    {
        CalculateDamage(target);
        targetStats.TakeDamage(damage, critCheck, null);

        Healing healing = FindObjectOfType<Healing>();
        lifestealAmount = healing.LifeStealCalculation(lifestealPercent, damage);
        attackerStats.HealBuff(lifestealAmount, false);

        critCheck = false;

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
        //critCheck = false;
    }
}
