using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SingleHeal : _BaseBuff
{
    [Header("Effects")]
    private float healAmount;

    public void Heal(GameObject target)
    {
        CalculateBuff(target);

        healAmount = Mathf.RoundToInt((skillBuffPercent / 100) *
            targetStats.maxHealth);

        Debug.Log("Heal Amount: " + healAmount);

        targetStats.HealBuff(healAmount);
        ResetStatus();
    }

    private void ResetStatus()
    {
        if (targetStats.bleedStack.Count > 0)
        {
            targetStats.bleedStack.Clear();
        }
    }
}
