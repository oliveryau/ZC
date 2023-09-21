using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AttackBuff : _BaseBuff
{
    [Header("Effects")]
    [SerializeField] private int enrageTurns;

    public void AttackBuff(GameObject target)
    {
        GetTargets(target);

        Enrage enrage = FindObjectOfType<Enrage>();

        targetStats.BuffText(skillBuffPercent, "enrage");

        if (targetStats.attackBuffCounter <= 0) //don't overstack attack
        {
            enrage.EnrageCalculation(targetStats, skillBuffPercent);
        }

        StatusEffectHud statusEffect = FindObjectOfType<StatusEffectHud>();
        statusEffect.SpawnEffectsBar(targetStats, enrageTurns, "enrage");

        targetStats.attackBuffCounter += enrageTurns;
        if (targetStats.attackBuffCounter > enrage.enrageLimit)
        {
            targetStats.attackBuffCounter = enrage.enrageLimit;
        }
    }
}
