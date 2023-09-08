using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : _BaseAttack
{
    public virtual void Attack(GameObject target)
    {
        CalculateDamage(target);

        //owner.GetComponent<Animator>().Play(animationName);
        Debug.Log("Attacked " + target.name);
        target.GetComponent<CharacterStats>().TakeDamage(damage);
    }
}
