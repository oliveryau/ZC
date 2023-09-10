using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : _BaseAttack
{
    public virtual void Attack(GameObject target)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        CalculateDamage(target);
        target.GetComponent<CharacterStats>().TakeDamage(damage, critCheck, null);

        critCheck = false;
    }
}
