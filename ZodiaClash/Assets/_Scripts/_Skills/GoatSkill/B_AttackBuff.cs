using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AttackBuff : _BaseBuff
{
    [Header("Effects")]
    public int enrageTurns;

    public void AttackBuff(GameObject target)
    {
        GetTargets(target);

        Enrage enrage = FindObjectOfType<Enrage>();

        targetStats.BuffText(skillBuffPercent, "enrage");

        if (targetStats.enrageCounter <= 0) //don't overstack attack
        {
            enrage.EnrageCalculation(targetStats, skillBuffPercent);
        }

        _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>();
        statusEffect.SpawnEffectsBar(targetStats, enrageTurns, "enrage");

        targetStats.enrageCounter += enrageTurns;
        if (targetStats.enrageCounter > enrage.enrageLimit)
        {
            targetStats.enrageCounter = enrage.enrageLimit;
        }

        BattleManager gameManager = FindObjectOfType<BattleManager>();
        gameManager.SwitchTurnOrder(targetStats);
        targetStats.speedCheck = true;
    }
}
