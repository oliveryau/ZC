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
        //owner.GetComponent<Animator>().Play(animationName);

        CalculateDamage(target);
        targetStats.TakeDamage(damage, critCheck, null);

        Healing healing = FindObjectOfType<Healing>();
        lifestealAmount = healing.LifeStealCalculation(lifestealPercent, damage);
        attackerStats.HealBuff(lifestealAmount, false);

        critCheck = false;
    }
}
