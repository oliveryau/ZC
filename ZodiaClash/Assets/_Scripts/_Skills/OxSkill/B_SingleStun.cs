using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_SingleStun : NormalAttack
{
    [Header("Effects")]
    [SerializeField] private int stunTurns;

    public override void Attack(GameObject target)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        CalculateDamage(target);

        targetStats.TakeDamage(damage, critCheck, "stun");

        Stun stun = FindObjectOfType<Stun>();
        stun.StunCalculation(targetStats, stunTurns);

        _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>();
        statusEffect.SpawnEffectsBar(targetStats, stunTurns, "stun");

        critCheck = false;
    }
}
