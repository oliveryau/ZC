using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    private float healValue;

    public float HealCalculation(CharacterStats healTarget, int healPercent)
    {
        healValue = Mathf.RoundToInt
            ((healPercent / 100f) * healTarget.maxHealth);

        return healValue;
    }

    public float LifeStealCalculation(int lifeStealPercent, float damage)
    {
        healValue = Mathf.RoundToInt
            ((lifeStealPercent / 100f) * damage);

        return healValue;
    }
}
