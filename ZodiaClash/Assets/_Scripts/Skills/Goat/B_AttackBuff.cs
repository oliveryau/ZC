using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AttackBuff : _BaseBuff
{
    [Header("Effects")]
    [SerializeField] private int buffCount;

    private float buffAmount;

    public void AttackBuff(GameObject target)
    {
        GetTargets(target);

        buffAmount = Mathf.RoundToInt(
            (skillBuffPercent / 100f) * targetStats.attack);

        Debug.LogError("Attack Buff: " + buffAmount);

        if (targetStats.attackBuffCounter <= 0)
        {
            targetStats.attack += buffAmount;
        }

        targetStats.attackBuffCounter = buffCount;
    }
}
