using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    private float healValue;

    public float HealCalculation(CharacterStats goat, int healPercent)
    {
        healValue = Mathf.RoundToInt
            ((healPercent / 100f) * goat.maxHealth);

        return healValue;
    }

    public float LifeStealCalculation(int lifeStealPercent, float damage)
    {
        healValue = Mathf.RoundToInt
            ((lifeStealPercent / 100f) * damage);

        return healValue;
    }
}
