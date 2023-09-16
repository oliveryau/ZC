using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class B_AoeBleed : AoeAttack
{
    [Header("Effects")]
    [SerializeField] private float bleedRate;
    [SerializeField] private int bleedTurns;

    [Header("Chi Cost")]
    [SerializeField] private int chiCost;

    public override void Attack(GameObject[] targets)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        _Bleed bleed = FindObjectOfType<_Bleed>();

        foreach (GameObject target in targets)
        {
            CalculateDamage(target);

            CharacterStats currentTarget = target.GetComponent<CharacterStats>();

            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= bleedRate)
            {
                if (currentTarget.bleedStack < bleed.bleedLimit)
                {
                    currentTarget.TakeDamage(damage, critCheck, "bleed");
                    currentTarget.bleedStack += bleedTurns;

                    if (currentTarget.bleedStack > bleed.bleedLimit)
                    {
                        currentTarget.bleedStack = bleed.bleedLimit;
                    }
                }
            }
            else
            {
                currentTarget.TakeDamage(damage, critCheck, null);
            }
        }

        critCheck = false;
    }
}
