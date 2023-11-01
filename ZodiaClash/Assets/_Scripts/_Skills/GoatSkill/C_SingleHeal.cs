using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SingleHeal : _BaseBuff
{
    [Header("Effects")]
    private float healAmount;

    public void Heal(GameObject target)
    {
        GetTargets(target);

        Healing healing = FindObjectOfType<Healing>();
        healAmount = healing.HealCalculation(ownerStats, skillBuffPercent);

        targetStats.HealBuff(healAmount, true);

        Cleanse cleanse = FindObjectOfType<Cleanse>();
        cleanse.RemoveAllStatus(targetStats);
    }
}
