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

        healAmount = Mathf.RoundToInt
            ((skillBuffPercent / 100f) * targetStats.maxHealth);

        Debug.Log("Heal Amount: " + healAmount);

        targetStats.HealBuff(healAmount);
        ResetStatus();
    }

    private void ResetStatus()
    {
        //dispel all negative effects here
        #region Bleed
        if (targetStats.bleedStack.Count > 0)
        {
            targetStats.bleedStack.Clear();
        }
        #endregion

        #region Defense Break
        if (targetStats.defBreakCounter > 0)
        {
            targetStats.defBreakCounter = 0;            
        }
        #endregion
    }
}
