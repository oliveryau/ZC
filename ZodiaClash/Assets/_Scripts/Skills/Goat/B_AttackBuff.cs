using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AttackBuff : _BaseBuff
{
    [Header("Effects")]
    [SerializeField] private int buffCount;

    private int buffAmount;

    public void AttackBuff(GameObject target)
    {
        CalculateBuff(target);

        buffAmount = Mathf.RoundToInt((skillBuffPercent / 100) *
            targetStats.attack);

        Debug.Log("Attack Buff: " + buffAmount);

        targetStats.attackBuffCounter += buffCount;
        targetStats.attack += buffAmount;
    }
}
