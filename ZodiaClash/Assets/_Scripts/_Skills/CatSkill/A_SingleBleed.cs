using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_SingleBleed : NormalAttack
{
    [Header("Effects")]
    [SerializeField] private float bleedRate;
    public int bleedTurns;

    public override void Attack(GameObject target)
    {
        CalculateDamage(target);

        Bleed bleed = FindObjectOfType<Bleed>();

        targetStats.TakeDamage(damage, critCheck, "bleed"); //actual damage

        if (targetStats.bleedStack < bleed.bleedLimit)
        {
            _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>(); //status effect icon
            statusEffect.SpawnEffectsBar(targetStats, bleedTurns, "bleed");

            targetStats.bleedStack += bleedTurns;
            if (targetStats.bleedStack > bleed.bleedLimit) //dont overstack bleed
            {
                targetStats.bleedStack = bleed.bleedLimit;
            }
        }

        critCheck = false;
    }
}
