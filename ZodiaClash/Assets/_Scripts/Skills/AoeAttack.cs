using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttack : _BaseAttack
{
    public virtual void Attack(GameObject[] targets)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        foreach (GameObject target in targets)
        {
            CalculateDamage(target);

            target.GetComponent<CharacterStats>().TakeDamage(damage, critCheck);
        }

        critCheck = false;
    }
}
