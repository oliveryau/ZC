using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_AttackDefBreak : NormalAttack
{
    [Header("Effects")]
    [SerializeField] private float defBreakRate;
    [SerializeField] private int defBreakCount;
    [SerializeField] private int defBreakPercent;

    public override void Attack(GameObject target)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        CalculateDamage(target);

        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= defBreakRate)
        {
            if (targetStats.defBreakCounter <= 0)
            {
                targetStats.TakeDamage(damage, critCheck, "defBreak");
                
                targetStats.defense *= 1 - (defBreakPercent / 100f);
            }

            targetStats.defBreakCounter = defBreakCount;

            StatusEffectManager statusEffect = FindObjectOfType<StatusEffectManager>();
            statusEffect.SpawnEffectsBar(targetStats, defBreakCount, "defBreak");
        }
        else
        {
            targetStats.TakeDamage(damage, critCheck, null);
        }

        critCheck = false;
    }
}
