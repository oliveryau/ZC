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
        base.Attack(target);

        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= defBreakRate)
        {
            Debug.Log("Defense Break");

            targetStats.defense *= 1 - (defBreakPercent / 100f);
            targetStats.defBreakCounter += defBreakCount;
        }
    }
}
