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
        targetStats.stunCounter += stunTurns;

        critCheck = false;
    }
}
