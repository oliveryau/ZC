using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NormalAttack : _BaseAttack
{
    public void Attack(GameObject target)
    {
        CalculateDamage(target);

        //owner.GetComponent<Animator>().Play(animationName);
        targetStats.TakeDamage(damage);
    }
}
