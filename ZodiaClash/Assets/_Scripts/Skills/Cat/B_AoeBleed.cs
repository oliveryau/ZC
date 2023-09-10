using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class B_AoeBleed : AoeAttack
{
    [Header("Effects")]
    [SerializeField] private float bleedRate; //bleed chance
    [SerializeField] private int bleedCount; //number of turns to bleed

    public override void Attack(GameObject[] targets)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        foreach (GameObject target in targets)
        {
            CalculateDamage(target);

            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= bleedRate)
            {
                target.GetComponent<CharacterStats>().TakeDamage(damage, critCheck, "bleed");

                target.GetComponent<CharacterStats>().bleedStack.Add(bleedCount);
            }
            else
            {
                target.GetComponent<CharacterStats>().TakeDamage(damage, critCheck, null);
            }
        }

        critCheck = false;
    }
}
