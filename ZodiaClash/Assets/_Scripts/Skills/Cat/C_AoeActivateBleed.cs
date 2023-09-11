using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AoeActivateBleed : AoeAttack
{
    [Header("Effects")]
    [SerializeField] private int bleedCount; //number of turns to bleed

    public override void Attack(GameObject[] targets)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        foreach (GameObject target in targets)
        {
            CalculateDamage(target);

            CharacterStats enemy = target.GetComponent<CharacterStats>();

            enemy.TakeDamage(damage, critCheck, null);

            if (enemy.bleedStack.Count > 0)
            {
                if (enemy.bleedStack.Count < targetStats.bleedLimit)
                {
                    //add bleed count
                    enemy.bleedStack.Add(bleedCount);
                }

                for (int i = 0; i < enemy.bleedStack.Count; i++)
                {
                    if (enemy.bleedStack[i] > 0) //apply bleed if there is bleed stack
                    {
                        A_SingleBleed bleed = FindObjectOfType<A_SingleBleed>();
                        bleed.ApplyBleed(enemy);

                        --enemy.bleedStack[i];
                    }
                }

                for (int j = 0; j < enemy.bleedStack.Count; j++)
                {
                    if (enemy.bleedStack[j] <= 0) //remove bleed status
                    {
                        enemy.bleedStack.RemoveAt(j);
                    }
                }
            }
            else
            {
                enemy.TakeDamage(damage, critCheck, null); //aoe damage if no bleed stacks
            }
        }

        critCheck = false;
    }
}
