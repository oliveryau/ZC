using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttack : _BaseAttack
{
    public virtual void Attack(GameObject[] targets)
    {
        foreach (GameObject target in targets)
        {
            CalculateDamage(target);

            target.GetComponent<CharacterStats>().TakeDamage(damage, critCheck, null);
        }

        critCheck = false;
    }
}
