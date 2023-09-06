using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttack : _BaseAttack
{
    public void Attack(GameObject[] targets)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        foreach (GameObject target in targets)
        {
            CalculateDamage(target);
            Debug.Log("Attacked " + target.name);
            targetStats.TakeDamage(damage);
        }
    }
}
