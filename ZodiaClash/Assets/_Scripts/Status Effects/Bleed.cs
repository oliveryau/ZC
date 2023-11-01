using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : MonoBehaviour
{
    public int bleedLimit;
    public int bleedPercent;

    [HideInInspector] public float bleedDamage;
    private CharacterStats catStats;

    private void Start()
    {
        CatAction catAction = FindObjectOfType<CatAction>();
        catStats = catAction.GetComponent<CharacterStats>();
    }

    public void BleedCalculation(CharacterStats bleedTarget) //universal bleed formula
    {
        bleedDamage = Mathf.RoundToInt(
            (bleedPercent / 100f) * catStats.attack);
    }

    public void AoeBleedCalculation(CharacterStats currentBleedTarget, int bleedStacks)
    {
        bleedDamage = Mathf.RoundToInt(
            (bleedPercent / 100f) * catStats.attack
            * bleedStacks);
    }
}
