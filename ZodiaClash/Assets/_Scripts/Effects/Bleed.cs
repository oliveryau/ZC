using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : MonoBehaviour
{
    public int bleedLimit;
    [SerializeField] private int bleedPercent;

    [HideInInspector] public float bleedDamage; //damage of bleed

    public void BleedCalculation(CharacterStats bleedTarget) //universal bleed formula
    {
        bleedDamage = Mathf.RoundToInt(
            (bleedPercent / 100f) * bleedTarget.maxHealth);
    }

    public void AoeBleedCalculation(CharacterStats currentBleedTarget, int bleedStacks)
    {
        bleedDamage = Mathf.RoundToInt(
            (bleedPercent / 100f) * currentBleedTarget.maxHealth
            * bleedStacks);
    }
}
