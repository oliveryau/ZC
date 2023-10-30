using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AoeBleed : AoeAttack
{
    [Header("Effects")]
    [SerializeField] private float bleedRate;
    public int bleedTurns;

    public override void Attack(GameObject[] targets)
    {
        Bleed bleed = FindObjectOfType<Bleed>();

        foreach (GameObject target in targets)
        {
            CalculateDamage(target);

            CharacterStats currentTarget = target.GetComponent<CharacterStats>();

            currentTarget.TakeDamage(damage, critCheck, "bleed"); //actual damage

            if (currentTarget.bleedStack < bleed.bleedLimit)
            {
                    
                _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>(); //status effect icon
                statusEffect.SpawnEffectsBar(currentTarget, bleedTurns, "bleed");

                currentTarget.bleedStack += bleedTurns;
                if (currentTarget.bleedStack > bleed.bleedLimit) //dont overstack bleed
                {
                    currentTarget.bleedStack = bleed.bleedLimit;
                }
            }
        }

        critCheck = false;
    }
}
