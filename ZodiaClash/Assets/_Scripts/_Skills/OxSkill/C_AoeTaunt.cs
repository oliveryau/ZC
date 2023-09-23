using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AoeTaunt : AoeAttack
{
    [Header("Effects")]
    [SerializeField] private float tauntRate;
    public int tauntTurns;
    [SerializeField] private int armorPercent;
    public int armorTurns;

    public override void Attack(GameObject[] targets)
    {
        //owner.GetComponent<Animator>().Play(animationName);

        base.Attack(targets);

        Taunt(targets);

        Armor();
    }

    public void Taunt(GameObject[] targets)
    {
        Taunt taunt = FindObjectOfType<Taunt>();
        taunt.AoeTauntCalculation(owner, targets, tauntTurns, tauntRate);
    }

    public void Armor()
    {
        Defense armor = FindObjectOfType<Defense>();

        attackerStats.BuffText(armorPercent, "armor");

        if (attackerStats.armorCounter <= 0) //dont overstack defense
        {
            armor.ArmorCalculation(attackerStats, armorPercent);
        }

        _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>();
        statusEffect.SpawnEffectsBar(attackerStats, armorTurns, "armor");

        attackerStats.armorCounter += armorTurns;
        if (attackerStats.armorCounter > armor.armorLimit)
        {
            attackerStats.armorCounter = armor.armorLimit;
        }
    }
}
